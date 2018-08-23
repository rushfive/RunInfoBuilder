//using R5.RunInfoBuilder.Processor.Models;
//using R5.RunInfoBuilder.Processor.Stages;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace R5.RunInfoBuilder.Processor
//{
//	// might just dump this and handle this directly from runinfobuilder
//	// possibly unnecessary layer of abstraction
//	internal interface IPipelineProcessor
//	{
//		TRunInfo Process<TRunInfo>(string[] args, TRunInfo runInfo)
//			where TRunInfo : class;
//	}

//	internal class PipelineProcessor : IPipelineProcessor
//	{
//		private IPipelineFactory _pipelineFactory { get; }

//		public PipelineProcessor(
//			IPipelineFactory pipelineFactory)
//		{
//			_pipelineFactory = pipelineFactory;
//		}

//		public TRunInfo Process<TRunInfo>(string[] args, TRunInfo runInfo)
//			where TRunInfo : class
//		{
//			if (args == null)
//			{
//				throw new ArgumentNullException(nameof(args), "Program arguments must be provided.");
//			}
//			if (args.Length == 0)
//			{
//				throw new ArgumentException("At least one program argument must be provided.");
//			}
//			// TODO: abstract await the queue into a Pipeline object, helps
//			// getting the cb context
//			//Queue<Stage<TRunInfo>> pipeline = _pipelineFactory.Create(args);

//			Pipeline<TRunInfo> pipeline = _pipelineFactory.Create<TRunInfo>(args);
//			// common: 
//			// - callbackcontext, 
//			// - anything related to stage and program args queues,  ***
//			// - runInfo

//			// unique per command/sub-command level: 
//			// - list of subcommands
//			// - list of full/short options, and their setter funcs
			
//			return pipeline.Process(runInfo);
//		}

//		//private TRunInfo ProcessPipeline(Queue<Stage<TRunInfo>> pipeline)
//		//{
//		//	int position = 0;

//		//	while (pipeline.Any())
//		//	{
//		//		Stage<TRunInfo> next = pipeline.Dequeue();

//		//		Func<CallbackContext<TRunInfo>> callbackContextFactory = () =>
//		//		{

//		//		};

//		//		ProcessStageResult result = next.ProcessStage();

//		//		position++;
//		//	}


//		//	throw new NotImplementedException();
//		//}
//	}
//}
