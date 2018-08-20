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

			Queue<Stage<TRunInfo>> pipeline = _pipelineFactory.Create(args);

			return ProcessPipeline(pipeline);
		}

		private TRunInfo ProcessPipeline(Queue<Stage<TRunInfo>> pipeline)
		{
			while (pipeline.Any())
			{
				Stage<TRunInfo> next = pipeline.Dequeue();

				ProcessStageResult result = next.ProcessStage();
			}


			throw new NotImplementedException();
		}
	}
}
