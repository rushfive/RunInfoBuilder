using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Processor.Models;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Processor
{
	internal class PipelineBuilder<TRunInfo> where TRunInfo : class
	{
		private ProcessContext<TRunInfo> GetProcessContext()
			=> throw new NotImplementedException("TODO!");



		internal Queue<Stage<TRunInfo>> Build(Command<TRunInfo> command)
		{
			var pipeline = new Queue<Stage<TRunInfo>>();

			ProcessContext<TRunInfo> processContext = GetProcessContext();
			
			AddCallbackStageIfExistsFor(command);

			foreach(ArgumentBase<TRunInfo> argument in command.Arguments)
			{
				AddCallbackStageIfExistsFor(argument);
				pipeline.Enqueue(argument.ToStage(processContext));
			}

			if (command.Options.Any())
			{
				pipeline.Enqueue(new OptionStage<TRunInfo>(processContext));
			}

			// recursively add subcommand pipelines
			if (command.SubCommands.Any())
			{
				var subCommandPipelineMap = new Dictionary<string, Queue<Stage<TRunInfo>>>();

				foreach(Command<TRunInfo> subCommand in command.SubCommands)
				{
					Queue<Stage<TRunInfo>> subCommandPipeline = Build(subCommand);
					subCommandPipelineMap.Add(subCommand.Key, subCommandPipeline);
				}

				// create and add sub command stage tthat uses this map...
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
