using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing.PropertyArgument
{
	public class PropertyArgumentSuccessTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		public class InSingleCommand
		{
			[Fact]
			public void ParsedArgumentValues_SuccessfullyBindToRunInfo()
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "command",
					Arguments =
					{
						new PropertyArgument<TestRunInfo, bool>
						{
							Property = ri => ri.Bool1
						},
						new PropertyArgument<TestRunInfo, int>
						{
							Property = ri => ri.Int1
						},
						new PropertyArgument<TestRunInfo, string>
						{
							Property = ri => ri.String1
						}
					}
				});

				var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "true", "99", "parsed" });

				Assert.True(runInfo.Bool1);
				Assert.Equal(99, runInfo.Int1);
				Assert.Equal("parsed", runInfo.String1);
			}
		}

		public class InNestedCommand
		{
			[Fact]
			public void ParsedArgumentValues_SuccessfullyBindToRunInfo()
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
							Arguments =
							{
								new PropertyArgument<TestRunInfo, bool>
								{
									Property = ri => ri.Bool1
								},
								new PropertyArgument<TestRunInfo, int>
								{
									Property = ri => ri.Int1
								},
								new PropertyArgument<TestRunInfo, string>
								{
									Property = ri => ri.String1
								}
							}
						}
					}
				});

				var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "true", "99", "parsed" });

				Assert.True(runInfo.Bool1);
				Assert.Equal(99, runInfo.Int1);
				Assert.Equal("parsed", runInfo.String1);
			}
		}
	}
}
