using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public class CallbackContext<TRunInfo> where TRunInfo : class
	{
		public TRunInfo RunInfo { get; }
		public ProgramArgumentType ArgumentType { get; }
		public int Position { get; }
		public string Token { get; }

		internal CallbackContext()
		{
			throw new NotImplementedException("todo");
		}
	}

	public enum ProgramArgumentType
	{
		ArgumentPropertyMapped,
		ArgumentExclusiveSet,
		ArgumentSequence,
		ArgumentUnhandled,
		Command,
		CommandPropertyMapped,
		OptionWithArguments,
		OptionAsFlag
	}
}
