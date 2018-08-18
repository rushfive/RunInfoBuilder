using R5.RunInfoBuilder.Parser;
using System;

namespace R5.RunInfoBuilder
{
	public class CallbackContext<TRunInfo> where TRunInfo : class
	{
		public string Token { get; }
		public int Position { get; }
		public TRunInfo RunInfo { get; }
		public string[] ProgramArguments { get; }
		public IArgumentParser Parser { get; }

		internal CallbackContext()
		{
			throw new NotImplementedException("todo");
		}
	}
}
