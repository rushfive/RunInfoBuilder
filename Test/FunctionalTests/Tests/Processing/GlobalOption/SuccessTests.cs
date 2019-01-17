using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing.GlobalOption
{
	public class SuccessTests
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
				public void FullGlobalOption_ImplicitWithoutValue_BindsAsTrue()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
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
				public void FullGlobalOption_ExplicitValue_InSameArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
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
				public void FullGlobalOption_ExplicitValue_InNextArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
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
				public void ShortGlobalOption_ImplicitWithoutValue_BindsAsTrue()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
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
				public void ShortGlobalOption_ExplicitValue_InSameArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
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
				public void ShortGlobalOption_ExplicitValue_InNextArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
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
				public void StackedGlobalOptions_ImplicitWithoutValue_BindsAsTrue()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
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
				public void StackedGlobalOptions_ExplicitValue_InSameArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
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
				public void StackedGlobalOptions_ExplicitValue_InNextArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
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

				[Fact]
				public void StackedGlobalAndCommandOption_ImplicitWithoutValue_BindsAsTrue()
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
							}
						},
						GlobalOptions =
						{
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
				public void StackedGlobalAndCommandOption_ExplicitValue_InSameArgumentToken_BindsAsParsed()
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
							}
						},
						GlobalOptions =
						{
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
				public void StackedGlobalAndCommandOption_ExplicitValue_InNextArgumentToken_BindsAsParsed()
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
							}
						},
						GlobalOptions =
						{
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
				public void FullGlobalOption_ExplicitValue_InSameArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
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
				public void FullGlobalOption_ExplicitValue_InNextArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
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
				public void ShortGlobalOption_ExplicitValue_InSameArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
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
				public void ShortGlobalOption_ExplicitValue_InNextArgumentToken_BindsAsParsed()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
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
				public void ShortGlobalOption_OnProcessInvokes_IfSet()
				{
					RunInfoBuilder builder = GetBuilder();

					bool invoked = false;

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri => ri.Int1,
								OnParsed = arg =>
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
				public void FullGlobalOption_OnProcessInvokes_IfSet()
				{
					RunInfoBuilder builder = GetBuilder();

					bool invoked = false;

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri => ri.Int1,
								OnParsed = arg =>
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
			[Theory]
			[InlineData("--bool")]
			[InlineData("-b")]
			public void GlobalOptions_AreAvailable_AtSubRootLevels(string option)
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "command",
					SubCommands =
					{
						new SubCommand<TestRunInfo>
						{
							Key = "subcommand"
						}
					},
					GlobalOptions =
					{
						new Option<TestRunInfo, bool>
						{
							Key = "bool | b",
							Property = ri => ri.Bool1
						}
					}
				});

				var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", option });

				Assert.True(runInfo.Bool1);
			}

			[Fact]
			public void GlobalOptions_AreUseable_WithOther_CommandScopedOptions()
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "command",
					Options =
					{
						new Option<TestRunInfo, bool>
						{
							Key = "bool2 | c",
							Property = ri => ri.Bool2
						}
					},
					SubCommands =
					{
						new SubCommand<TestRunInfo>
						{
							Key = "subcommand",
							Options =
							{
								new Option<TestRunInfo, bool>
								{
									Key = "bool3 | b",
									Property = ri => ri.Bool3
								}
							}
						}
					},
					GlobalOptions =
					{
						new Option<TestRunInfo, bool>
						{
							Key = "bool1 | a",
							Property = ri => ri.Bool1
						}
					}
				});

				var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "--bool2", "subcommand", "--bool3", "--bool1" });

				Assert.True(runInfo.Bool1);
				Assert.True(runInfo.Bool2);
				Assert.True(runInfo.Bool3);

				// move the global '--bool1' to before the subcommand
				runInfo = (TestRunInfo)builder.Build(new string[] { "command", "--bool2", "--bool1", "subcommand", "--bool3" });
				Assert.True(runInfo.Bool1);
				Assert.True(runInfo.Bool2);
				Assert.True(runInfo.Bool3);
			}

			[Fact]
			public void GlobalOption_ShortKeysAreStackable_WithOther_CommandScopedOptions()
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "command",
					Options =
					{
						new Option<TestRunInfo, bool>
						{
							Key = "bool2 | c",
							Property = ri => ri.Bool2
						}
					},
					SubCommands =
					{
						new SubCommand<TestRunInfo>
						{
							Key = "subcommand",
							Options =
							{
								new Option<TestRunInfo, bool>
								{
									Key = "bool3 | b",
									Property = ri => ri.Bool3
								}
							}
						}
					},
					GlobalOptions =
					{
						new Option<TestRunInfo, bool>
						{
							Key = "bool1 | a",
							Property = ri => ri.Bool1
						}
					}
				});

				var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "-ab" });

				Assert.True(runInfo.Bool1);
				Assert.False(runInfo.Bool2);
				Assert.True(runInfo.Bool3);

				runInfo = (TestRunInfo)builder.Build(new string[] { "command", "-ac", "subcommand" });
				Assert.True(runInfo.Bool1);
				Assert.True(runInfo.Bool2);
				Assert.False(runInfo.Bool3);
			}
		}
	}
}
