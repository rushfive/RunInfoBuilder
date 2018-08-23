using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor.Models;
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
		private IArgumentParser _parser { get; }

		public StagesFactory(IArgumentParser parser)
		{
			_parser = parser;
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

				Action<Queue<Stage<TRunInfo>>> extendPipelineCallback = subCommandPipeline =>
				{
					while (subCommandPipeline.Any())
					{
						pipeline.Enqueue(subCommandPipeline.Dequeue());
					}
				};

				pipeline.Enqueue(new SubCommandStage<TRunInfo>(subCommandInfoMap));
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

			AddCallbackStageIfExistsFor(command);

			foreach (ArgumentBase<TRunInfo> argument in command.Arguments)
			{
				AddCallbackStageIfExistsFor(argument);
				pipeline.Enqueue(argument.ToStage(_parser));
			}

			if (command.Options.Any())
			{
				pipeline.Enqueue(new OptionStage<TRunInfo>(_parser));
			}

			return pipeline;

			// local functions
			void AddCallbackStageIfExistsFor(ICallbackElement<TRunInfo> element)
			{
				if (element.Callback != null)
				{
					pipeline.Enqueue(new CallbackStage<TRunInfo>(element.Callback));
				}
			}
		}
	}
}
