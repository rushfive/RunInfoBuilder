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
		private OptionsProcessInfo<TRunInfo> _optionsInfo { get; }
		private List<string> _availableSubCommands { get; }
		private IArgumentParser _parser { get; }

		internal OptionStage(
			OptionsProcessInfo<TRunInfo> optionsInfo,
			List<string> availableSubCommands,
			IArgumentParser parser,
			ArgumentsQueue argumentsQueue,
			Func<ProcessContext<TRunInfo>, ProcessStageResult> callback)
			: base(argumentsQueue, callback)
		{
			_optionsInfo = optionsInfo;
			_availableSubCommands = availableSubCommands;
			_parser = parser;
		}
		
		protected override ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context)
		{
			while (MoreProgramArgumentsExist())
			{
				if (NextIsSubCommand())
				{
					return ProcessResult.Continue;
				}

				string next = Dequeue();

				var (type, fullKey, shortKeys, valueFromToken) = OptionTokenizer.TokenizeProgramArgument(next);

				string value = ResolveValue(valueFromToken, next);

				switch (type)
				{
					case OptionType.Full:
						ProcessFull(fullKey, value);
						break;
					case OptionType.Short:
						ProcessShort(shortKeys.Single(), value);
						break;
					case OptionType.Stacked:
						ProcessStacked(shortKeys, value);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(type), $"'{type}' is not a valid option type.");
				}

				//List<(Action<TRunInfo, object> setter, Type valueType)> setters = _optionsInfo.GetSetters(nextProgramArgument);

				//bool isStackedOption = setters.Count > 1;

				// if single, check if next is argument. if type != bool, MUST have a next argumment value so we can set.
				//                 if bool, no argument means set to true.

				// if < 1, means its stacked short keys and they must ALL be bool types
				// if next is argument, set it to that (ensuring it can be parsed into a bool)
				// if next is NOT argument, set all to true



				bool nextIsArgument = NextProgramArgumentIsOption();

			}

			return ProcessResult.End;
		}

		private string ResolveValue(string valueFromToken, string optionToken)
		{
			if (NextProgramArgumentIsOption())
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

		private void ProcessFull(string key, string valueString)
		{
			var (setter, valueType) = _optionsInfo.GetSetter(key);

			object value = GetParsedValue(valueType, valueString);
			
		}

		private void ProcessShort(char key, string valueString)
		{

		}

		private void ProcessStacked(List<char> keys, string valueString)
		{

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

		private bool NextIsSubCommand() => _availableSubCommands.Contains(Peek());

		private bool NextProgramArgumentIsOption()
		{
			if (!MoreProgramArgumentsExist())
			{
				return false;
			}

			return _optionsInfo.IsOption(Peek());
		}
	}
}
