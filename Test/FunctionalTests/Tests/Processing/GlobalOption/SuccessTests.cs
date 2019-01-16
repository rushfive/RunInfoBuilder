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
	}
}
