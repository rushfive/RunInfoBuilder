using R5.RunInfoBuilder.Processor.Models;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Processor
{
	internal interface ICommandPipelineProcessor
	{

	}

	internal class CommandPipelineProcessor<TRunInfo> : ICommandPipelineProcessor
		   where TRunInfo : class
	{
		private ICommandPipelineFactory<TRunInfo> _pipelineFactory { get; }

		public CommandPipelineProcessor(
			ICommandPipelineFactory<TRunInfo> pipelineFactory)
		{
			_pipelineFactory = pipelineFactory;
		}

		public TRunInfo Process(string[] args, TRunInfo runInfo)
		{
			if (args == null)
			{
				throw new ArgumentNullException(nameof(args), "Program arguments must be provided.");
			}
			if (args.Length == 0)
			{
				throw new ArgumentException("At least one program argument must be provided.");
			}
			// TODO: abstract await the queue into a Pipeline object, helps
			// getting the cb context
			//Queue<Stage<TRunInfo>> pipeline = _pipelineFactory.Create(args);

			var pipeline = new Pipeline<TRunInfo>(new Queue<Stage<TRunInfo>>(), args); // TODO

			// common: 
			// - callbackcontext, 
			// - anything related to stage and program args queues,  ***
			// - runInfo

			// unique per command/sub-command level: 
			// - list of subcommands
			// - list of full/short options, and their setter funcs

			StageQueueCallbacks<TRunInfo> stageQueueCallbacks = pipeline.GetStageQueueCallbacks();
			ProgramArgumentQueueCallbacks<TRunInfo> programArgumentQueueCallbacks = pipeline.GetProgramArgumentQueueCallbacks();

			var processContext = new ProcessContext<TRunInfo>(
				runInfo,
				pipeline.GetCallbackContextFactory(args, runInfo),
				stageQueueCallbacks,
				programArgumentQueueCallbacks);


			return ProcessPipeline(pipeline);
		}

		private TRunInfo ProcessPipeline(Queue<Stage<TRunInfo>> pipeline)
		{
			int position = 0;

			while (pipeline.Any())
			{
				Stage<TRunInfo> next = pipeline.Dequeue();

				Func<CallbackContext<TRunInfo>> callbackContextFactory = () =>
				{

				};

				ProcessStageResult result = next.ProcessStage();

				position++;
			}


			throw new NotImplementedException();
		}
	}
}
