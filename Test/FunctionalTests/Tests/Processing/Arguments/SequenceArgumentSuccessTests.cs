using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing.Arguments
{
	public class SequenceArgumentSuccessTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		public class InSingleCommand
		{
			[Fact]
			public void ParsedValues_SuccessfullyBind_ToListProperty()
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "command",
					Arguments =
					{
						new SequenceArgument<TestRunInfo, int>
						{
							ListProperty = ri => ri.IntList1
						}
					}
				});

				var runInfo = (TestRunInfo)builder.Build(new string[] { "command", "1", "2", "3" });

				Assert.Equal(3, runInfo.IntList1.Count);
				Assert.Equal(1, runInfo.IntList1[0]);
				Assert.Equal(2, runInfo.IntList1[1]);
				Assert.Equal(3, runInfo.IntList1[2]);
			}
		}

		public class InNestedCommand
		{
			[Fact]
			public void ParsedValues_SuccessfullyBind_ToListProperty()
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "command",
					Arguments =
					{
						new SequenceArgument<TestRunInfo, int>
						{
							ListProperty = ri => ri.IntList1
						}
					},
					SubCommands =
					{
						new Command<TestRunInfo>
						{
							Key = "subcommand",
							Arguments =
							{
								new SequenceArgument<TestRunInfo, int>
								{
									ListProperty = ri => ri.IntList2
								}
							}
						}
					}
				});

				var runInfo = (TestRunInfo)builder.Build(new string[] 
				{
					"command", "1", "2", "3", "subcommand", "4", "5", "6"
				});

				Assert.Equal(3, runInfo.IntList1.Count);
				Assert.Equal(1, runInfo.IntList1[0]);
				Assert.Equal(2, runInfo.IntList1[1]);
				Assert.Equal(3, runInfo.IntList1[2]);

				Assert.Equal(3, runInfo.IntList2.Count);
				Assert.Equal(4, runInfo.IntList2[0]);
				Assert.Equal(5, runInfo.IntList2[1]);
				Assert.Equal(6, runInfo.IntList2[2]);
			}
		}
	}
}
