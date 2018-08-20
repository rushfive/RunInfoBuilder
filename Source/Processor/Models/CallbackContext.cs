using R5.RunInfoBuilder.Parser;
using System;

namespace R5.RunInfoBuilder
{
	public class CallbackContext<TRunInfo> where TRunInfo : class
	{
		public string CurrentToken { get; }
		public int Position { get; }
		public TRunInfo RunInfo { get; }
		public string[] ProgramArguments { get; }
		//public IArgumentParser Parser { get; }

		internal CallbackContext(
			string currentToken,
			int position,
			TRunInfo runInfo,
			string[] programArguments)
		{
			CurrentToken = currentToken;
			Position = position;
			RunInfo = runInfo;
			ProgramArguments = programArguments;
		}
	}
}
