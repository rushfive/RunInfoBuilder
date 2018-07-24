using System.Collections.Generic;

namespace R5.RunInfoBuilder.Help
{
	public class HelpMetadata
	{
		public List<ArgumentHelpInfo> Arguments { get; }
		public List<CommandHelpInfo> Commands { get; }
		public List<OptionHelpInfo> Options { get; }
		public List<string> Triggers { get; }
		public string ProgramDescription { get; }

		internal HelpMetadata(
			List<ArgumentHelpInfo> arguments,
			List<CommandHelpInfo> commands,
			List<OptionHelpInfo> options,
			List<string> triggers,
			string programDescription)
		{
			Arguments = arguments;
			Commands = commands;
			Options = options;
			Triggers = triggers;
			ProgramDescription = programDescription;
		}
	}
}
