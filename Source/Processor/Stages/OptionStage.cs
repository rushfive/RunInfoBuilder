using System;
using System.Collections.Generic;
using System.Text;
using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Processor.Models;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class OptionStage<TRunInfo> : Stage<TRunInfo>
			where TRunInfo : class
	{
		private OptionsInfo<TRunInfo> _optionsInfo { get; }
		private List<string> _availableSubCommands { get; }

		internal OptionStage(
			OptionsInfo<TRunInfo> optionsInfo,
			ArgumentsQueue argumentsQueue,
			List<string> availableSubCommands,
			Func<ProcessContext<TRunInfo>, ProcessStageResult> callback)
			: base(argumentsQueue, callback)
		{
			_optionsInfo = optionsInfo;
			_availableSubCommands = availableSubCommands;
		}
		
		protected override ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context)
		{
			while (HasNext())
			{
				string nextProgramArgument = PeekNext();

				if (NextIsSubCommand(nextProgramArgument))
				{
					return ProcessResult.Continue;
				}

				nextProgramArgument = DequeueNext();

				List<(Action<TRunInfo, object> setter, Type valueType)> setters = _optionsInfo.GetSetters(nextProgramArgument);

				// if single, check if next is argument. if type != bool, MUST have a next argumment value so we can set.
				//                 if bool, no argument means set to true.

				// if < 1, means its stacked short keys and they must ALL be bool types
				// if next is argument, set it to that (ensuring it can be parsed into a bool)
				// if next is NOT argument, set all to true



				bool nextIsArgument = NextIsArgument();

				//OptionType? type = _optionsInfo.GetType(nextProgramArgument);

				//if (!type.HasValue)
				//{
				//	throw new InvalidOperationException($"'{nextProgramArgument}' is not a valid option.");
				//}

				//switch (type.Value)
				//{
				//	case OptionType.Full:
				//		break;
				//	case OptionType.Short:
				//		break;
				//	case OptionType.Stacked: // can only flag mapped options!
				//		break;
				//	default:
				//		throw new ArgumentOutOfRangeException($"'{type.Value}' is not a valid option type.");
				//}
			}

			return ProcessResult.End;
		}

		private bool NextIsSubCommand(string nextProgramArgument) => _availableSubCommands.Contains(nextProgramArgument);

		private bool NextIsArgument()
		{
			if (!HasNext())
			{
				return false;
			}

			return _optionsInfo.IsOption(PeekNext());
		}
	}
}
