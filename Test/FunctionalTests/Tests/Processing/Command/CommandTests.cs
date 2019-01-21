using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing.Command
{
	public class CommandTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		public class InSingleCommand
		{
			public class PostBuildCallback
			{
				[Fact]
				public void OnSuccessfulBuild_Invokes()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(
						new Command<TestRunInfo>
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
						},
						runInfo =>
						{
							Assert.True(runInfo.Bool1);
						});

					builder.Build(new string[] { "command", "--bool" });
				}
			}

			public class OnMatchedCallback
			{
				[Fact]
				public void NotNull_Invokes()
				{
					RunInfoBuilder builder = GetBuilder();

					bool flag = false;

					builder.Commands.Add(
						new Command<TestRunInfo>
						{
							Key = "command",
							OnMatched = runInfo =>
							{
								flag = true;
								return ProcessResult.Continue;
							}
						});

					Assert.False(flag);
					builder.Build(new string[] { "command" });
					Assert.True(flag);
				}

				[Fact]
				public void EndResult_StopsFurtherProcessing()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(
						new Command<TestRunInfo>
						{
							Key = "command",
							OnMatched = ri => ProcessResult.End,
							Options =
							{
								new Option<TestRunInfo, bool>
								{
									Key = "bool",
									Property = ri => ri.Bool1
								}
							}
						});

					var runInfo = builder.Build(new string[] { "command", "--bool" }) as TestRunInfo;
					Assert.False(runInfo?.Bool1);
				}

				[Fact]
				public void ContinueResult_CorrectlySetsFlag()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(
						new Command<TestRunInfo>
						{
							Key = "command",
							OnMatched = ri =>
							{
								ri.String1 = "SetFromOnMatched";

								return ProcessResult.Continue;
							},
							Options =
							{
								new Option<TestRunInfo, bool>
								{
									Key = "bool",
									Property = ri => ri.Bool1
								}
							}
						});

					var runInfo = builder.Build(new string[] { "command", "--bool" }) as TestRunInfo;
					Assert.True(runInfo?.Bool1);
					Assert.Equal("SetFromOnMatched", runInfo?.String1);
				}
			}
		}

		public class InNestedCommand
		{
			public class PostBuildCallback
			{
				[Fact]
				public void OnSuccessfulBuild_Invokes()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(
						new Command<TestRunInfo>
						{
							Key = "command",
							Options =
							{
								new Option<TestRunInfo, bool>
								{
									Key = "bool",
									Property = ri => ri.Bool1
								}
							},
							SubCommands =
							{
								new SubCommand<TestRunInfo>
								{
									Key = "subcommand",
									Arguments =
									{
										new PropertyArgument<TestRunInfo, int>
										{
											Property = ri => ri.Int1
										}
									}
								}
							}
						},
						runInfo =>
						{
							Assert.True(runInfo.Bool1);
							Assert.Equal(99, runInfo.Int1);
						});

					builder.Build(new string[] { "command", "--bool", "subcommand", "99" });
				}
			}

			public class OnMatchedCallback
			{
				[Fact]
				public void NotNull_Invokes()
				{
					RunInfoBuilder builder = GetBuilder();

					bool flag = false;

					builder.Commands.Add(
						new Command<TestRunInfo>
						{
							Key = "command",
							SubCommands =
							{
								new SubCommand<TestRunInfo>
								{
									Key = "subcommand",
									OnMatched = runInfo =>
									{
										flag = true;
										return ProcessResult.Continue;
									}
								}
							}
						});

					Assert.False(flag);
					builder.Build(new string[] { "command", "subcommand" });
					Assert.True(flag);
				}

				[Fact]
				public void EndResult_StopsFurtherProcessing()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(
						new Command<TestRunInfo>
						{
							Key = "command",
							SubCommands =
							{
								new SubCommand<TestRunInfo>
								{
									Key = "subcommand",
									OnMatched = ri => ProcessResult.End,
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

					var runInfo = builder.Build(new string[] { "command", "subcommand", "--bool" }) as TestRunInfo;
					Assert.False(runInfo?.Bool1);
				}

				[Fact]
				public void ContinueResult_CorrectlySetsFlag()
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(
						new Command<TestRunInfo>
						{
							Key = "command",
							SubCommands =
							{
								new SubCommand<TestRunInfo>
								{
									Key = "subcommand",
									OnMatched = ri =>
									{
										ri.String1 = "SetFromOnMatched";

										return ProcessResult.Continue;
									},
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

					var runInfo = builder.Build(new string[] { "command", "subcommand", "--bool" }) as TestRunInfo;
					Assert.True(runInfo?.Bool1);
					Assert.Equal("SetFromOnMatched", runInfo?.String1);
				}
			}
		}
	}
}
