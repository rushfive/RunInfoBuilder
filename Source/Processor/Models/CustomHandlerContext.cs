using R5.RunInfoBuilder.Parser;
using System.Collections.Generic;

namespace R5.RunInfoBuilder
{
	public class CustomHandlerContext<TRunInfo> where TRunInfo : class
	{
		public TRunInfo RunInfo { get; }
		public List<string> ProgramArguments { get; }
		public ArgumentParser Parser { get; }

		internal CustomHandlerContext(
			TRunInfo runInfo,
			List<string> handledProgramArguments,
			ArgumentParser parser)
		{
			RunInfo = runInfo;
			ProgramArguments = handledProgramArguments;
			Parser = parser;
		}
	}
}
