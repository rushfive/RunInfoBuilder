using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor.Models;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class OptionStage<TRunInfo> : Stage<TRunInfo>
			where TRunInfo : class
	{
		private IArgumentParser _parser { get; }

		internal OptionStage(IArgumentParser parser)
		{
			_parser = parser;
		}
		
		internal override ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context)
		{
			while (context.ProgramArguments.HasMore())
			{
				if (context.NextIsSubCommand())
				{
					return ProcessResult.Continue;
				}

				if (!context.NextIsOption())
				{
					throw new ProcessException($"Processing failed because '{context.ProgramArguments.Peek()}' is not a valid option.",
						ProcessError.OptionExpected, context.CommandLevel);
				}

				string option = context.ProgramArguments.Dequeue(context.CommandLevel);
				
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

				// if stacked option, must ensure that all is bool type
				//bool allStackedAreBoolType = shortKeys != null
				//	&& shortKeys.Count > 1 && isBoolType;
				//if (!allStackedAreBoolType)
				//{
				//	throw new InvalidOperationException($"Stacked options can only be mapped to "
				//		+ $"boolean properties but found one or more invalid options in: {string.Join("", shortKeys)}");
				//}

				string value = ResolveValue(valueFromToken, isBoolType, context);

				switch (type)
				{
					case OptionType.Full:
						ProcessFull(fullKey, value, context);
						break;
					case OptionType.Short:
						ProcessShort(shortKeys.Single(), value, context);
						break;
					case OptionType.Stacked:
						ProcessStacked(shortKeys, value, context);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(type), $"'{type}' is not a valid option type.");
				}
			}

			return ProcessResult.End;
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

			if (context.NextIsOption())
			{
				throw new ProcessException("Expected a value for the next program argument "
					+ $"but found an option instead: '{next}'",
					ProcessError.ExpectedValueFoundOption, context.CommandLevel);
			}

			if (context.NextIsSubCommand())
			{
				throw new ProcessException("Expected a value for the next program argument "
					+ $"but found an sub command instead: '{next}'",
					ProcessError.ExpectedValueFoundSubCommand, context.CommandLevel);
			}

			return context.ProgramArguments.Dequeue(context.CommandLevel);
		}

		private void ProcessFull(string key, string valueString, ProcessContext<TRunInfo> context)
		{
			var (setter, valueType) = context.Options.GetOptionValueSetter(key);

			object value = GetParsedValue(valueType, valueString, context.CommandLevel);

			setter(context.RunInfo, value);
		}

		private void ProcessShort(char key, string valueString, ProcessContext<TRunInfo> context)
		{
			var (setter, valueType) = context.Options.GetOptionValueSetter(key);

			object value = GetParsedValue(valueType, valueString, context.CommandLevel);

			setter(context.RunInfo, value);
		}

		private void ProcessStacked(List<char> keys, string valueString, ProcessContext<TRunInfo> context)
		{
			List<(Action<TRunInfo, object> setter, Type valueType)> setters = context.Options.GetOptionValueSetters(keys);

			object value = GetParsedValue(setters.First().valueType, valueString, context.CommandLevel);

			foreach((Action<TRunInfo, object> setter, _) in setters)
			{
				setter(context.RunInfo, value);
			}
		}

		private object GetParsedValue(Type valueType, string valueString, int commandLevel)
		{
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

				if (!_parser.TryParseAs(valueString, out bool parsed))
				{
					throw new ProcessException($"'{valueString}' could not be parsed as a 'bool' type.",
						ProcessError.ParserInvalidValue, commandLevel);
				}
				return parsed;
			}

			object getByParsing()
			{
				if (valueString == null)
				{
					throw new ProcessException("Options mapped to a non-boolean property must have a value.",
						ProcessError.OptionValueRequired, commandLevel);
				}

				if (!_parser.TryParseAs(valueType, valueString, out object parsed))
				{
					throw new ProcessException($"'{valueString}' could not be parsed as a '{valueType.Name}' type.",
						ProcessError.ParserInvalidValue, commandLevel);
				}

				return parsed;
			}
		}
	}
}
