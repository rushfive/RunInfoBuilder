using R5.RunInfoBuilder.Store;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R5.RunInfoBuilder.Process
{
	internal interface IProcessInvoker
	{
		ProcessResult Start(string[] args);
	}

	internal class ProcessInvoker<TRunInfo> : IProcessInvoker
		where TRunInfo : class
	{
		private IArgumentTypeResolver _argumentTypeResolver { get; set; }
		private IStageChainFactory<TRunInfo> _chainFactory { get; }
		private RunInfo<TRunInfo> _runInfo { get; }

		public ProcessInvoker(
			IArgumentTypeResolver argumentTypeResolver,
			IStageChainFactory<TRunInfo> chainFactory,
			RunInfo<TRunInfo> runInfo)
		{
			_argumentTypeResolver = argumentTypeResolver;
			_chainFactory = chainFactory;
			_runInfo = runInfo;
		}

		public ProcessResult Start(string[] args)
		{
			List<ProgramArgument> programArguments = ResolveProgramArguments(args, _argumentTypeResolver.GetArgumentType);

			var argumentQueue = new Queue<ProgramArgument>(programArguments);
			
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory = CreateContextFactory(programArguments, _runInfo.Value);

			while (argumentQueue.Any())
			{
				ProgramArgument argument = argumentQueue.Dequeue();

				StageChain<TRunInfo> chain = _chainFactory.Create();

				(StageChainResult result, int skipNext) = chain.TryProcessArgument(argument, contextFactory);

				if (result == StageChainResult.KillBuild)
				{
					return ProcessResult.Success;
				}
				if (result == StageChainResult.Help)
				{
					return ProcessResult.Help;
				}
				if (result == StageChainResult.Version)
				{
					return ProcessResult.Version;
				}

				while (argumentQueue.Any() && skipNext-- > 0)
				{
					argumentQueue.Dequeue();
				}
			}

			return ProcessResult.Success;
		}

		private static List<ProgramArgument> ResolveProgramArguments(string[] args, 
			Func<string, ProgramArgumentType> typeResolveFunc)
		{
			var result = new List<ProgramArgument>();

			for (int i = 0; i < args.Length; i++)
			{
				var argument = new ProgramArgument(i, args[i], typeResolveFunc(args[i]));
			}

			return result;
		}

		private static Func<ProgramArgument, ProcessContext<TRunInfo>> CreateContextFactory
			(List<ProgramArgument> arguments, TRunInfo runInfo) =>
				(ProgramArgument argument) => new ProcessContext<TRunInfo>(argument, arguments, runInfo);
	}
}
