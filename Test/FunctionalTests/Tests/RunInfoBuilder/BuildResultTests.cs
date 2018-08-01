using R5.RunInfoBuilder;
using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.RunInfoBuilder
{
	// Tests whether the correct BuildResult types are returned
	// given the right situations
    public class BuildResultTests
    {
		public class NotProcessed
		{
			[Fact]
			public void ReturnsCorrectResult()
			{
				var builder = new RunInfoBuilder<TestRunInfo>();

				BuildResult<TestRunInfo> nullResult = builder.Build(null);
				BuildResult<TestRunInfo> emptyResult = builder.Build(new string[] { });

				validateResult(nullResult);
				validateResult(emptyResult);

				void validateResult(BuildResult<TestRunInfo> result)
				{
					Assert.Equal(BuildResultType.NotProcessed, result.Type);
					Assert.Null(result.RunInfo);
					Assert.Null(result.FailMessage);
					Assert.Null(result.Exception);
					Assert.Null(result.ProgramArgumentErrors);
				}
			}
		}

		public class Version
		{
			[Fact]
			public void ReturnsCorrectResult()
			{
				var setup = new BuilderSetup<TestRunInfo>();

				int counter = 0;

				setup.ConfigureVersion(config =>
				{
					config
						.SetTriggers("-version", "-v")
						.IgnoreCase()
						.SetCallback(() => { counter++; });
				});

				RunInfoBuilder<TestRunInfo> builder = setup.Create();

				var triggers = new List<string> { "-version", "-vErSioN", "-v" };
				for (int i = 0; i < 3; i++)
				{
					string trigger = triggers[i];

					BuildResult<TestRunInfo> result = builder.Build(new string[] { trigger });

					Assert.Equal(BuildResultType.Version, result.Type);
					Assert.Equal(i + 1, counter);
				}
			}
		}

		public class Help
		{
			[Fact]
			public void ReturnsCorrectResult()
			{
				var setup = new BuilderSetup<TestRunInfo>();

				int counter = 0;

				setup.ConfigureHelp(config =>
				{
					config
						.SetTriggers("-help", "-h")
						.IgnoreCase()
						.SetCallback(context => { counter++; });
				});

				RunInfoBuilder<TestRunInfo> builder = setup.Create();

				var triggers = new List<string> { "-help", "-hELp", "-h" };
				for (int i = 0; i < 3; i++)
				{
					string trigger = triggers[i];

					BuildResult<TestRunInfo> result = builder.Build(new string[] { trigger });

					Assert.Equal(BuildResultType.Help, result.Type);
					Assert.Equal(i + 1, counter);
				}
			}
		}

		public class Success
		{
			[Fact]
			public void ReturnsCorrectResult()
			{
				var builder = new RunInfoBuilder<TestRunInfo>();

				builder.Store.AddOption("option", ri => ri.Bool1);

				BuildResult<TestRunInfo> result = builder.Build(new string[] { "--option" });

				Assert.Equal(BuildResultType.Success, result.Type);
				Assert.True(result.RunInfo.Bool1);
			}
		}

		public class Fail
		{
			[Fact]
			public void ReturnsCorrectResult()
			{
				var setup = new BuilderSetup<TestRunInfo>();

				setup.AlwaysReturnBuildResult();

				setup.Hooks.EnablePreArgumentProcessing(context => throw new Exception());

				RunInfoBuilder<TestRunInfo> builder = setup.Create();

				builder.Store.AddOption("option", ri => ri.Bool1);

				BuildResult<TestRunInfo> result = builder.Build(new string[] { "--option" });

				Assert.Equal(BuildResultType.Fail, result.Type);
			}
		}
	}
}
