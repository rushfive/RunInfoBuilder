using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor.Models;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class OptionStage<TRunInfo> : Stage<TRunInfo>
			where TRunInfo : class
	{
		internal OptionStage()
		{
		}
		
		internal override ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context,
			Action<CommandBase<TRunInfo>> resetContextFunc)
		{
			while (context.ProgramArguments.HasMore())
			{
				if (context.ProgramArguments.NextIsSubCommand())
				{
					return ProcessResult.Continue;
				}

				if (!context.ProgramArguments.NextIsOption())
				{
					return ProcessResult.Continue; // results are optional so just continue
				}

				string option = context.ProgramArguments.Dequeue();
				
				var (type, fullKey, shortKeys, valueFromToken) = OptionTokenizer.TokenizeProgramArgument(option);
				
				bool isBoolType = fullKey != null
					? context.Options.IsBoolType(fullKey)
					: shortKeys.All(context.Options.IsBoolType);

				if (type == OptionType.Stacked && !isBoolType)
				{
					throw new ProcessException($"Stacked options can only be mapped to "
						+ $"boolean properties but found one or more invalid options in: {string.Join("", shortKeys)}",
						ProcessError.InvalidStackedOption, context.CommandLevel);
				}

				string value = ResolveValue(valueFromToken, isBoolType, context);

				ProcessStageResult optionResult;
				switch (type)
				{
					case OptionType.Full:
						optionResult = ProcessFull(fullKey, value, context);
						break;
					case OptionType.Short:
						optionResult = ProcessShort(shortKeys.Single(), value, context);
						break;
					case OptionType.Stacked:
						optionResult = ProcessStacked(shortKeys, value, context);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(type), $"'{type}' is not a valid option type.");
				}

				switch (optionResult)
				{
					case Continue _:
						break;
					case End _:
						return ProcessResult.End;
					case null:
					default:
						throw new ProcessException(
							"Option OnProcess callback returned an unknown result.",
							ProcessError.InvalidStageResult, context.CommandLevel);
				}
			}

			return ProcessResult.Continue;
		}

		private string ResolveValue(string valueFromToken, bool isBoolTypeOption,
			ProcessContext<TRunInfo> context)
		{
			if (!string.IsNullOrWhiteSpace(valueFromToken))
			{
				return valueFromToken;
			}

			if (!context.ProgramArguments.HasMore())
			{
				if (!isBoolTypeOption)
				{
					throw new ProcessException("Expected a value but reached the end of program args.",
						ProcessError.ExpectedProgramArgument, context.CommandLevel);
				}

				// TODO: need the parser to always handle some strnig constant for true
				// if the user resets the predicate, we need to also add our own for 
				// handling this case
				return "true";
			}

			// more program args exist
			string next = context.ProgramArguments.Peek();

			if (context.ProgramArguments.NextIsOption())
			{
				if (!isBoolTypeOption)
				{
					throw new ProcessException("Expected a value for the next program argument "
					+ $"but found an option instead: '{next}'",
					ProcessError.ExpectedValueFoundOption, context.CommandLevel);
				}

				return "true";
			}

			if (context.ProgramArguments.NextIsSubCommand())
			{
				if (!isBoolTypeOption)
				{
					throw new ProcessException("Expected a value for the next program argument "
					+ $"but found an sub command instead: '{next}'",
					ProcessError.ExpectedValueFoundSubCommand, context.CommandLevel);
				}

				return "true";
			}

			return context.ProgramArguments.Dequeue();
		}

		private ProcessStageResult ProcessFull(string key, string valueString, ProcessContext<TRunInfo> context)
		{
			OptionProcessInfo<TRunInfo> processInfo = context.Options.GetOptionProcessInfo(key);

			object value = GetParsedValue(processInfo.Type, valueString, context, processInfo.OnParseErrorUseMessage);

			if (processInfo.OnParsed != null)
			{
				dynamic convertedType = Convert.ChangeType(value, processInfo.Type);

				dynamic onProcess = processInfo.OnParsed;

				ProcessStageResult onProcessResult = onProcess.Invoke(convertedType);
				if (onProcessResult == ProcessResult.End)
				{
					return ProcessResult.End;
				}
			}

			processInfo.Setter(context.RunInfo, value);

			return ProcessResult.Continue;
		}

		private ProcessStageResult ProcessShort(char key, string valueString, ProcessContext<TRunInfo> context)
		{
			OptionProcessInfo<TRunInfo> processInfo = context.Options.GetOptionProcessInfo(key);

			object value = GetParsedValue(processInfo.Type, valueString, context, processInfo.OnParseErrorUseMessage);
			
			if (processInfo.OnParsed != null)
			{
				dynamic convertedType = Convert.ChangeType(value, processInfo.Type);

				dynamic onProcess = processInfo.OnParsed;

				ProcessStageResult onProcessResult = onProcess.Invoke(convertedType);
				if (onProcessResult == ProcessResult.End)
				{
					return ProcessResult.End;
				}
			}

			processInfo.Setter(context.RunInfo, value);

			return ProcessResult.Continue;
		}

		private ProcessStageResult ProcessStacked(List<char> keys, string valueString, ProcessContext<TRunInfo> context)
		{
			List<OptionProcessInfo<TRunInfo>> processInfos = context.Options.GetOptionProcessInfos(keys);

			object value = GetParsedValue(processInfos.First().Type, valueString, 
				context, processInfos.First().OnParseErrorUseMessage);

			foreach(Action<TRunInfo, object> setter in processInfos.Select(i => i.Setter))
			{
				setter(context.RunInfo, value);
			}

			return ProcessResult.Continue;
		}

		private object GetParsedValue(Type valueType, string valueString, 
			ProcessContext<TRunInfo> context, Func<string, string> onParseErrorUseMessage)
		{
			int commandLevel = context.CommandLevel;
			ArgumentParser parser = context.Parser;

			if (valueType == typeof(string))
			{
				return valueString;
			}

			if (valueType == typeof(bool))
			{
				return getForBoolType();
			}

			return getByParsing();

			// local functions
			bool getForBoolType()
			{
				if (valueString == null)
				{
					return true;
				}

				if (!parser.TryParseAs(valueString, out bool parsed))
				{
					string message = onParseErrorUseMessage != null
						? onParseErrorUseMessage(valueString)
						: $"'{valueString}' could not be parsed as a 'bool' type.";

					throw new ProcessException(message, ProcessError.ParserInvalidValue, commandLevel);
				}
				return parsed;
			}

			object getByParsing()
			{
				if (!parser.TryParseAs(valueType, valueString, out object parsed))
				{
					string message = onParseErrorUseMessage != null
						? onParseErrorUseMessage(valueString)
						: $"'{valueString}' could not be parsed as a '{valueType.Name}' type.";

					throw new ProcessException(message, ProcessError.ParserInvalidValue, commandLevel);
				}

				return parsed;
			}
		}
	}
}
