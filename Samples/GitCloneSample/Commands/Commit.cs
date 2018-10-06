using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Samples.HelpExamples.Commands
{
	public class CommitRunInfo
	{
		// Options
		public bool All { get; set; }
		public bool Patch { get; set; }
		public bool Branch { get; set; }
	}

	public class Commit
	{
		public static Command<CommitRunInfo> GetCommandConfiguration()
		{
			return new Command<CommitRunInfo>
			{
				Key = "commit",
				Description = "Stores the current contents of the index in a new commit along with a log message from the user describing the changes.",
				Options =
				{
					new Option<CommitRunInfo, bool>
					{
						Key = "all | a",
						Property = ri => ri.All
					},
					new Option<CommitRunInfo, bool>
					{
						Key = "patch | p",
						Property = ri => ri.Patch
					},
					new Option<CommitRunInfo, bool>
					{
						Key = "branch",
						Property = ri => ri.Branch
					}
				},
				SubCommands =
				{
					new Command<CommitRunInfo>
					{
						Key = "sub1",
						Options =
						{
							new Option<CommitRunInfo, bool>
							{
								Key = "branch",
								Property = ri => ri.Branch
							}
						},
						Arguments =
						{
							new PropertyArgument<CommitRunInfo, bool>
							{
								Property = ri => ri.Branch,
								HelpToken = "<shutdown>"
							}
						}	
					},
					new Command<CommitRunInfo>
					{
						Key = "sub2",
						Options =
						{
							new Option<CommitRunInfo, bool>
							{
								Key = "branch",
								Property = ri => ri.Branch
							}
						},
						Arguments =
						{
							new PropertyArgument<CommitRunInfo, bool>
							{
								Property = ri => ri.Branch,
								HelpToken = "<shutdown>"
							}
						},
						SubCommands =
						{
							new Command<CommitRunInfo>
							{
								Key = "sub1",
								Options =
								{
									new Option<CommitRunInfo, bool>
									{
										Key = "branch",
										Property = ri => ri.Branch
									}
								},
								Arguments =
								{
									new PropertyArgument<CommitRunInfo, bool>
									{
										Property = ri => ri.Branch,
										HelpToken = "<shutdown>"
									}
								}
							},
							new Command<CommitRunInfo>
							{
								Key = "sub2",
								Options =
								{
									new Option<CommitRunInfo, bool>
									{
										Key = "branch",
										Property = ri => ri.Branch
									}
								},
								Arguments =
								{
									new PropertyArgument<CommitRunInfo, bool>
									{
										Property = ri => ri.Branch,
										HelpToken = "<shutdown>"
									}
								}
							}
						}
					}
				}
			};
		}

		public static Command<CommitRunInfo> GetCommandConfiguration2()
		{
			return new Command<CommitRunInfo>
			{
				Key = "commit2",
				Description = "Stores the current contents of the index in a new commit along with a log message from the user describing the changes.",
				Options =
				{
					new Option<CommitRunInfo, bool>
					{
						Key = "all | a",
						Property = ri => ri.All
					},
					new Option<CommitRunInfo, bool>
					{
						Key = "patch | p",
						Property = ri => ri.Patch
					},
					new Option<CommitRunInfo, bool>
					{
						Key = "branch",
						Property = ri => ri.Branch
					}
				}
			};
		}
	}
}
