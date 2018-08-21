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
					throw new InvalidOperationException($"Processing failed because '{context.ProgramArguments.Peek()}' is not a valid option.");
				}

				string option = context.ProgramArguments.Dequeue();
				
				var (type, fullKey, shortKeys, valueFromToken) = OptionTokenizer.TokenizeProgramArgument(option);

				string value = ResolveValue(valueFromToken, option, context);

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

		private string ResolveValue(string valueFromToken, string optionToken, ProcessContext<TRunInfo> context)
		{
			if (context.NextIsOption() || !context.ProgramArguments.HasMore())
			{
				return valueFromToken;
			}

			string next = context.ProgramArguments.Dequeue();

			if (valueFromToken != null)
			{
				throw new InvalidOperationException($"Ambiguous option value: Contains a value in '{optionToken}' "
					+ $"but also in the next program argument '{next}'");
			}

			return next;
		}

		private void ProcessFull(string key, string valueString, ProcessContext<TRunInfo> context)
		{
			var (setter, valueType) = context.Options.GetOptionValueSetter(key);

			object value = GetParsedValue(valueType, valueString);

			setter(context.RunInfo, value);
		}

		private void ProcessShort(char key, string valueString, ProcessContext<TRunInfo> context)
		{
			var (setter, valueType) = context.Options.GetOptionValueSetter(key);

			object value = GetParsedValue(valueType, valueString);

			setter(context.RunInfo, value);
		}

		private void ProcessStacked(List<char> keys, string valueString, ProcessContext<TRunInfo> context)
		{
			List<(Action<TRunInfo, object> setter, Type valueType)> setters = context.Options.GetOptionValueSetters(keys);

			object value = GetParsedValue(setters.First().valueType, valueString);

			foreach((Action<TRunInfo, object> setter, _) in setters)
			{
				setter(context.RunInfo, value);
			}
		}

		private object GetParsedValue(Type valueType, string valueString)
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
					throw new ArgumentException($"'{valueString}' could not be parsed as a 'bool' type.", nameof(valueString));
				}
				return parsed;
			}

			object getByParsing()
			{
				if (valueString == null)
				{
					throw new ArgumentException("Options mapped to a non-boolean property must have a value.", nameof(valueString));
				}

				if (!_parser.TryParseAs(valueType, valueString, out object parsed))
				{
					throw new ArgumentException($"'{valueString}' could not be parsed as a '{valueType.Name}' type.", nameof(valueString));
				}

				return parsed;
			}
		}
	}
}
