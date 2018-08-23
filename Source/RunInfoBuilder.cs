using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor;
using R5.RunInfoBuilder.Validators;

namespace R5.RunInfoBuilder
{
	// todo: allow advanced config AND hook into DI
	public class RunInfoBuilder
	{
		public ICommandStore Commands { get; }
		//private IPipelineFactory _pipelineFactory { get; }

		private CommandStore _commandStore { get; }

		public RunInfoBuilder()
		{
			// temp
			var keyValidator = new RestrictedKeyValidator();

			var argPArser = new ArgumentParser();

			IStagesFactory stagesQueueFactory = new StagesFactory(argPArser);

			Commands = _commandStore = new CommandStore(
				new CommandValidator(keyValidator),
				keyValidator,
				stagesQueueFactory);

			

			//_pipelineFactory = new PipelineFactory((ICommandStoreInternal)Commands, stagesQueueFactory);
		}






		public object Build(string[] args)
		{
			dynamic pipeline = _commandStore.ResolvePipelineFromArgs(args);
			var runInfo = pipeline.Process();
			return runInfo;
			

		}
	}
	
	
}
