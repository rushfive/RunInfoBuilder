using R5.RunInfoBuilder.Parser;
using System.Collections.Generic;

namespace R5.RunInfoBuilder.Processor.Models
{
	public class CustomHandlerContext<TRunInfo> where TRunInfo : class
	{
		public TRunInfo RunInfo { get; }
		public List<string> HandledProgramArguments { get; }
		public IArgumentParser Parser { get; }

		internal CustomHandlerContext(
			TRunInfo runInfo,
			List<string> handledProgramArguments,
			IArgumentParser parser)
		{
			RunInfo = runInfo;
			HandledProgramArguments = handledProgramArguments;
			Parser = parser;
		}
	}
}
