using R5.RunInfoBuilder.Commands;
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
		private int _position { get; set; }
		private Queue<string> _programArguments { get; }
		private CommandBase<TRunInfo> _initialCommand { get; }

		internal Pipeline(
			Queue<Stage<TRunInfo>> stages,
			string[] args,
			CommandBase<TRunInfo> initialCommand)
		{
			_stages = stages;
			_args = args;
			_position = 0;
			_programArguments = new Queue<string>(args);
			_initialCommand = initialCommand;
		}

		internal TRunInfo Process()
		{
			TRunInfo runInfo = (TRunInfo)Activator.CreateInstance(typeof(TRunInfo));

			ProcessContext<TRunInfo> processContext = GetProcessContext(runInfo).RefreshForCommand(_initialCommand);

			while (_stages.Any())
			{
				Stage<TRunInfo> current = _stages.Dequeue();

				ProcessStageResult result = current.ProcessStage(processContext);

				if (result == ProcessResult.End)
				{
					break;
				}

			}

			return runInfo;
		}

		private ProcessContext<TRunInfo> GetProcessContext(TRunInfo runInfo)
		{
			Func<int, string> dequeueProgramArgument = (int commandLevel) =>
			{
				if (!_programArguments.Any())
				{
					throw new ProcessException("Cannot dequeue because there's no more items.",
						ProcessError.ExpectedProgramArgument, commandLevel);
				}

				_position++;
				return _programArguments.Dequeue();
			};

			Func<CallbackContext<TRunInfo>> callbackContextFactory =
				() => new CallbackContext<TRunInfo>(_args[_position], _position, runInfo, (string[])_args.Clone());

			var stageCallbacks = new StageCallbacks<TRunInfo>(_stages.Any, _stages.Dequeue);

			var programArgumentCallbacks = new ProgramArgumentCallbacks<TRunInfo>(
				_programArguments.Any, 
				_programArguments.Peek, 
				dequeueProgramArgument);

			Action<Queue<Stage<TRunInfo>>> extendPipeline = stagesQueue =>
			{
				while (stagesQueue.Any())
				{
					_stages.Enqueue(stagesQueue.Dequeue());
				}
			};

			return new ProcessContext<TRunInfo>(
				runInfo,
				callbackContextFactory,
				stageCallbacks,
				programArgumentCallbacks,
				extendPipeline);
		}
		
	}
}
