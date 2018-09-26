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
		private Queue<string> _programArguments { get; }
		private CommandBase<TRunInfo> _initialCommand { get; }

		internal Pipeline(
			Queue<Stage<TRunInfo>> stages,
			string[] args,
			CommandBase<TRunInfo> initialCommand)
		{
			_stages = stages;
			_args = args;
			_programArguments = new Queue<string>(args);
			_initialCommand = initialCommand;
		}

		internal TRunInfo Process()
		{
			TRunInfo runInfo = (TRunInfo)Activator.CreateInstance(typeof(TRunInfo));

			//ProcessContext<TRunInfo> processContext = GetProcessContext(runInfo).RefreshForCommand(_initialCommand);
			ProcessContext<TRunInfo> context = new ProcessContext<TRunInfo>(
				runInfo, 0, _stages, _programArguments, _initialCommand);

			Action<CommandBase<TRunInfo>> resetContextFunc = cmd =>
			{
				context = context.RefreshForCommand(cmd);
			};

			bool ended = false;
			while (_stages.Any())
			{
				Stage<TRunInfo> current = _stages.Dequeue();

				ProcessStageResult result = current.ProcessStage(context);

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

		//private ProcessContext<TRunInfo> GetProcessContext(TRunInfo runInfo)
		//{
		//	//Func<CallbackContext<TRunInfo>> callbackContextFactory =
		//	//	() => new CallbackContext<TRunInfo>(_args[_position], _position, runInfo, (string[])_args.Clone());

		//	//var stageCallbacks = new StageFunctions<TRunInfo>(_stages.Any, _stages.Dequeue);

		//	//var programArgumentCallbacks = new ProgramArgumentFunctions<TRunInfo>(
		//	//	_programArguments.Any, 
		//	//	_programArguments.Peek, 
		//	//	_programArguments.Dequeue);

		//	//Action<Queue<Stage<TRunInfo>>> extendPipeline = stagesQueue =>
		//	//{
		//	//	while (stagesQueue.Any())
		//	//	{
		//	//		_stages.Enqueue(stagesQueue.Dequeue());
		//	//	}
		//	//};

		//	return new ProcessContext<TRunInfo>(
		//		runInfo, 0, _stages, _programArguments, )
		//		//callbackContextFactory,
		//		stageCallbacks,
		//		programArgumentCallbacks,
		//		extendPipeline);
		//}
		
	}
}
