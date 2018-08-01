using R5.RunInfoBuilder.Version;
using System;

namespace R5.RunInfoBuilder.Process
{
	internal class VersionStage<TRunInfo> : StageChain<TRunInfo>
			where TRunInfo : class
	{
		private IVersionManager _versionManager { get; }

		internal VersionStage(IVersionManager versionManager)
			: base(handlesType: ProgramArgumentType.Version)
		{
			_versionManager = versionManager;
		}

		protected override (StageChainResult Result, int SkipNext) Process(
			ProgramArgument argument,
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory,
			ValidationContext validationContext)
		{
			ProcessContext<TRunInfo> context = contextFactory(argument);

			if (argument.Position > 0 || context.ProgramArguments.Count > 1)
			{
				throw new InvalidOperationException("Version command must be the only program argument.");
			}

			_versionManager.InvokeCallback();

			return (StageChainResult.Version, 0);
		}
	}
}
