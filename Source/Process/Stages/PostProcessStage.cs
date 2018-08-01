using System;

namespace R5.RunInfoBuilder.Process
{
	internal class PostProcessStage<TRunInfo> : StageChain<TRunInfo>
		where TRunInfo : class
	{
		private Func<ProcessContext<TRunInfo>, ProcessStageResult> _callback { get; }

		internal PostProcessStage(Func<ProcessContext<TRunInfo>, ProcessStageResult> callback)
			: base(handlesType: null)
		{
			_callback = callback;
		}

		protected override (StageChainResult Result, int SkipNext) Process(
			ProgramArgument argument,
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory,
			ValidationContext validationContext)
		{
			ProcessContext<TRunInfo> context = contextFactory(argument);

			ProcessStageResult result = _callback(context);

			return GoToNextFromCallbackResult(result, argument, contextFactory, validationContext);
		}
	}
}
