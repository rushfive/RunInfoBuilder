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

		internal StageQueueCallbacks<TRunInfo> GetStageQueueCallbacks()
			=> new StageQueueCallbacks<TRunInfo>(_stages.Any, _stages.Dequeue);

		internal ProgramArgumentQueueCallbacks<TRunInfo> GetProgramArgumentQueueCallbacks()
			=> new ProgramArgumentQueueCallbacks<TRunInfo>(_programArguments.Any, _programArguments.Peek, DequeueProgramArgument);

		private string DequeueProgramArgument()
		{
			if (!_programArguments.Any())
			{
				throw new InvalidOperationException("Cannot dequeue because there's no more items.");
			}

			_position++;
			return _programArguments.Dequeue();
		}

		internal void ExtendPipeline(Queue<Stage<TRunInfo>> subCommandPipeline)
		{
			while (subCommandPipeline.Any())
			{
				_stages.Enqueue(subCommandPipeline.Dequeue());
			}
		}
	}
}
