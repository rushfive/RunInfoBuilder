using System;

namespace R5.RunInfoBuilder.Process
{
    internal abstract class ValidationStage<TRunInfo> : StageChain<TRunInfo>
			where TRunInfo : class
	{
		internal ValidationStage(ProgramArgumentType handlesType)
			: base(handlesType)
		{
		}

		protected abstract void Validate(ProgramArgument argument, ValidationContext validationContext);

		protected abstract void MarkSeen(string token, ValidationContext validationContext);

		protected override (StageChainResult Result, int SkipNext) Process(
			ProgramArgument argument,
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory,
			ValidationContext validationContext)
		{
			Validate(argument, validationContext);

			ThrowIfDuplicateNotAllowed(argument, validationContext);

			MarkSeen(argument.ArgumentToken, validationContext);

			return GoToNext(argument, contextFactory, validationContext);
		}

		private void ThrowIfDuplicateNotAllowed(ProgramArgument argument, ValidationContext validationContext)
		{
			if (validationContext.ProcessConfig.DuplicateArgumentsAllowed)
			{
				return;
			}

			if (validationContext.HasSeen(argument.ArgumentToken))
			{
				throw new InvalidOperationException("Duplicate arguments aren't allowed: "
				+ $"{argument.Type} '{argument.ArgumentToken}' has already been seen.");
			}
		}
	}
}
