using R5.RunInfoBuilder.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public class ProcessContext<TRunInfo> where TRunInfo : class
	{
		//public ProgramArgumentType ArgumentType { get; }
		public string Token { get; }
		public int Position { get; }
		public TRunInfo RunInfo { get; }
		public string[] ProgramArguments { get; }
		public IArgumentParser Parser { get; }

		internal ProcessContext()
		{
			throw new NotImplementedException("todo");
		}
	}

	//public enum ProgramArgumentType
	//{
	//	ArgumentPropertyMapped,
	//	ArgumentSequence,
	//	ArgumentUnhandled,
	//	Command,
	//	OptionWithArguments,
	//	OptionAsFlag
	//}
}
