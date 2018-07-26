using System;

namespace R5.RunInfoBuilder.Pipeline
{
	internal class PreProcessArgumentStage<TRunInfo> : ProcessPipelineStageBase<TRunInfo>
		where TRunInfo : class
	{
		private Func<ProcessArgumentContext<TRunInfo>, ProcessStageResult> _processCallback { get; }

		internal PreProcessArgumentStage(Func<ProcessArgumentContext<TRunInfo>, ProcessStageResult> processCallback)
			: base(handlesArgumentType: null)
		{
			_processCallback = processCallback;
		}

		internal override (int SkipNext, AfterProcessingStage AfterStage) Process(ProcessArgumentContext<TRunInfo> context)
		{
			ProcessStageResult result = _processCallback(context);
			return (result.SkipNextArgumentsCount, result.HandleType);
		}
	}
}
