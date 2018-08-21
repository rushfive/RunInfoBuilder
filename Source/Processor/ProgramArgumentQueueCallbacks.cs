using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Processor
{
    internal class ProgramArgumentQueueCallbacks<TRunInfo>
		where TRunInfo : class
    {
		private Func<bool> _hasMoreProgramArguments { get; }
		private Func<string> _peekProgramArgument { get; }
		private Func<string> _dequeueProgramArgument { get; }

		internal ProgramArgumentQueueCallbacks(
			Func<bool> hasMoreProgramArgumentsCallback,
			Func<string> peekProgramArgumentCallback,
			Func<string> dequeueProgramArgumentCallback)
		{
			_hasMoreProgramArguments = hasMoreProgramArgumentsCallback;
			_peekProgramArgument = peekProgramArgumentCallback;
			_dequeueProgramArgument = dequeueProgramArgumentCallback;
		}

		internal bool HasMoreProgramArguments() => _hasMoreProgramArguments();

		internal string PeekProgramArgument() => _peekProgramArgument();

		internal string DequeueProgramArgument() => _dequeueProgramArgument();
	}
}
