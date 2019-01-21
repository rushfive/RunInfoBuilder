using System;
using R5.RunInfoBuilder.Processor.Models;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class DefaultCommandStage<TRunInfo> : Stage<TRunInfo>
		where TRunInfo : class
	{
		public Func<TRunInfo, ProcessStageResult> _onMatched { get; }

		internal DefaultCommandStage(Func<TRunInfo, ProcessStageResult> onMatched)
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
