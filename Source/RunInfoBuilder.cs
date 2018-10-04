using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor;
using System;

namespace R5.RunInfoBuilder
{
	public class RunInfoBuilder
	{
		public ArgumentParser Parser { get; }
		public CommandStore Commands { get; }
		
		public RunInfoBuilder()
		{
			Parser = new ArgumentParser();
			Commands = new CommandStore(Parser);
		}

		public object Build(string[] args)
		{
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
