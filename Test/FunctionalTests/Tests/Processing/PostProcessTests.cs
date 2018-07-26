using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.FunctionalTests.Models;
using R5.RunInfoBuilder.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing
{
    public class PostProcessTests
    {
		[Fact]
		public void Callback_Invoked_WhenConfigured()
		{
			var setup = new BuilderSetup<TestRunInfo>();

			bool invoked = false;

			setup.Process.Hooks.EnablePostProcessing(context => {
				invoked = true;
			});

			RunInfoBuilder<TestRunInfo> builder = setup.Create();

			builder.Store.AddOption("option", ri => ri.Bool1);

			builder.Build(new string[] { "--option" });

			Assert.True(invoked);
		}

		[Fact]
		public void CallbackContext_ContainsExpectedProperties()
		{
			var setup = new BuilderSetup<TestRunInfo>();

			var args = new string[] { "command", "--option" };

			setup.Process.Hooks.EnablePostProcessing((PostProcessContext<TestRunInfo> context) => {
				Assert.NotNull(context.RunInfo);
				Assert.Equal(2, context.ProgramArguments.Length);
				Assert.Equal("command", context.ProgramArguments[0]);
				Assert.Equal("--option", context.ProgramArguments[1]);
				Assert.True(context.KilledBuildProcess);
				// todo: reflection helper for count
			});

			RunInfoBuilder<TestRunInfo> builder = setup.Create();

			builder.Store
				.AddCommand("command", context => new ProcessStageResult().KillBuildProcess())
				.AddOption("option", ri => ri.Bool1);

			BuildResult<TestRunInfo> result = builder.Build(args);
			Assert.False(result.RunInfo.Bool1);
		}

		[Fact]
		public void RunInfo_CanBeModified_InCallback()
		{
			var runInfo = new TestRunInfo();

			var setup = new BuilderSetup<TestRunInfo>();

			setup.UseImplementation(runInfo);

			setup.Process.Hooks.EnablePostProcessing(context => {
				context.RunInfo.Bool3 = true;
				Assert.Same(runInfo, context.RunInfo);
			});

			RunInfoBuilder<TestRunInfo> builder = setup.Create();

			builder.Store.AddOption("option", ri => ri.Bool1);

			Assert.False(runInfo.Bool3);

			builder.Build(new string[] { "--option" });

			Assert.True(runInfo.Bool3);
		}

		[Fact]
		public void RunInfo_InCallbackContext_SameAsInBuildResult()
		{
			var setup = new BuilderSetup<TestRunInfo>();

			TestRunInfo referenced = null;
			setup.Process.Hooks.EnablePostProcessing(context => {
				referenced = context.RunInfo;
			});

			RunInfoBuilder<TestRunInfo> builder = setup.Create();

			builder.Store.AddOption("option", ri => ri.Bool1);

			BuildResult<TestRunInfo> result = builder.Build(new string[] { "--option" });

			Assert.NotNull(referenced);
			Assert.Same(referenced, result.RunInfo);
		}

		[Fact]
		public void ProgramArgumentsProperty_InCallbackContext_IsCopy()
		{
			var args = new string[] { "--option1", "--option2" };
			var runInfo = new TestRunInfo();

			var setup = new BuilderSetup<TestRunInfo>();

			setup.UseImplementation(runInfo);

			setup.Process.Hooks.EnablePostProcessing(context => {
				Assert.NotSame(args, context.ProgramArguments);
				context.ProgramArguments[1] = "invalid";
			});

			RunInfoBuilder<TestRunInfo> builder = setup.Create();

			builder.Store
				.AddOption("option1", ri => ri.Bool1)
				.AddOption("option2", ri => ri.Bool2);

			Assert.False(runInfo.Bool1);
			Assert.False(runInfo.Bool2);

			BuildResult<TestRunInfo> result = builder.Build(args);

			Assert.True(runInfo.Bool1);
			Assert.True(runInfo.Bool2);
		}

		[Fact]
		public void Occurs_After_ProgramArgumentProcessing()
		{
			var runInfo = new TestRunInfo();

			var setup = new BuilderSetup<TestRunInfo>();

			setup
				.UseImplementation(runInfo)
				.AlwaysReturnBuildResult();

			long? commandSet = null;
			long? postProcessSet = null;

			setup.Process.Hooks.EnablePostProcessing(context => postProcessSet = DateTime.UtcNow.Ticks);

			RunInfoBuilder<TestRunInfo> builder = setup.Create();

			builder.Store.AddCommand("command", context =>
			{
				commandSet = DateTime.UtcNow.Ticks;
				return new ProcessStageResult();
			});

			builder.Build(new string[] { "command" });

			Assert.NotNull(commandSet);
			Assert.NotNull(postProcessSet);
			Assert.True(postProcessSet.Value > commandSet.Value);
		}
	}
}
