using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing.CustomArgument
{
	public class CustomArgumentSuccessTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		[Fact]
		public void CustomHandlerContext_ContainsCorrect_ProgramArguments()
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
					new CustomArgument<TestRunInfo>
					{
						Count = 3,
						Handler = context =>
						{
							Assert.Equal(3, context.HandledProgramArguments.Count);
							Assert.Equal("1", context.HandledProgramArguments[0]);
							Assert.Equal("2", context.HandledProgramArguments[1]);
							Assert.Equal("3", context.HandledProgramArguments[2]);
							context.RunInfo.Int1 = 10;
							return ProcessResult.Continue;
						}
					},
					new PropertyArgument<TestRunInfo, bool>
					{
						Property = ri => ri.Bool2
					}
				}
			});

			var runInfo = builder.Build(new string[] { "command", "true", "1", "2", "3", "true" });

			var testRunInfo = runInfo as TestRunInfo;
			Assert.NotNull(testRunInfo);

			Assert.True(testRunInfo.Bool1);
			Assert.True(testRunInfo.Bool2);
			Assert.Equal(10, testRunInfo.Int1);
		}

		[Fact]
		public void CustomHandler_ReturnsEndResult_StopsFurtherProcessing()
		{
			RunInfoBuilder builder = GetBuilder();

			builder.Commands.Add(new Command<TestRunInfo>
			{
				Key = "command",
				Arguments =
				{
					new CustomArgument<TestRunInfo>
					{
						Count = 2,
						Handler = context => ProcessResult.End
					},
					new PropertyArgument<TestRunInfo, bool>
					{
						Property = ri => ri.Bool3
					},
					new CustomArgument<TestRunInfo>
					{
						Count = 1,
						Handler = context => throw new Exception()
					}
				}
			});

			var runInfo = builder.Build(new string[] { "command", "1", "1", "true" });

			var testRunInfo = runInfo as TestRunInfo;
			Assert.NotNull(testRunInfo);

			Assert.False(testRunInfo.Bool3);
		}

		[Fact]
		public void TwoSequential_CustomArguments_ProcessesCorrectly()
		{
			RunInfoBuilder builder = GetBuilder();

			builder.Commands.Add(new Command<TestRunInfo>
			{
				Key = "command",
				Arguments =
				{
					new CustomArgument<TestRunInfo>
					{
						Count = 2,
						Handler = context =>
						{
							Assert.Equal(2, context.HandledProgramArguments.Count);
							Assert.Equal("1", context.HandledProgramArguments[0]);
							Assert.Equal("2", context.HandledProgramArguments[1]);
							context.RunInfo.Int1 = 10;
							return ProcessResult.Continue;
						}
					},
					new CustomArgument<TestRunInfo>
					{
						Count = 3,
						Handler = context =>
						{
							Assert.Equal(3, context.HandledProgramArguments.Count);
							Assert.Equal("3", context.HandledProgramArguments[0]);
							Assert.Equal("4", context.HandledProgramArguments[1]);
							Assert.Equal("5", context.HandledProgramArguments[2]);
							context.RunInfo.Int2 = 20;
							return ProcessResult.Continue;
						}
					}
				}
			});

			var runInfo = builder.Build(new string[] { "command", "1", "2", "3", "4", "5" });

			var testRunInfo = runInfo as TestRunInfo;
			Assert.NotNull(testRunInfo);

			Assert.Equal(10, testRunInfo.Int1);
			Assert.Equal(20, testRunInfo.Int2);
		}
	}
}
