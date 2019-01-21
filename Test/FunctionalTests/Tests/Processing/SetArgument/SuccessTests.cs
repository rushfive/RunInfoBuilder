using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing.SetArgument
{
	public class SuccessTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		public class InSingleCommand
		{
			[Fact]
			public void Valid_ProgramArgumentValue_SuccessfullyBinds()
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "command",
					Arguments =
					{
						new SetArgument<TestRunInfo, bool>
						{
							Property = ri => ri.Bool1,
							Values = new List<(string, bool)>
							{
								("true", true), ("false", false)
							}
						}
					}
				});

				var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "true" });

				Assert.True(runInfo.Bool1);
			}
		}

		public class InNestedCommand
		{
			[Fact]
			public void Valid_ProgramArgumentValue_SuccessfullyBinds()
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "command",
					SubCommands =
					{
						new SubCommand<TestRunInfo>
						{
							Key = "subcommand",
							Arguments =
							{
								new SetArgument<TestRunInfo, bool>
								{
									Property = ri => ri.Bool1,
									Values = new List<(string, bool)>
									{
										("true", true), ("false", false)
									}
								}
							}
						}
					}
				});

				var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "subcommand", "true" });

				Assert.True(runInfo.Bool1);
			}
		}
	}
}
