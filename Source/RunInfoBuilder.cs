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
		private IPipelineFactory _pipelineFactory { get; }


		public RunInfoBuilder()
		{
			// temp
			var keyValidator = new RestrictedKeyValidator();

			Commands = new CommandStore(
				new CommandValidator(keyValidator),
				keyValidator);

			var argPArser = new ArgumentParser();

			IStagesQueueFactory stagesQueueFactory = new StagesQueueFactory(argPArser);

			_pipelineFactory = new PipelineFactory((ICommandStoreInternal)Commands, stagesQueueFactory);
		}






		public object Build(string[] args)
		{
			var pipeline = _pipelineFactory.Create(args);


		}
	}
	
	
}
