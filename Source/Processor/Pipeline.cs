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

		internal Pipeline(
			Queue<Stage<TRunInfo>> stages,
			string[] args)
		{
			_stages = stages;
			_args = args;

			_position = 0;
			_programArguments = new Queue<string>(args);
		}

		internal TRunInfo Process(TRunInfo runInfo)
		{
			ProcessContext<TRunInfo> processContext = GetProcessContext(runInfo);


			return runInfo;
		}

		private ProcessContext<TRunInfo> GetProcessContext(TRunInfo runInfo)
		{
			Func<string> dequeueProgramArgument = () =>
			{
				if (!_programArguments.Any())
				{
					throw new InvalidOperationException("Cannot dequeue because there's no more items.");
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
