using System;

namespace R5.RunInfoBuilder.Processor.Models
{
    internal class ProgramArgumentCallbacks<TRunInfo>
		where TRunInfo : class
    {
		internal Func<bool> HasMore { get; }
		internal Func<string> Peek { get; }
		internal Func<string> Dequeue { get; }

		internal ProgramArgumentCallbacks(
			Func<bool> hasMoreFunc,
			Func<string> peekFunc,
			Func<string> dequeueFunc)
		{
			HasMore = hasMoreFunc;
			Peek = peekFunc;
			Dequeue = dequeueFunc;
		}
	}
}
