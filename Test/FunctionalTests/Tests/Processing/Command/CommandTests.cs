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
								new Command<TestRunInfo>
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
		}

		
	}
}
