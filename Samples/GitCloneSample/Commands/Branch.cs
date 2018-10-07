using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Samples.HelpExamples.Commands
{
	public class BranchRunInfo
	{
		// Arguments
		public string BranchName { get; set; }

		// Options
		public bool Delete { get; set; }
		public bool Force { get; set; }
		public bool IgnoreCase { get; set; }
	}

	public class Branch
	{
		public static Command<BranchRunInfo> Configuration =>
			new Command<BranchRunInfo>
			{
				Key = "branch",
				Description = "List, create, or delete branches.",
				Arguments =
				{
					new PropertyArgument<BranchRunInfo, string>
					{
						HelpToken = "<branchname>",
						Property = ri => ri.BranchName
					}
				},
				Options =
				{
					new Option<BranchRunInfo, bool>
					{
						Key = "delete | d",
						Property = ri => ri.Delete
					},
					new Option<BranchRunInfo, bool>
					{
						Key = "force | f",
						Property = ri => ri.Force
					},
					new Option<BranchRunInfo, bool>
					{
						Key = "ignore-case | i",
						Property = ri => ri.IgnoreCase
					}
				}
			};
	}
}
