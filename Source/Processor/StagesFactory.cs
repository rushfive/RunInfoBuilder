using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Processor
{
	internal class StagesFactory
	{
		internal Queue<Stage<TRunInfo>> Create<TRunInfo>(
			StackableCommand<TRunInfo> command, bool hasGlobalOptions, Action<TRunInfo> postBuildCallback)
			 where TRunInfo : class
		{
			var pipeline = new Queue<Stage<TRunInfo>>();

			if (command is Command<TRunInfo> rootCommand)
			{
				pipeline.Enqueue(rootCommand.ToStage());
			}
			
			Queue<Stage<TRunInfo>> commonStages = BuildCommonPipelineStages(command, hasGlobalOptions);
			while (commonStages.Any())
			{
				pipeline.Enqueue(commonStages.Dequeue());
			}

			// recursively add subcommand pipelines
			if (command.SubCommands.Any())
			{
				var subCommandInfoMap = new Dictionary<string, (Queue<Stage<TRunInfo>>, SubCommand<TRunInfo>)>();

				foreach (SubCommand<TRunInfo> subCommand in command.SubCommands)
				{
					Queue<Stage<TRunInfo>> subCommandPipeline = Create(subCommand, hasGlobalOptions, postBuildCallback);
					
					subCommandInfoMap.Add(subCommand.Key, (subCommandPipeline, subCommand));
				}
				
				pipeline.Enqueue(new SubCommandStage<TRunInfo>(subCommandInfoMap));
			}

			// EndProcessStage should ONLY be on a leaf stage node
			if (!command.SubCommands.Any())
			{
				pipeline.Enqueue(new EndProcessStage<TRunInfo>(postBuildCallback));
			}
			
			return pipeline;
		}

		internal Queue<Stage<TRunInfo>> Create<TRunInfo>(
			DefaultCommand<TRunInfo> defaultCommand, Action<TRunInfo> postBuildCallback)
			 where TRunInfo : class
		{
			var pipeline = new Queue<Stage<TRunInfo>>();

			pipeline.Enqueue(defaultCommand.ToStage());
			
			Queue<Stage<TRunInfo>> commonStages = BuildCommonPipelineStages(defaultCommand, hasGlobalOptions: false);
			while (commonStages.Any())
			{
				pipeline.Enqueue(commonStages.Dequeue());
			}

			pipeline.Enqueue(new EndProcessStage<TRunInfo>(postBuildCallback));

			return pipeline;
		}

		private Queue<Stage<TRunInfo>> BuildCommonPipelineStages<TRunInfo>(
			CommandBase<TRunInfo> command, bool hasGlobalOptions)
			 where TRunInfo : class
		{
			var pipeline = new Queue<Stage<TRunInfo>>();

			foreach (ArgumentBase<TRunInfo> argument in command.Arguments)
			{
				pipeline.Enqueue(argument.ToStage());
			}

			if (hasGlobalOptions || command.Options.Any())
			{
				pipeline.Enqueue(new OptionStage<TRunInfo>());
			}

			return pipeline;
		}
	}
}
