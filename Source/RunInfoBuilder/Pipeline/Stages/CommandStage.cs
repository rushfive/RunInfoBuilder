//using R5.RunInfoBuilder.Store;
//using System;

//namespace R5.RunInfoBuilder.Pipeline
//{
//	internal class CommandStage<TRunInfo> : ProcessPipelineStageBase<TRunInfo>
//		where TRunInfo : class
//	{
//		private IArgumentMetadata<TRunInfo> _argumentMaps { get; }
//		private RunInfo<TRunInfo> _runInfo { get; }

//		internal CommandStage(
//			IArgumentMetadata<TRunInfo> argumentMaps,
//			RunInfo<TRunInfo> runInfo)
//			: base(ProgramArgumentType.Command)
//		{
//			_argumentMaps = argumentMaps;
//			_runInfo = runInfo;
//		}

//		internal override (int SkipNext, AfterProcessingStage AfterStage) Process(ProcessArgumentContext<TRunInfo> context)
//		{
//			string commandToken = context.Token;

//			CommandMetadata<TRunInfo> metadata = _argumentMaps.GetCommand(commandToken);

//			switch (metadata.Type)
//			{
//				case CommandType.PropertyMapped:
//					return HandleForPropertyMapped(metadata);
//				case CommandType.CustomCallback:
//					return HandleForCustomCallback(metadata, context);
//				case CommandType.MappedAndCallback:
//					return HandleForMappedAndCallback(metadata, context);
//				default:
//					throw new ArgumentOutOfRangeException($"'{metadata.Type}' is not a valid command type.");
//			}
//		}

//		private (int SkipNext, AfterProcessingStage AfterStage) HandleForPropertyMapped(CommandMetadata<TRunInfo> metadata)
//		{
//			metadata.PropertyInfo.SetValue(_runInfo.Value, metadata.MappedValue);
//			return (0, AfterProcessingStage.Continue);
//		}

//		private (int SkipNext, AfterProcessingStage AfterStage) HandleForCustomCallback(CommandMetadata<TRunInfo> metadata,
//			ProcessArgumentContext<TRunInfo> context)
//		{
//			ProcessStageResult result = metadata.Callback(context);
//			return (result.SkipNext, result.AfterProcessing);
//		}

//		private (int SkipNext, AfterProcessingStage AfterStage) HandleForMappedAndCallback(CommandMetadata<TRunInfo> metadata,
//			ProcessArgumentContext<TRunInfo> context)
//		{
//			metadata.PropertyInfo.SetValue(_runInfo.Value, metadata.MappedValue);

//			ProcessStageResult result = metadata.Callback(context);
//			return (result.SkipNext, result.AfterProcessing);
//		}
//	}
//}
