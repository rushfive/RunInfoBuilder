using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder.Processor
{
	internal interface ICommandPipelineFactory<TRunInfo>
		where TRunInfo : class
	{
		Queue<Stage<TRunInfo>> Create(string[] args);
	}

	internal class CommandPipelineFactory<TRunInfo> : ICommandPipelineFactory<TRunInfo>
		where TRunInfo : class
	{
		private ICommandStore<TRunInfo> _commandStore { get; }
		private IPipelineBuilder<TRunInfo> _pipelineBuilder { get; }

		public CommandPipelineFactory(
			ICommandStore<TRunInfo> commandStore,
			IPipelineBuilder<TRunInfo> pipelineBuilder)
		{
			_commandStore = commandStore;
			_pipelineBuilder = pipelineBuilder;
		}

		public Queue<Stage<TRunInfo>> Create(string[] args)
		{
			if (args.Length == 0 || !_commandStore.IsCommand(args[0]))
			{
				return CreateForDefaultCommand();
			}

			return CreateForCommand(args[0]);
		}

		private Queue<Stage<TRunInfo>> CreateForCommand(string firstProgramArgument)
		{
			if (!_commandStore.TryGetCommand(firstProgramArgument, out Command<TRunInfo> command))
			{
				throw new InvalidOperationException($"'{firstProgramArgument}' is not a valid configured command.");
			}
			
			return _pipelineBuilder.Build(command);
		}

		private Queue<Stage<TRunInfo>> CreateForDefaultCommand()
		{
			if (!_commandStore.TryGetDefaultCommand(out DefaultCommand<TRunInfo> defaultCommand))
			{
				throw new InvalidOperationException("Default command is not configured.");
			}

			return _pipelineBuilder.Build(defaultCommand);
		}
	}
}
