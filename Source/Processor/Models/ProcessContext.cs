using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace R5.RunInfoBuilder.Processor.Models
{

	// Each command that is processed gets its own shared ProcessContext
	// - processing is nested only by commands:
	//   - Always starts with a top-level command
	//   - all processing is "flat", except if it has subcommands, then it can
	//     recursively nest down n levels depending on how whether subcommands have subcommands themselves

    internal class ProcessContext<TRunInfo> where TRunInfo : class
    {
		internal TRunInfo RunInfo { get; }
		private Func<CallbackContext<TRunInfo>> _callbackContextFactory { get; }
		internal StageCallbacks<TRunInfo> Stages { get; }
		internal ProgramArgumentCallbacks<TRunInfo> ProgramArguments { get; }
		internal Action<Queue<Stage<TRunInfo>>> ExtendPipeline { get; }

		// "Refreshed" per command/subCommand
		private HashSet<string> _subCommands { get; set; }
		internal ProcessOptions<TRunInfo> Options { get; private set; }
		
		internal ProcessContext(
			TRunInfo runInfo,
			Func<CallbackContext<TRunInfo>> callbackContextFactory,
			StageCallbacks<TRunInfo> stageCallbacks,
			ProgramArgumentCallbacks<TRunInfo> programArgumentCallbacks,
			Action<Queue<Stage<TRunInfo>>> extendPipelineCallback)
		{
			RunInfo = runInfo;
			_callbackContextFactory = callbackContextFactory;
			Stages = stageCallbacks;
			ProgramArguments = programArgumentCallbacks;
			ExtendPipeline = extendPipelineCallback;
		}

		internal ProcessContext<TRunInfo> RefreshForCommand(CommandBase<TRunInfo> command)
		{
			_subCommands = new HashSet<string>();
			Options = new ProcessOptions<TRunInfo>(command.Options);

			if (command is Command<TRunInfo> cmd)
			{
				_subCommands = new HashSet<string>(cmd.SubCommands.Select(c => c.Key));
			}

			return this;
		}

		internal CallbackContext<TRunInfo> GetCallbackContext() => _callbackContextFactory();

		internal bool NextIsSubCommand() => _subCommands.Contains(ProgramArguments.Peek());

		internal bool NextIsOption() => Options.IsOption(ProgramArguments.Peek());
	}
}
