using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder.Processor
{
	internal interface IPipelineFactory<TRunInfo>
		where TRunInfo : class
	{
		Pipeline<TRunInfo> Create(string[] args);
	}

	internal class PipelineFactory<TRunInfo> : IPipelineFactory<TRunInfo>
		where TRunInfo : class
	{
		private ICommandStore<TRunInfo> _commandStore { get; }
		private IStagesQueueFactory<TRunInfo> _stagesQueueFactory { get; }

		public PipelineFactory(
			ICommandStore<TRunInfo> commandStore,
			IStagesQueueFactory<TRunInfo> stagesQueueFactory)
		{
			_commandStore = commandStore;
			_stagesQueueFactory = stagesQueueFactory;
		}

		public Pipeline<TRunInfo> Create(string[] args)
		{
			if (args.Length == 0 || !_commandStore.IsCommand(args[0]))
			{
				return CreateForDefaultCommand(args);
			}

			return CreateForCommand(args);
		}

		private Pipeline<TRunInfo> CreateForCommand(string[] args)
		{
			if (!_commandStore.TryGetCommand(args[0], out Command<TRunInfo> command))
			{
				throw new InvalidOperationException($"'{args[0]}' is not a valid configured command.");
			}

			Queue<Stage<TRunInfo>> stages = _stagesQueueFactory.Create(command);
			return new Pipeline<TRunInfo>(stages, args);
		}

		private Pipeline<TRunInfo> CreateForDefaultCommand(string[] args)
		{
			if (!_commandStore.TryGetDefaultCommand(out DefaultCommand<TRunInfo> defaultCommand))
			{
				throw new InvalidOperationException("Default command is not configured.");
			}

			Queue<Stage<TRunInfo>> stages = _stagesQueueFactory.Create(defaultCommand);
			return new Pipeline<TRunInfo>(stages, args);
		}
	}
}
