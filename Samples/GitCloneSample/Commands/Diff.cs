using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Samples.HelpExamples.Commands
{
	public class DiffRunInfo
	{
		// Arguments
		public string FirstBranch { get; set; }
		public string SecondBranch { get; set; }

		// Options
		public bool NoPatch { get; set; }
		public bool Raw { get; set; }
		public bool Minimal { get; set; }
	}

	public class Diff
	{
		public static Command<DiffRunInfo> Configuration =>
			new Command<DiffRunInfo>
			{
				Key = "diff",
				Description = "Switch branches or restore working tree files.",
				Arguments =
				{
					new CustomArgument<DiffRunInfo>
					{
						HelpToken = "[first-branch]...[second-branch]",
						Count = 1,
						Handler = context =>
						{
							string[] branchSplit = context.ProgramArguments.Single().Split("...");
							context.RunInfo.FirstBranch = branchSplit[0];
							context.RunInfo.SecondBranch = branchSplit[1];
							return ProcessResult.Continue;
						}
					}
				},
				Options =
				{
					new Option<DiffRunInfo, bool>
					{
						Key = "no-patch | s",
						Property = ri => ri.NoPatch
					},
					new Option<DiffRunInfo, bool>
					{
						Key = "raw",
						Property = ri => ri.Raw
					},
					new Option<DiffRunInfo, bool>
					{
						Key = "minimal",
						Property = ri => ri.Minimal
					}
				}
			};
	}
}
