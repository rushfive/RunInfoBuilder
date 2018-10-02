using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor;
using R5.RunInfoBuilder.Validators;
using System;

namespace R5.RunInfoBuilder
{
	// todo: allow advanced config AND hook into DI
	public class RunInfoBuilder
	{
		public ICommandStore Commands { get; }
		public IArgumentParser Parser { get; }

		private CommandStore _commandStore { get; }

		public RunInfoBuilder()
		{
			// temp
			var keyValidator = new RestrictedKeyValidator();

			Parser = new ArgumentParser();

			IStagesFactory stagesQueueFactory = new StagesFactory();

			Commands = _commandStore = new CommandStore(
				new CommandValidator(keyValidator),
				keyValidator,
				stagesQueueFactory,
				Parser);
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
