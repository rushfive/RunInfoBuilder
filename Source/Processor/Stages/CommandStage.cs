using R5.RunInfoBuilder.Processor.Models;
using System;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class CommandStage<TRunInfo> : Stage<TRunInfo>
		where TRunInfo : class
	{
		public Func<TRunInfo, ProcessStageResult> _onMatched { get; }

		internal CommandStage(Func<TRunInfo, ProcessStageResult> onMatched)
		{
			_onMatched = onMatched;
		}

		internal override ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context,
			Action<CommandBase<TRunInfo>> resetContextFunc)
		{
			ProcessStageResult onMatchedResult = _onMatched?.Invoke(context.RunInfo);
			if (onMatchedResult == ProcessResult.End)
			{
				return ProcessResult.End;
			}

			return ProcessResult.Continue;
		}
	}
}
