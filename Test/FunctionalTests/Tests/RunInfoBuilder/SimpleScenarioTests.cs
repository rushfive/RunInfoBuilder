using R5.RunInfoBuilder;
using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.FunctionalTests.Models;
using System.Collections.Generic;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.RunInfoBuilder
{
    public class SimpleScenarioTests
    {
		public class VersionTests
		{
			[Fact]
			public void VersionTrigger_AsSingleArgument_Invokes()
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
	}
}
