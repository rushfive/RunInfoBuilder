using System.Collections.Generic;
using System.Linq;

namespace R5.RunInfoBuilder.Process
{
	public class ProcessContext<TRunInfo>
		where TRunInfo : class
	{
		public string ArgumentToken { get; }
		public ProgramArgumentType ArgumentType { get; }
		public int Position { get; }
		public string[] ProgramArguments { get; }
		public TRunInfo RunInfo { get; }

		internal ProcessContext(
			ProgramArgument argument,
			List<ProgramArgument> arguments,
			TRunInfo runInfo)
		{
			ArgumentToken = argument.ArgumentToken;
			ArgumentType = argument.Type;
			Position = argument.Position;
			ProgramArguments = arguments.Select(a => a.ArgumentToken).ToArray();
			RunInfo = runInfo;
		}
	}
}
