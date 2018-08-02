using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing
{
	public class ProcessStageResultHandlingTests
	{
		[Fact]
		public void SkipsCorrectly()
		{
			var builder = new RunInfoBuilder<TestRunInfo>();

			builder.Store
				.AddCommand("command", context => new ProcessStageResult().SkipNext(1))
				.AddOption("option1", ri => ri.Bool1)
				.AddOption("option2", ri => ri.Bool2)
				.AddOption("option3", ri => ri.Bool3);

			var args = new string[] { "--option1", "command", "--option2", "--option3" };

			BuildResult<TestRunInfo> result = builder.Build(args);

			Assert.True(result.RunInfo.Bool1);
			Assert.False(result.RunInfo.Bool2);
			Assert.True(result.RunInfo.Bool3);
		}

		[Fact]
		public void Skipped_ToBeyondProgramArgumentsLength_GracefullyFinishesProcessing()
		{
			var builder = new RunInfoBuilder<TestRunInfo>();

			builder.Store
				.AddCommand("command", context => new ProcessStageResult().SkipNext(3))
				.AddOption("option1", ri => ri.Bool1)
				.AddOption("option2", ri => ri.Bool2)
				.AddOption("option3", ri => ri.Bool3);

			var args = new string[] { "--option1", "command", "--option2", "--option3" };

			BuildResult<TestRunInfo> result = builder.Build(args);

			Assert.Equal(BuildResultType.Success, result.Type);
			Assert.True(result.RunInfo.Bool1);
			Assert.False(result.RunInfo.Bool2);
			Assert.False(result.RunInfo.Bool3);
		}

		[Fact]
		public void KillingBuildProcess_CorrectlyStopsFurther_ProgramArgumentsProcessing()
		{
			var builder = new RunInfoBuilder<TestRunInfo>();

			builder.Store
				.AddCommand("command", context => new ProcessStageResult().KillBuildProcess())
				.AddCommand("command_throw", context =>
				{
					throw new Exception("This shouldn't be thrown because build is killed.");
				})
				.AddOption("option1", ri => ri.Bool1)
				.AddOption("option2", ri => ri.Bool2);

			var args = new string[] { "--option1", "command", "--option2", "command_throw" };

			BuildResult<TestRunInfo> result = builder.Build(args);

			Assert.Equal(BuildResultType.Success, result.Type);
			Assert.True(result.RunInfo.Bool1);
			Assert.False(result.RunInfo.Bool2);
		}

		[Fact]
		public void SkipFurtherArgumentProcessing_StopsProcessing_OnlyForCurrentProgramArgument()
		{
			var setup = new BuilderSetup<TestRunInfo>();

			var seen = new HashSet<string>();

			setup.Hooks.EnablePostArgumentProcessing(context =>
			{
				seen.Add(context.ProgramArguments[context.Position].ArgumentToken);
				return new ProcessStageResult();
			});

			RunInfoBuilder<TestRunInfo> builder = setup.Create();

			builder.Store
				.AddCommand("command1", context => new ProcessStageResult())
				.AddCommand("command2", context => new ProcessStageResult().StopProcessingCurrentArgument())
				.AddCommand("command3", context => new ProcessStageResult());

			var args = new string[] { "command1", "command2", "command3" };

			builder.Build(args);

			Assert.Contains("command1", seen);
			Assert.DoesNotContain("command2", seen);
			Assert.Contains("command3", seen);
		}
	}
}
