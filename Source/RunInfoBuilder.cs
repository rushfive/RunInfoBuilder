using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor;
using System;

namespace R5.RunInfoBuilder
{
	public class RunInfoBuilder
	{
		public ArgumentParser Parser { get; }
		public HelpManager Help { get; }
		public CommandStore Commands { get; }
		public VersionManager Version { get; }
		
		public RunInfoBuilder()
		{
			Parser = new ArgumentParser();
			Help = new HelpManager();
			Commands = new CommandStore(Parser, Help);
			Version = new VersionManager();
		}

		public object Build(string[] args)
		{
			Help.Invoke();



			return null;// TEMP SHORT CIRCUIT
			try
			{
				dynamic pipeline = Commands.ResolvePipelineFromArgs(args);
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
