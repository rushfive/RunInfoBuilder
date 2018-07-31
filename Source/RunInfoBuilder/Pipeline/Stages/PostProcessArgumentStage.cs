//using R5.RunInfoBuilder.Process;
//using System;

//namespace R5.RunInfoBuilder.Pipeline
//{
//	internal class PostProcessArgumentStage<TRunInfo> : ProcessPipelineStageBase<TRunInfo>
//		where TRunInfo : class
//	{
//		private Func<ProcessContext<TRunInfo>, ProcessStageResult> _processCallback { get; }

//		internal PostProcessArgumentStage(Func<ProcessContext<TRunInfo>, ProcessStageResult> processCallback)
//			: base(handlesArgumentType: null)
//		{
//			_processCallback = processCallback;
//		}

//		internal override (int SkipNext, AfterProcessingStage AfterStage) Process(ProcessContext<TRunInfo> context)
//		{
//			ProcessStageResult result = _processCallback(context);
//			return (result.SkipNext, result.AfterProcessing);
//		}
//	}
//}
