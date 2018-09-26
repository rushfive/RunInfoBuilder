using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder.Processor.Models
{
    internal class ProgramArgumentFunctions
    {
		internal Func<bool> HasMore { get; }
		internal Func<string> Peek { get; }
		internal Func<string> Dequeue { get; }
		internal Func<bool> NextIsSubCommand { get; }
		internal Func<bool> NextIsOption { get; }
		
		internal ProgramArgumentFunctions(
			Func<bool> hasMoreFunc,
			Func<string> peekFunc,
			Func<string> dequeueFunc,
			HashSet<string> subCommands,
			Func<string, bool> isOptionFunc)
		{
			HasMore = hasMoreFunc;
			Peek = peekFunc;
			Dequeue = dequeueFunc;
			NextIsSubCommand = () => subCommands.Contains(Peek());
			NextIsOption = () => isOptionFunc(Peek());
		}

		//internal bool NextIsSubCommand() => _subCommands.Contains(Peek());

		
	}
}
