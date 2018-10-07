using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Samples.HelpExamples.Commands
{
	public class InitRunInfo
	{
		// Options
		public bool Quiet { get; set; }
		public bool Bare { get; set; }
	}

	public class Init
	{
		public static Command<InitRunInfo> Configuration =>
			new Command<InitRunInfo>
			{
				Key = "init",
				Description = "Create an empty Git repository or reinitialize an existing one.",
				Options =
				{
					new Option<InitRunInfo, bool>
					{
						Key = "quiet | q",
						Property = ri => ri.Quiet
					},
					new Option<InitRunInfo, bool>
					{
						Key = "bare",
						Property = ri => ri.Bare
					}
				}
			};
	}
}
