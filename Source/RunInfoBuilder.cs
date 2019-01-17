using R5.RunInfoBuilder.Parser;
using System;
using System.Linq;

namespace R5.RunInfoBuilder
{
	/// <summary>
	/// Provides members for various configurations and a build method to start the parsing process.
	/// </summary>	
	public class RunInfoBuilder
	{
		/// <summary>
		/// Used to parse program arguments. Can be configured to handle additional types.
		/// </summary>
		public ArgumentParser Parser { get; }
		
		/// <summary>
		/// Configures how the program's help menu is displayed.
		/// </summary>
		public HelpManager Help { get; }

		/// <summary>
		/// Stores command configurations used for parsing program arguments into run info objects.
		/// </summary>
		public CommandStore Commands { get; }

		/// <summary>
		/// Configures how the program's version information is displayed.
		/// </summary>
		public VersionManager Version { get; }

		/// <summary>
		/// Configures custom callbacks to be hooked into the build process.
		/// </summary>
		public BuildHooks Hooks { get; }

		/// <summary>
		/// Initializes a new instance of the RunInfoBuilder class.
		/// </summary>
		public RunInfoBuilder()
		{
			Parser = new ArgumentParser();
			Help = new HelpManager();
			Commands = new CommandStore(Parser, Help);
			Version = new VersionManager();
			Hooks = new BuildHooks();
		}

		/// <summary>
		/// Call to build run info objects from program arguments.
		/// </summary>
		/// <param name="args">Program arguments to be parsed.</param>
		/// <returns>
		/// A run info object that's been configured in the Command Store
		/// and built by parsing the program arguments.
		/// </returns>
		public object Build(string[] args)
		{
			Hooks.InvokeOnStartIfSet(args);

			if (args == null)
			{
				throw new ArgumentNullException(nameof(args), "Program arguments must be provided.");
			}

			if (args.Any() && Help.IsTrigger(args.First()))
			{
				Help.Invoke();
				return null;
			}

			if (args.Any() && Version.IsTrigger(args.First()))
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

				// if not ProcessException, wrap and throw
				throw new ProcessException("Failed to process args.", 
					innerException: ex);
			}
		}
	}
}
