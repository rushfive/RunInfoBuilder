using System;
using System.Collections.Generic;
using System.Linq;

namespace R5.RunInfoBuilder.Process
{
	internal interface IProcessInvoker
	{
		void Start(List<ProgramArgument> programArguments);
	}

	internal class ProcessInvoker<TRunInfo> : IProcessInvoker
		where TRunInfo : class
	{
		private Queue<ProgramArgument> _argumentQueue { get; set; }
		private IStageChainFactory<TRunInfo> _chainFactory { get; }
		private RunInfo<TRunInfo> _runInfo { get; }

		public ProcessInvoker(
			IStageChainFactory<TRunInfo> chainFactory,
			RunInfo<TRunInfo> runInfo)
		{
			_chainFactory = chainFactory;
			_runInfo = runInfo;
		}

		public void Start(List<ProgramArgument> programArguments)
		{
			_argumentQueue = new Queue<ProgramArgument>(programArguments);
			
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory = CreateContextFactory(programArguments, _runInfo.Value);

			while (_argumentQueue.Any())
			{
				ProgramArgument argument = _argumentQueue.Dequeue();

				StageChain<TRunInfo> chain = _chainFactory.Create();

				(StageChainResult result, int skipNext) = chain.TryProcessArgument(argument, contextFactory);

				if (result == StageChainResult.KillBuild)
				{
					break;
				}

				while (_argumentQueue.Any() && skipNext-- > 0)
				{
					_argumentQueue.Dequeue();
				}
			}
		}

		private static Func<ProgramArgument, ProcessContext<TRunInfo>> CreateContextFactory
			(List<ProgramArgument> arguments, TRunInfo runInfo) =>
				(ProgramArgument argument) => new ProcessContext<TRunInfo>(argument, arguments, runInfo);
	}
}
