using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Processor
{
    internal class StageQueueCallbacks<TRunInfo>
		where TRunInfo : class
    {
		private Func<bool> _hasMoreStages { get; }
		private Func<Stage<TRunInfo>> _dequeueStage { get; }

		internal StageQueueCallbacks(
			Func<bool> hasMoreStagesCallback,
			Func<Stage<TRunInfo>> dequeueStageCallback)
		{
			_hasMoreStages = hasMoreStagesCallback;
			_dequeueStage = dequeueStageCallback;
		}

		internal bool HasMoreStages() => _hasMoreStages();

		internal Stage<TRunInfo> DequeueStage() => _dequeueStage();
	}
}
