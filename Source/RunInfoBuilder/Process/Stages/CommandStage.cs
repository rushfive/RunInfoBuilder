using R5.RunInfoBuilder.Store;
using System;

namespace R5.RunInfoBuilder.Process
{
	internal class CommandStage<TRunInfo> : StageChain<TRunInfo>
		where TRunInfo : class
	{
		private IArgumentMetadata<TRunInfo> _argumentMetadata { get; }
		private RunInfo<TRunInfo> _runInfo { get; }

		internal CommandStage(
			IArgumentMetadata<TRunInfo> argumentMetadata,
			RunInfo<TRunInfo> runInfo)
			: base(handlesType: ProgramArgumentType.Command)
		{
			_argumentMetadata = argumentMetadata;
			_runInfo = runInfo;
		}

		protected override (StageChainResult Result, int SkipNext) Process(
			ProgramArgument argument,
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory,
			ValidationContext validationContext)
		{
			CommandMetadata<TRunInfo> metadata = _argumentMetadata.GetCommand(argument.ArgumentToken);

			switch (metadata.Type)
			{
				case CommandType.PropertyMapped:
					{
						metadata.PropertyInfo.SetValue(_runInfo.Value, metadata.MappedValue);
						return GoToNext(argument, contextFactory, validationContext);
					}
				case CommandType.CustomCallback:
					{
						ProcessContext<TRunInfo> context = contextFactory(argument);

						ProcessStageResult result = metadata.Callback(context);

						return GoToNextFromCallbackResult(result, argument, contextFactory, validationContext);
					}
				case CommandType.MappedAndCallback:
					{
						metadata.PropertyInfo.SetValue(_runInfo.Value, metadata.MappedValue);

						ProcessContext<TRunInfo> context = contextFactory(argument);

						ProcessStageResult result = metadata.Callback(context);

						return GoToNextFromCallbackResult(result, argument, contextFactory, validationContext);
					}
				default:
					throw new ArgumentOutOfRangeException($"'{metadata.Type}' is not a valid command type.");
			}
		}
	}
}
