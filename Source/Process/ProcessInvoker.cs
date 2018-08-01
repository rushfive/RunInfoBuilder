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
		private IProcessContextsFactory<TRunInfo> _processContextsFactory { get; }

		public ProcessInvoker(
			IArgumentTypeResolver argumentTypeResolver,
			IStageChainFactory<TRunInfo> chainFactory,
			RunInfo<TRunInfo> runInfo,
			IProcessContextsFactory<TRunInfo> processContextsFactory)
		{
			_argumentTypeResolver = argumentTypeResolver;
			_chainFactory = chainFactory;
			_runInfo = runInfo;
			_processContextsFactory = processContextsFactory;
		}

		public ProcessResult Start(string[] args)
		{
			List<ProgramArgument> programArguments = ResolveProgramArguments(args, _argumentTypeResolver.GetArgumentType);
			var argumentQueue = new Queue<ProgramArgument>(programArguments);
			
			Func<ProgramArgument, ProcessContext<TRunInfo>> contextFactory = _processContextsFactory.CreateProcessContextFactory(programArguments, _runInfo.Value);

			ValidationContext validationContext = _processContextsFactory.CreateValidationContext();

			while (argumentQueue.Any())
			{
				ProgramArgument argument = argumentQueue.Dequeue();

				StageChain<TRunInfo> chain = _chainFactory.Create();

				(StageChainResult result, int skipNext) = chain.TryProcessArgument(argument, contextFactory, validationContext);

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
				result.Add(argument);
			}

			return result;
		}
	}
}
