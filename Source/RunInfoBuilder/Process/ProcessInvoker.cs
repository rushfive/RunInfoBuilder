using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Process
{
	// notes:
	// preProcess and postProcess should be moved into the builder

	internal interface IProcessInvoker
	{
		void Begin(List<ProgramArgument> programArguments);
	}
	internal class ProcessInvoker : IProcessInvoker
	{
		private Queue<ProgramArgument> _argumentQueue { get; set; }
		private IStageChainFactory _chainFactory { get; }

		public ProcessInvoker(IStageChainFactory chainFactory)
		{
			_chainFactory = chainFactory;
		}

		public void Begin(List<ProgramArgument> programArguments)
		{
			_argumentQueue = new Queue<ProgramArgument>(programArguments);

			while (_argumentQueue.Count > 0)
			{
				ProgramArgument argument = _argumentQueue.Dequeue();
				StageChainResult chainResult = _chainFactory.Create().ProcessStage(argument);
				// the ProcessStage above SHOULD return skip next info too,
				// resulting in queue dequeueing
			}
		}
	}

	// chain of responsibility pattern
	internal interface IStageChainFactory
	{
		StageChain Create();
	}
	internal class StageChainFactory : IStageChainFactory
	{
		public StageChain Create()
		{
			throw new NotImplementedException();
		}
	}

	internal enum StageChainResult
	{

	}
	internal abstract class StageChain
	{
		internal abstract StageChainResult ProcessStage(ProgramArgument argument);
	}
}
