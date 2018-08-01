using R5.RunInfoBuilder.Configuration;
using System;

namespace R5.RunInfoBuilder.Process
{
	internal class HandleUnresolvedStage<TRunInfo> : StageChain<TRunInfo>
		where TRunInfo : class
	{
		private ProcessConfig _config { get; }

		internal HandleUnresolvedStage(ProcessConfig config)
			: base(handlesType: ProgramArgumentType.Unresolved)
		{
			_config = config;
		}

		protected override (StageChainResult Result, int SkipNext) Process(
			ProgramArgument argument,
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory,
			ValidationContext validationContext)
		{
			if (argument.Type != ProgramArgumentType.Unresolved)
			{
				return GoToNext(argument, contextFactory, validationContext);
			}

			switch (_config.HandleUnresolved)
			{
				case HandleUnresolvedArgument.NotAllowed:
					throw new InvalidOperationException("Unresolved program arguments are invalid for this configuration.");
				case HandleUnresolvedArgument.AllowedButThrowOnProcess:
					throw new InvalidOperationException($"Failed to process program argument '{argument.ArgumentToken}' because it's an unknown type.");
				case HandleUnresolvedArgument.AllowedButSkipOnProcess:
					return (StageChainResult.Continue, 0);
				default:
					throw new ArgumentOutOfRangeException(nameof(_config.HandleUnresolved),
						$"'{_config.HandleUnresolved}' is not a valid handle unresolved argument type.");
			}
		}
	}
}
