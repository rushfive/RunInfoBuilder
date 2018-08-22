//using R5.RunInfoBuilder.Commands;
//using R5.RunInfoBuilder.Processor.Stages;
//using System;
//using System.Collections.Generic;

//namespace R5.RunInfoBuilder.Processor
//{
//	internal interface IPipelineFactory
//	{
//		Pipeline<TRunInfo> Create<TRunInfo>(string[] args)
//			where TRunInfo : class;
//	}

//	internal class PipelineFactory : IPipelineFactory
//	{
//		private ICommandStoreInternal _commandStore { get; }
//		private IStagesFactory _stagesQueueFactory { get; }

//		public PipelineFactory(
//			ICommandStoreInternal commandStore,
//			IStagesFactory stagesQueueFactory)
//		{
//			_commandStore = commandStore;
//			_stagesQueueFactory = stagesQueueFactory;
//		}

//		public Pipeline<TRunInfo> Create<TRunInfo>(string[] args)
//			where TRunInfo : class
//		{
//			if (args.Length == 0 || !_commandStore.IsCommand(args[0]))
//			{
//				return CreateForDefaultCommand<TRunInfo>(args);
//			}

//			return CreateForCommand<TRunInfo>(args);
//		}

//		private Pipeline<TRunInfo> CreateForCommand<TRunInfo>(string[] args)
//			where TRunInfo : class
//		{
//			if (!_commandStore.TryGetCommand(args[0], out Command<TRunInfo> command))
//			{
//				throw new InvalidOperationException($"'{args[0]}' is not a valid configured command.");
//			}

//			Queue<Stage<TRunInfo>> stages = _stagesQueueFactory.Create(command);
//			return new Pipeline<TRunInfo>(stages, args);
//		}

//		private Pipeline<TRunInfo> CreateForDefaultCommand<TRunInfo>(string[] args)
//			where TRunInfo : class
//		{
//			if (!_commandStore.TryGetDefaultCommand(out DefaultCommand<TRunInfo> defaultCommand))
//			{
//				throw new InvalidOperationException("Default command is not configured.");
//			}

//			Queue<Stage<TRunInfo>> stages = _stagesQueueFactory.Create(defaultCommand);
//			return new Pipeline<TRunInfo>(stages, args);
//		}
//	}
//}
