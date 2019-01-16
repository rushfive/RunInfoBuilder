using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor.Functions;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R5.RunInfoBuilder.Processor.Models
{
    internal class ProcessContext<TRunInfo> where TRunInfo : class
    {
		internal TRunInfo RunInfo { get; }
		internal int CommandLevel { get; }
		internal ArgumentParser Parser { get; }
		internal StageFunctions<TRunInfo> Stages { get; private set; }
		internal ProgramArgumentFunctions ProgramArguments { get; private set; }
		internal OptionFunctions<TRunInfo> Options { get; private set; }

		private Queue<Stage<TRunInfo>> _stages { get; }
		private Queue<string> _programArguments { get; }
		private List<OptionBase<TRunInfo>> _globalOptions { get; }

		internal ProcessContext(
			TRunInfo runInfo,
			int commandLevel,
			ArgumentParser parser,
			Queue<Stage<TRunInfo>> stages,
			Queue<string> programArguments,
			CommandBase<TRunInfo> command,
			List<OptionBase<TRunInfo>> globalOptions)
		{
			RunInfo = runInfo;
			CommandLevel = commandLevel;
			Parser = parser;
			_stages = stages;
			_programArguments = programArguments;
			_globalOptions = globalOptions;

			List<OptionBase<TRunInfo>> options = command.Options;
			if (globalOptions != null)
			{
				options = options.Concat(globalOptions).ToList();
			}

			InitializeStageFunctions();
			InitializeOptionFunctions(options);
			InitializeProgramArgumentFunctions(command);
		}

		internal ProcessContext<TRunInfo> RecreateForCommand(CommandBase<TRunInfo> command)
		{
			return new ProcessContext<TRunInfo>(
				RunInfo, CommandLevel + 1, Parser, _stages, _programArguments, command, _globalOptions);
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
		
		private void InitializeOptionFunctions(List<OptionBase<TRunInfo>> options)
		{
			Options = new OptionFunctions<TRunInfo>(options);
		}

		private void InitializeProgramArgumentFunctions(CommandBase<TRunInfo> command)
		{
			var subCommands = command is StackableCommand<TRunInfo> cmd
				? new HashSet<string>(cmd.SubCommands.Select(c => c.Key))
				: new HashSet<string>();

			ProgramArguments = new ProgramArgumentFunctions(
				_programArguments.Any,
				_programArguments.Peek,
				_programArguments.Dequeue,
				subCommands,
				Options.IsOption);
		}
	}
}
