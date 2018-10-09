using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor;
using System;
using System.Linq;

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
			// potential future hook
			if (args == null || !args.Any())
			{
				throw new ArgumentNullException(nameof(args), "Program arguments must be provided.");
			}

			if (Help.IsTrigger(args.First()))
			{
				Help.Invoke();
				return null;
			}

			if (Version.IsTrigger(args.First()))
			{
				Version.Invoke();
				return null;
			}

			try
			{
				dynamic pipeline = Commands.ResolvePipelineFromArgs(args);
				var runInfo = pipeline.Process();
				return runInfo;
			}
			catch (Exception ex)
			{
				// this also suppresses exception bubbling
				if (Help.InvokeOnFail)
				{
					Help.Invoke();
					return null;
				}

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
