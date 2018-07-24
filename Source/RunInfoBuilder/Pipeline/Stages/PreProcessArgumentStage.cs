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

		internal override ProcessStageResult Process(ProcessArgumentContext<TRunInfo> context)
		{
			return _processCallback(context);
		}
	}
}
