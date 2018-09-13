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
		public IArgumentParser Parser { get; }

		private CommandStore _commandStore { get; }

		public RunInfoBuilder()
		{
			// temp
			var keyValidator = new RestrictedKeyValidator();

			Parser = new ArgumentParser();

			IStagesFactory stagesQueueFactory = new StagesFactory(Parser);

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
