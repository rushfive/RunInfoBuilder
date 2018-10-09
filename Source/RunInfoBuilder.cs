using R5.RunInfoBuilder.Parser;
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
		public BuildHooks Hooks { get; }
		
		public RunInfoBuilder()
		{
			Parser = new ArgumentParser();
			Help = new HelpManager();
			Commands = new CommandStore(Parser, Help);
			Version = new VersionManager();
			Hooks = new BuildHooks();
		}

		public object Build(string[] args)
		{
			if (Hooks.OnStartIsSet)
			{
				Hooks.InvokeOnStart(args);
			}

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
				// supress exceptions from bubbling to client
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
