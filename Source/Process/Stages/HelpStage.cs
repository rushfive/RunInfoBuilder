using OLD.Help;
using System;

namespace OLD.Process
{
	internal class HelpStage<TRunInfo> : StageChain<TRunInfo>
				where TRunInfo : class
	{
		private IHelpManager<TRunInfo> _helpManager { get; }

		internal HelpStage(IHelpManager<TRunInfo> helpManager)
			: base(handlesType: ProgramArgumentType.Help)
		{
			_helpManager = helpManager;
		}

		protected override (StageChainResult Result, int SkipNext) Process(
			ProgramArgument argument,
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory,
			ValidationContext validationContext)
		{
			ProcessContext<TRunInfo> context = contextFactory(argument);

			if (argument.Position > 0 || context.ProgramArguments.Count > 1)
			{
				throw new InvalidOperationException("Help command must be the only program argument.");
			}

			_helpManager.InvokeCallback();

			return (StageChainResult.Help, 0);
		}
	}
}
