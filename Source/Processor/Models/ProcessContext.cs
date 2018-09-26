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
		//private Func<CallbackContext<TRunInfo>> _callbackContextFactory { get; }
		internal StageFunctions<TRunInfo> Stages { get; private set; }
		internal ProgramArgumentCallbacks<TRunInfo> ProgramArguments { get; }
		internal Action<Queue<Stage<TRunInfo>>> ExtendPipeline { get; }

		private Queue<Stage<TRunInfo>> _stages { get; }
		private Queue<string> _programArguments { get; }


		// "Refreshed" per command/subCommand
		private HashSet<string> _subCommands { get; set; }
		internal ProcessOptions<TRunInfo> Options { get; private set; }
		internal int CommandLevel { get; private set; }

		internal ProcessContext(
			TRunInfo runInfo,
			//Func<CallbackContext<TRunInfo>> callbackContextFactory,
			StageFunctions<TRunInfo> stageCallbacks,
			ProgramArgumentCallbacks<TRunInfo> programArgumentCallbacks,
			Action<Queue<Stage<TRunInfo>>> extendPipelineCallback)
		{
			RunInfo = runInfo;
			//_callbackContextFactory = callbackContextFactory;
			Stages = stageCallbacks;
			ProgramArguments = programArgumentCallbacks;
			ExtendPipeline = extendPipelineCallback;
			CommandLevel = -1;
		}

		internal ProcessContext(
			TRunInfo runInfo,
			int commandLevel,
			Queue<Stage<TRunInfo>> stages,
			Queue<string> programArguments,
			CommandBase<TRunInfo> command)
		{
			RunInfo = runInfo;
			CommandLevel = commandLevel;
			_stages = stages;
			_programArguments = programArguments;

			Initialize(command);
		}

		private void Initialize(CommandBase<TRunInfo> command)
		{
			var subCommands = command is Command<TRunInfo> cmd
				? new HashSet<string>(cmd.SubCommands.Select(c => c.Key))
				: new HashSet<string>();

			Options = new ProcessOptions<TRunInfo>(command.Options);

			InitializeStageFunctions();

			//CommandLevel++;
		}

		private void InitializeStageFunctions()
		{
			Action<Queue<Stage<TRunInfo>>> extendPipelineFunc = newStages =>
			{
				while (newStages.Any())
				{
					_stages.Enqueue(newStages.Dequeue());
				}
			};

			Stages = new StageFunctions<TRunInfo>(
				_stages.Any, 
				_stages.Dequeue,
				extendPipelineFunc);
		}

		private void InitializeProgramArgumentFunctions()
		{

		}

		// todo: consider making each command level have its own context object.
		// constructor will take in this additional command argument, and a call to
		// this method will return a new process context
		internal ProcessContext<TRunInfo> RefreshForCommand(CommandBase<TRunInfo> command)
		{
			_subCommands = new HashSet<string>();
			Options = new ProcessOptions<TRunInfo>(command.Options);

			if (command is Command<TRunInfo> cmd)
			{
				_subCommands = new HashSet<string>(cmd.SubCommands.Select(c => c.Key));
			}

			CommandLevel++;

			return this;
		}

		//internal CallbackContext<TRunInfo> GetCallbackContext() => _callbackContextFactory();

		internal bool NextIsSubCommand() => _subCommands.Contains(ProgramArguments.Peek());

		internal bool NextIsOption() => Options.IsOption(ProgramArguments.Peek());
	}
}
