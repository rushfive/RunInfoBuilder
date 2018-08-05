using OLD.ArgumentParser;
using System.Collections.Generic;
using System.Linq;

namespace OLD.Process
{
	public class ProcessContext<TRunInfo>
		where TRunInfo : class
	{
		public string ArgumentToken { get; }
		public ProgramArgumentType ArgumentType { get; }
		public int Position { get; }
		public List<ProgramArgument> ProgramArguments { get; }
		public TRunInfo RunInfo { get; }
		public IParser Parser { get; }

		internal ProcessContext(
			ProgramArgument argument,
			List<ProgramArgument> arguments,
			TRunInfo runInfo,
			IParser parser)
		{
			ArgumentToken = argument.ArgumentToken;
			ArgumentType = argument.Type;
			Position = argument.Position;
			ProgramArguments = arguments.ToList();
			RunInfo = runInfo;
			Parser = parser;
		}
	}
}
