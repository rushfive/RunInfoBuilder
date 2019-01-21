using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor.Models;
using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Processor
{
	internal class Pipeline<TRunInfo>
		where TRunInfo : class
	{
		private Queue<Stage<TRunInfo>> _stages { get; }
		private string[] _args { get; }
		private Queue<string> _programArguments { get; }
		private CommandBase<TRunInfo> _initialCommand { get; }
		private ArgumentParser _parser { get; }
		private List<OptionBase<TRunInfo>> _globalOptions { get; }

		internal Pipeline(
			Queue<Stage<TRunInfo>> stages,
			string[] args,
			CommandBase<TRunInfo> initialCommand,
			ArgumentParser parser,
			List<OptionBase<TRunInfo>> globalOptions)
		{
			_stages = stages;
			_args = args;
			_programArguments = new Queue<string>(args);
			_initialCommand = initialCommand;
			_parser = parser;
			_globalOptions = globalOptions;
		}

		internal TRunInfo Process()
		{
			TRunInfo runInfo = (TRunInfo)Activator.CreateInstance(typeof(TRunInfo));

			ProcessContext<TRunInfo> context = new ProcessContext<TRunInfo>(
				runInfo, 0, _parser, _stages, _programArguments, _initialCommand, _globalOptions);

			Action<CommandBase<TRunInfo>> resetContextFunc = cmd =>
			{
				context = context.RecreateForCommand(cmd);
			};

			bool ended = false;
			while (_stages.Any())
			{
				Stage<TRunInfo> current = _stages.Dequeue();

				ProcessStageResult result = current.ProcessStage(context, resetContextFunc);

				switch (result)
				{
					case Continue _:
						break;
					case End _:
						ended = true;
						break;
					case null:
					default:
						throw new ProcessException(
							"Current stage processing returned an invalid result.",
							ProcessError.InvalidStageResult, context.CommandLevel);
				}

				if (ended)
				{
					break;
				}
			}

			return runInfo;
		}
	}
}
