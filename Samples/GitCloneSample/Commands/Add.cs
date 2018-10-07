using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Samples.HelpExamples.Commands
{
	public class AddRunInfo
	{
		// Options
		public bool DryRun { get; set; }
		public bool Verbose { get; set; }
		public bool IgnoreErrors { get; set; }
		public bool Refresh { get; set; }
	}

	public class Add
	{
		public static Command<AddRunInfo> Configuration =>
			new Command<AddRunInfo>
			{
				Key = "add",
				Description = "Add file contents to the index.",
				Options =
				{
					new Option<AddRunInfo, bool>
					{
						Key = "dry-run | n",
						Property = ri => ri.DryRun
					},
					new Option<AddRunInfo, bool>
					{
						Key = "verbose | v",
						Property = ri => ri.Verbose
					},
					new Option<AddRunInfo, bool>
					{
						Key = "ignore-errors",
						Property = ri => ri.IgnoreErrors
					},
					new Option<AddRunInfo, bool>
					{
						Key = "refresh",
						Property = ri => ri.Refresh
					}
				}
			};
	}
}
