using System;

namespace R5.RunInfoBuilder.Process
{
	internal abstract class StageChain<TRunInfo>
		where TRunInfo : class
	{
		private ProgramArgumentType? _handlesType { get; }
		protected StageChain<TRunInfo> _next { get; private set; }

		protected StageChain(ProgramArgumentType? handlesType)
		{
			_handlesType = handlesType;
		}

		internal StageChain<TRunInfo> SetNext(StageChain<TRunInfo> next)
		{
			_next = next;
			return next;
		}

		internal (StageChainResult Result, int SkipNext) TryProcessArgument(
			ProgramArgument argument,
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory,
			ValidationContext validationContext)
		{
			if (CanProcessArgument(argument))
			{
				return Process(argument, contextFactory, validationContext);
			}
			return GoToNext(argument, contextFactory, validationContext);
		}

		protected abstract (StageChainResult Result, int SkipNext) Process(
			ProgramArgument argument,
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory,
			ValidationContext validationContext);

		private bool CanProcessArgument(ProgramArgument argument)
		{
			if (_handlesType == null)
			{
				return true;
			}
			return argument.Type == _handlesType;
		}

		protected (StageChainResult Result, int SkipNext) GoToNext(
			ProgramArgument argument,
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory,
			ValidationContext validationContext)
		{
			if (_next != null)
			{
				return _next.TryProcessArgument(argument, contextFactory, validationContext);
			}
			return (StageChainResult.Continue, 0);
		}

		protected (StageChainResult Result, int SkipNext) GoToNextFromCallbackResult(ProcessStageResult result,
			ProgramArgument argument, Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory,
			ValidationContext validationContext)
		{
			switch (result.AfterProcessing)
			{
				case AfterProcessingStage.Continue:
					if (_next != null)
					{
						(StageChainResult downstreamResult, int downstreamSkip) = _next.TryProcessArgument(argument, contextFactory, validationContext);
						return (downstreamResult, result.SkipNextCount + downstreamSkip);
					}
					return (StageChainResult.Continue, result.SkipNextCount);
				case AfterProcessingStage.StopProcessingRemainingStages:
					return (StageChainResult.Continue, result.SkipNextCount);
				case AfterProcessingStage.KillBuild:
					return (StageChainResult.KillBuild, result.SkipNextCount);
				default:
					throw new ArgumentOutOfRangeException(nameof(result.AfterProcessing),
						$"'{result.AfterProcessing}' is nto a valid after processing type.");
			}
		}
	}
}
