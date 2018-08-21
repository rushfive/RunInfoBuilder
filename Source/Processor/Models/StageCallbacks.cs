using R5.RunInfoBuilder.Processor.Stages;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Processor.Models
{
    internal class StageCallbacks<TRunInfo>
		where TRunInfo : class
    {
		private Func<bool> _hasMoreCallback { get; }
		private Func<Stage<TRunInfo>> _dequeueCallback { get; }

		internal StageCallbacks(
			Func<bool> hasMoreCallback,
			Func<Stage<TRunInfo>> dequeueCallback)
		{
			_hasMoreCallback = hasMoreCallback;
			_dequeueCallback = dequeueCallback;
		}

		internal bool HasMore() => _hasMoreCallback();

		internal Stage<TRunInfo> Dequeue() => _dequeueCallback();
	}
}
