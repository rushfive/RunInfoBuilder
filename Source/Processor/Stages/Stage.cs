using R5.RunInfoBuilder.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Processor.Stages
{
    internal abstract class Stage<TRunInfo>
		where TRunInfo : class
    {
		private ArgumentsQueue _queue { get; }
		private Func<ProcessContext<TRunInfo>, ProcessStageResult> _callback { get; }

		protected Stage(
			ArgumentsQueue queue,
			Func<ProcessContext<TRunInfo>, ProcessStageResult> callback)
		{
			_queue = queue;
			_callback = callback;
		}

		protected abstract ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context);

		protected bool MoreProgramArgumentsExist() => _queue.HasNext();

		protected string Peek() => _queue.Peek();

		protected string Dequeue() => _queue.Dequeue();
		
		protected ProcessStageResult InvokeCallback(ProcessContext<TRunInfo> context)
		{
			if (_callback == null)
			{
				return ProcessResult.Continue;
			}

			return _callback(context);
		}
	}
}
