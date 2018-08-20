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
	internal interface IPipelineBuilder<TRunInfo> where TRunInfo : class
	{
		Queue<Stage<TRunInfo>> Build(Command<TRunInfo> command);

		Queue<Stage<TRunInfo>> Build(DefaultCommand<TRunInfo> defaultCommand);
	}

	internal class PipelineBuilder<TRunInfo> : IPipelineBuilder<TRunInfo>
		where TRunInfo : class
	{
		private IArgumentParser _parser { get; }

		public PipelineBuilder(IArgumentParser parser)
		{
			_parser = parser;
		}

		private ProcessContext<TRunInfo> GetProcessContext()
			=> throw new NotImplementedException("TODO!");

		public Queue<Stage<TRunInfo>> Build(Command<TRunInfo> command)
		{
			ProcessContext<TRunInfo> processContext = GetProcessContext();

			Queue<Stage<TRunInfo>> pipeline = BuildCommonPipelineStages(command, processContext);

			// recursively add subcommand pipelines
			if (command.SubCommands.Any())
			{
				var subCommandPipelineMap = new Dictionary<string, Queue<Stage<TRunInfo>>>();

				foreach (Command<TRunInfo> subCommand in command.SubCommands)
				{
					Queue<Stage<TRunInfo>> subCommandPipeline = Build(subCommand);
					subCommandPipelineMap.Add(subCommand.Key, subCommandPipeline);
				}

				Action<Queue<Stage<TRunInfo>>> extendPipelineCallback = subCommandPipeline =>
				{
					while (subCommandPipeline.Any())
					{
						pipeline.Enqueue(subCommandPipeline.Dequeue());
					}
				};

				pipeline.Enqueue(new SubCommandStage<TRunInfo>(subCommandPipelineMap, 
					extendPipelineCallback, processContext));
			}

			return pipeline;
		}

		public Queue<Stage<TRunInfo>> Build(DefaultCommand<TRunInfo> defaultCommand)
		{
			ProcessContext<TRunInfo> processContext = GetProcessContext();

			return BuildCommonPipelineStages(defaultCommand, processContext);
		}

		private Queue<Stage<TRunInfo>> BuildCommonPipelineStages(CommandBase<TRunInfo> command,
			ProcessContext<TRunInfo> processContext)
		{
			var pipeline = new Queue<Stage<TRunInfo>>();

			AddCallbackStageIfExistsFor(command);

			foreach (ArgumentBase<TRunInfo> argument in command.Arguments)
			{
				AddCallbackStageIfExistsFor(argument);
				pipeline.Enqueue(argument.ToStage(processContext, _parser));
			}

			if (command.Options.Any())
			{
				pipeline.Enqueue(new OptionStage<TRunInfo>(_parser, processContext));
			}

			return pipeline;

			// local functions
			void AddCallbackStageIfExistsFor(ICallbackElement<TRunInfo> element)
			{
				if (element.Callback != null)
				{
					pipeline.Enqueue(new CallbackStage<TRunInfo>(element.Callback, processContext));
				}
			}
		}
	}
}
