using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor;
using System;

namespace R5.RunInfoBuilder
{
	public class RunInfoBuilder
	{
		public ICommandStore Commands { get; }
		public IArgumentParser Parser { get; }

		private CommandStore _commandStore { get; }

		public RunInfoBuilder()
		{
			Parser = new ArgumentParser();

			IStagesFactory stagesQueueFactory = new StagesFactory();

			Commands = _commandStore = new CommandStore(stagesQueueFactory, Parser);
		}

		public object Build(string[] args)
		{
			try
			{
				dynamic pipeline = _commandStore.ResolvePipelineFromArgs(args);
				var runInfo = pipeline.Process();
				return runInfo;
			}
			catch (Exception ex)
			{
				if (ex is ProcessException processException)
				{
					throw;
				}

				throw new ProcessException("Failed to process args.", 
					ProcessError.GeneralFailure, -1, ex);
			}
		}
	}
}
