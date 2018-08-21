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



			return runInfo;
		}

		// todo: access modifier
		internal Func<CallbackContext<TRunInfo>> GetCallbackContextFactory(string[] args, TRunInfo runInfo) => 
			() => new CallbackContext<TRunInfo>(args[_position], _position, runInfo, (string[])args.Clone());

		private bool HasMoreStages() => _stages.Any();

		private Stage<TRunInfo> DequeueStage() => _stages.Dequeue();


		// program args
		private bool HasMoreProgramArguments() => _programArguments.Any();
		
		private string PeekProgramArgument() => _programArguments.Peek();

		private string DequeueProgramArgument()
		{
			if (!_programArguments.Any())
			{
				throw new InvalidOperationException("Cannot dequeue because there's no more items.");
			}

			_position++;
			return _programArguments.Dequeue();
		}

		internal StageQueueCallbacks<TRunInfo> GetStageQueueCallbacks()
			=> new StageQueueCallbacks<TRunInfo>(HasMoreStages, DequeueStage);

		internal ProgramArgumentQueueCallbacks<TRunInfo> GetProgramArgumentQueueCallbacks()
			=> new ProgramArgumentQueueCallbacks<TRunInfo>(HasMoreProgramArguments, PeekProgramArgument, DequeueProgramArgument);
	}
}
