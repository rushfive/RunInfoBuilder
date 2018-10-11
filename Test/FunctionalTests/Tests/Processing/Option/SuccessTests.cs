using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing.Option
{
	public class OptionSuccessTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		public class InSingleCommand
		{
			public class BoolProperty
			{
				[Fact]
				public void FullOption_ImplicitWithoutValue_BindsAsTrue()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, bool>
							{
								Key = "bool",
								Property = ri => ri.Bool1
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "--bool" });

					Assert.True(runInfo.Bool1);
				}

				[Fact]
				public void FullOption_ExplicitValue_InSameArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, bool>
							{
								Key = "bool",
								Property = ri => ri.Bool1
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "--bool=true" });

					Assert.True(runInfo.Bool1);
				}

				[Fact]
				public void FullOption_ExplicitValue_InNextArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, bool>
							{
								Key = "bool",
								Property = ri => ri.Bool1
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "--bool", "true" });

					Assert.True(runInfo.Bool1);
				}

				[Fact]
				public void ShortOption_ImplicitWithoutValue_BindsAsTrue()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, bool>
							{
								Key = "bool | b",
								Property = ri => ri.Bool1
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "-b" });

					Assert.True(runInfo.Bool1);
				}

				[Fact]
				public void ShortOption_ExplicitValue_InSameArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, bool>
							{
								Key = "bool | b",
								Property = ri => ri.Bool1
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "-b=true" });

					Assert.True(runInfo.Bool1);
				}

				[Fact]
				public void ShortOption_ExplicitValue_InNextArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, bool>
							{
								Key = "bool | b",
								Property = ri => ri.Bool1
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "-b", "true" });

					Assert.True(runInfo.Bool1);
				}

				// stacked only for bool types
				[Fact]
				public void Stacked_ImplicitWithoutValue_BindsAsTrue()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, bool>
							{
								Key = "one | o",
								Property = ri => ri.Bool1
							},
							new Option<TestRunInfo, bool>
							{
								Key = "two | t",
								Property = ri => ri.Bool2
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "-ot" });

					Assert.True(runInfo.Bool1);
					Assert.True(runInfo.Bool2);
				}

				[Fact]
				public void Stacked_ExplicitValue_InSameArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, bool>
							{
								Key = "one | o",
								Property = ri => ri.Bool1
							},
							new Option<TestRunInfo, bool>
							{
								Key = "two | t",
								Property = ri => ri.Bool2
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "-ot=true" });

					Assert.True(runInfo.Bool1);
					Assert.True(runInfo.Bool2);
				}

				[Fact]
				public void Stacked_ExplicitValue_InNextArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, bool>
							{
								Key = "one | o",
								Property = ri => ri.Bool1
							},
							new Option<TestRunInfo, bool>
							{
								Key = "two | t",
								Property = ri => ri.Bool2
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "-ot", "true" });

					Assert.True(runInfo.Bool1);
					Assert.True(runInfo.Bool2);
				}
			}

			public class NonBoolProperty
			{
				[Fact]
				public void FullOption_ExplicitValue_InSameArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int",
								Property = ri => ri.Int1
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "--int=99" });

					Assert.Equal(99, runInfo.Int1);
				}

				[Fact]
				public void FullOption_ExplicitValue_InNextArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int",
								Property = ri => ri.Int1
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "--int", "99" });

					Assert.Equal(99, runInfo.Int1);
				}

				[Fact]
				public void ShortOption_ExplicitValue_InSameArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri => ri.Int1
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "-i=99" });

					Assert.Equal(99, runInfo.Int1);
				}

				[Fact]
				public void ShortOption_ExplicitValue_InNextArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri => ri.Int1
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "-i", "99" });

					Assert.Equal(99, runInfo.Int1);
				}

				[Fact]
				public void ShortOption_OnProcessInvokes_IfSet()
				{
					RunInfoBuilder builder = GetBuilder();

					bool invoked = false;

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri => ri.Int1,
								OnProcess = arg =>
								{
									invoked = true;
									return ProcessResult.Continue;
								}
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "-i", "99" });

					Assert.Equal(99, runInfo.Int1);
					Assert.True(invoked);
				}

				[Fact]
				public void FullOption_OnProcessInvokes_IfSet()
				{
					RunInfoBuilder builder = GetBuilder();

					bool invoked = false;

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri => ri.Int1,
								OnProcess = arg =>
								{
									invoked = true;
									return ProcessResult.Continue;
								}
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "--int", "99" });

					Assert.Equal(99, runInfo.Int1);
					Assert.True(invoked);
				}
			}
		}

		public class InNestedCommand
		{
			public class BoolProperty
			{
				[Fact]
				public void FullOption_ImplicitWithoutValue_BindsAsTrue()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "subcommand",
								Options =
								{
									new Option<TestRunInfo, bool>
									{
										Key = "bool",
										Property = ri => ri.Bool1
									}
								}
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "--bool" });

					Assert.True(runInfo.Bool1);
				}

				[Fact]
				public void FullOption_ExplicitValue_InSameArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "subcommand",
								Options =
								{
									new Option<TestRunInfo, bool>
									{
										Key = "bool",
										Property = ri => ri.Bool1
									}
								}
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "--bool=true" });

					Assert.True(runInfo.Bool1);
				}

				[Fact]
				public void FullOption_ExplicitValue_InNextArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "subcommand",
								Options =
								{
									new Option<TestRunInfo, bool>
									{
										Key = "bool",
										Property = ri => ri.Bool1
									}
								}
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "--bool", "true" });

					Assert.True(runInfo.Bool1);
				}

				[Fact]
				public void ShortOption_ImplicitWithoutValue_BindsAsTrue()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "subcommand",
								Options =
								{
									new Option<TestRunInfo, bool>
									{
										Key = "bool | b",
										Property = ri => ri.Bool1
									}
								}
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "-b" });

					Assert.True(runInfo.Bool1);
				}

				[Fact]
				public void ShortOption_ExplicitValue_InSameArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "subcommand",
								Options =
								{
									new Option<TestRunInfo, bool>
									{
										Key = "bool | b",
										Property = ri => ri.Bool1
									}
								}
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "-b=true" });

					Assert.True(runInfo.Bool1);
				}

				[Fact]
				public void ShortOption_ExplicitValue_InNextArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "subcommand",
								Options =
								{
									new Option<TestRunInfo, bool>
									{
										Key = "bool | b",
										Property = ri => ri.Bool1
									}
								}
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "-b", "true" });

					Assert.True(runInfo.Bool1);
				}

				// stacked only for bool types
				[Fact]
				public void Stacked_ImplicitWithoutValue_BindsAsTrue()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "subcommand",
								Options =
								{
									new Option<TestRunInfo, bool>
									{
										Key = "one | o",
										Property = ri => ri.Bool1
									},
									new Option<TestRunInfo, bool>
									{
										Key = "two | t",
										Property = ri => ri.Bool2
									}
								}
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "-ot" });

					Assert.True(runInfo.Bool1);
					Assert.True(runInfo.Bool2);
				}

				[Fact]
				public void Stacked_ExplicitValue_InSameArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "subcommand",
								Options =
								{
									new Option<TestRunInfo, bool>
									{
										Key = "one | o",
										Property = ri => ri.Bool1
									},
									new Option<TestRunInfo, bool>
									{
										Key = "two | t",
										Property = ri => ri.Bool2
									}
								}
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "-ot=true" });

					Assert.True(runInfo.Bool1);
					Assert.True(runInfo.Bool2);
				}

				[Fact]
				public void Stacked_ExplicitValue_InNextArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "subcommand",
								Options =
								{
									new Option<TestRunInfo, bool>
									{
										Key = "one | o",
										Property = ri => ri.Bool1
									},
									new Option<TestRunInfo, bool>
									{
										Key = "two | t",
										Property = ri => ri.Bool2
									}
								}
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "-ot", "true" });

					Assert.True(runInfo.Bool1);
					Assert.True(runInfo.Bool2);
				}
			}

			public class NonBoolProperty
			{
				[Fact]
				public void FullOption_ExplicitValue_InSameArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "subcommand",
								Options =
								{
									new Option<TestRunInfo, int>
									{
										Key = "int",
										Property = ri => ri.Int1
									}
								}
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "--int=99" });

					Assert.Equal(99, runInfo.Int1);
				}

				[Fact]
				public void FullOption_ExplicitValue_InNextArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "subcommand",
								Options =
								{
									new Option<TestRunInfo, int>
									{
										Key = "int",
										Property = ri => ri.Int1
									}
								}
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "--int", "99" });

					Assert.Equal(99, runInfo.Int1);
				}

				[Fact]
				public void ShortOption_ExplicitValue_InSameArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "subcommand",
								Options =
								{
									new Option<TestRunInfo, int>
									{
										Key = "int | i",
										Property = ri => ri.Int1
									}
								}
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "-i=99" });

					Assert.Equal(99, runInfo.Int1);
				}

				[Fact]
				public void ShortOption_ExplicitValue_InNextArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "subcommand",
								Options =
								{
									new Option<TestRunInfo, int>
									{
										Key = "int | i",
										Property = ri => ri.Int1
									}
								}
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "-i", "99" });

					Assert.Equal(99, runInfo.Int1);
				}

				[Fact]
				public void ShortOption_OnProcessInvokes_IfSet()
				{
					RunInfoBuilder builder = GetBuilder();

					bool invoked = false;

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "subcommand",
								Options =
								{
									new Option<TestRunInfo, int>
									{
										Key = "int | i",
										Property = ri => ri.Int1,
										OnProcess = arg =>
										{
											invoked = true;
											return ProcessResult.Continue;
										}
									}
								}
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "-i", "99" });

					Assert.Equal(99, runInfo.Int1);
					Assert.True(invoked);
				}

				[Fact]
				public void FullOption_OnProcessInvokes_IfSet()
				{
					RunInfoBuilder builder = GetBuilder();

					bool invoked = false;

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "subcommand",
								Options =
								{
									new Option<TestRunInfo, int>
									{
										Key = "int | i",
										Property = ri => ri.Int1,
										OnProcess = arg =>
										{
											invoked = true;
											return ProcessResult.Continue;
										}
									}
								}
							}
						}
					});

					var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "--int", "99" });

					Assert.Equal(99, runInfo.Int1);
					Assert.True(invoked);
				}
			}
		}
	}
}
