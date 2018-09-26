using R5.RunInfoBuilder.Commands;
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
		internal StageFunctions<TRunInfo> Stages { get; private set; }
		internal ProgramArgumentFunctions ProgramArguments { get; private set; }
		internal OptionFunctions<TRunInfo> Options { get; private set; }

		private Queue<Stage<TRunInfo>> _stages { get; }
		private Queue<string> _programArguments { get; }

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

			InitializeStageFunctions();
			InitializeOptionFunctions(command.Options);
			InitializeProgramArgumentFunctions(command);
		}

		internal ProcessContext<TRunInfo> RecreateForCommand(CommandBase<TRunInfo> command)
		{
			return new ProcessContext<TRunInfo>(
				RunInfo, CommandLevel + 1, _stages, _programArguments, command);
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
			var subCommands = command is Command<TRunInfo> cmd
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
