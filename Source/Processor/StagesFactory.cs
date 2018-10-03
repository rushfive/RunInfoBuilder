using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Processor
{
	internal interface IStagesFactory
	{
		Queue<Stage<TRunInfo>> Create<TRunInfo>(Command<TRunInfo> command)
			 where TRunInfo : class;

		Queue<Stage<TRunInfo>> Create<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand)
			 where TRunInfo : class;
	}

	internal class StagesFactory : IStagesFactory
	{
		public StagesFactory()
		{
			
		}
		
		public Queue<Stage<TRunInfo>> Create<TRunInfo>(Command<TRunInfo> command)
			 where TRunInfo : class
		{
			Queue<Stage<TRunInfo>> pipeline = BuildCommonPipelineStages(command);

			// recursively add subcommand pipelines
			if (command.SubCommands.Any())
			{
				var subCommandInfoMap = new Dictionary<string, (Queue<Stage<TRunInfo>>, Command<TRunInfo>)>();

				foreach (Command<TRunInfo> subCommand in command.SubCommands)
				{
					Queue<Stage<TRunInfo>> subCommandPipeline = Create(subCommand);
					
					subCommandInfoMap.Add(subCommand.Key, (subCommandPipeline, subCommand));
				}
				
				pipeline.Enqueue(new SubCommandStage<TRunInfo>(subCommandInfoMap));
			}

			// EndProcessStage should ONLY be on a leaf stage node
			if (!command.SubCommands.Any())
			{
				pipeline.Enqueue(new EndProcessStage<TRunInfo>());
			}
			
			return pipeline;
		}

		public Queue<Stage<TRunInfo>> Create<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand)
			 where TRunInfo : class
		{
			return BuildCommonPipelineStages(defaultCommand);
		}

		private Queue<Stage<TRunInfo>> BuildCommonPipelineStages<TRunInfo>(CommandBase<TRunInfo> command)
			 where TRunInfo : class
		{
			var pipeline = new Queue<Stage<TRunInfo>>();

			foreach (ArgumentBase<TRunInfo> argument in command.Arguments)
			{
				pipeline.Enqueue(argument.ToStage());
			}

			if (command.Options.Any())
			{
				pipeline.Enqueue(new OptionStage<TRunInfo>());
			}

			return pipeline;
		}
	}
}
