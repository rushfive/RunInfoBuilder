using R5.RunInfoBuilder.Store;
using System;

namespace R5.RunInfoBuilder.Pipeline
{
	internal class CommandStage<TRunInfo> : ProcessPipelineStageBase<TRunInfo>
		where TRunInfo : class
	{
		private IArgumentMetadataMaps<TRunInfo> _argumentMaps { get; }
		private RunInfo<TRunInfo> _runInfo { get; }

		internal CommandStage(
			IArgumentMetadataMaps<TRunInfo> argumentMaps,
			RunInfo<TRunInfo> runInfo)
			: base(ProgramArgumentType.Command)
		{
			_argumentMaps = argumentMaps;
			_runInfo = runInfo;
		}

		internal override ProcessStageResult Process(ProcessArgumentContext<TRunInfo> context)
		{
			string commandToken = context.Token;

			CommandMetadata<TRunInfo> metadata = _argumentMaps.GetCommand(commandToken);

			switch (metadata.Type)
			{
				case CommandType.PropertyMapped:
					return HandleForPropertyMapped(metadata);
				case CommandType.CustomCallback:
					return HandleForCustomCallback(metadata, context);
				case CommandType.MappedAndCallback:
					return HandleForMappedAndCallback(metadata, context);
				default:
					throw new ArgumentOutOfRangeException($"'{metadata.Type}' is not a valid command type.");
			}
		}

		private ProcessStageResult HandleForPropertyMapped(CommandMetadata<TRunInfo> metadata)
		{
			metadata.PropertyInfo.SetValue(_runInfo.Value, metadata.MappedValue);
			return new ProcessStageResult();
		}

		private ProcessStageResult HandleForCustomCallback(CommandMetadata<TRunInfo> metadata,
			ProcessArgumentContext<TRunInfo> context)
		{
			return metadata.Callback(context);
		}

		private ProcessStageResult HandleForMappedAndCallback(CommandMetadata<TRunInfo> metadata,
			ProcessArgumentContext<TRunInfo> context)
		{
			metadata.PropertyInfo.SetValue(_runInfo.Value, metadata.MappedValue);
			return metadata.Callback(context);
		}
	}
}
