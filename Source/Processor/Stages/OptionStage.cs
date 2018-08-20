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

		internal OptionStage(
			IArgumentParser parser,
			ProcessContext<TRunInfo> context)
			: base(context)
		{
			_parser = parser;
		}
		
		internal override ProcessStageResult ProcessStage()
		{
			while (MoreProgramArgumentsExist())
			{
				if (NextIsSubCommand())
				{
					return ProcessResult.Continue;
				}

				if (!NextIsOption())
				{
					throw new InvalidOperationException($"Processing failed because '{Peek()}' is not a valid option.");
				}

				string option = Dequeue();
				
				var (type, fullKey, shortKeys, valueFromToken) = OptionTokenizer.TokenizeProgramArgument(option);

				string value = ResolveValue(valueFromToken, option);

				switch (type)
				{
					case OptionType.Full:
						ProcessFull(fullKey, value, _context.RunInfo);
						break;
					case OptionType.Short:
						ProcessShort(shortKeys.Single(), value, _context.RunInfo);
						break;
					case OptionType.Stacked:
						ProcessStacked(shortKeys, value, _context.RunInfo);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(type), $"'{type}' is not a valid option type.");
				}
			}

			return ProcessResult.End;
		}

		private string ResolveValue(string valueFromToken, string optionToken)
		{
			if (NextIsOption())
			{
				return valueFromToken;
			}

			if (valueFromToken != null)
			{
				throw new InvalidOperationException($"Ambiguous option value: Contains a value in '{optionToken}' "
					+ $"but also in the next program argument '{Peek()}'");
			}

			return Dequeue();
		}

		private void ProcessFull(string key, string valueString, TRunInfo runInfo)
		{
			var (setter, valueType) = _context.GetOptionValueSetter(key);

			object value = GetParsedValue(valueType, valueString);

			setter(runInfo, value);
		}

		private void ProcessShort(char key, string valueString, TRunInfo runInfo)
		{
			var (setter, valueType) = _context.GetOptionValueSetter(key);

			object value = GetParsedValue(valueType, valueString);

			setter(runInfo, value);
		}

		private void ProcessStacked(List<char> keys, string valueString, TRunInfo runInfo)
		{
			List<(Action<TRunInfo, object> setter, Type valueType)> setters = _context.GetOptionValueSetters(keys);

			object value = GetParsedValue(setters.First().valueType, valueString);

			foreach((Action<TRunInfo, object> setter, _) in setters)
			{
				setter(runInfo, value);
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
