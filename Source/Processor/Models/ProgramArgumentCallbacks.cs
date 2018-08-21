using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Processor.Models
{
    internal class ProgramArgumentCallbacks<TRunInfo>
		where TRunInfo : class
    {
		private Func<bool> _hasMoreCallback { get; }
		private Func<string> _peekCallback { get; }
		private Func<string> _dequeueCallback { get; }

		internal ProgramArgumentCallbacks(
			Func<bool> hasMoreCallback,
			Func<string> peekCallback,
			Func<string> dequeueCallback)
		{
			_hasMoreCallback = hasMoreCallback;
			_peekCallback = peekCallback;
			_dequeueCallback = dequeueCallback;
		}

		internal bool HasMore() => _hasMoreCallback();

		internal string Peek() => _peekCallback();

		internal string Dequeue() => _dequeueCallback();
	}
}
