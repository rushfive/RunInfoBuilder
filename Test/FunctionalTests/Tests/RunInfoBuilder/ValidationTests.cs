using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.RunInfoBuilder
{
	public class ValidationTests
	{
		public class RunInfoPrimitives
		{
			public bool Bool { get; set; }
			public string String { get; set; }
			public DateTime DateTime { get; set; }
		}

		public class BuilderConfiguration_ValidationTests
		{
			[Fact]
			public void Parser_DoesntHandle_Any_RequiredTypes_Throws()
			{
				var builder = new RunInfoBuilder<RunInfoPrimitives>();

				builder.Store
					.AddArgument("bool", ri => ri.Bool)
					.AddArgument("string", ri => ri.String)
					.AddArgument("datetime", ri => ri.DateTime);
				
				Assert.Throws<BuilderConfigurationValidationException>(
					() => builder.Build(new string[] { "bool", "string", "datetime" }));
			}

			[Fact]
			public void Parser_DoesntHandle_All_RequiredTypes_Throws()
			{
				var builder = new RunInfoBuilder<RunInfoPrimitives>();

				builder.Store
					.AddArgument("bool", ri => ri.Bool)
					.AddArgument("string", ri => ri.String)
					.AddArgument("datetime", ri => ri.DateTime);

				builder.Parser
					.SetPredicateForType<bool>(value => (true, default))
					.SetPredicateForType<DateTime>(value => (true, default));

				Assert.Throws<BuilderConfigurationValidationException>(
					() => builder.Build(new string[] { "bool", "string", "datetime" }));
			}
		}

		public class ProgramArgument_ValidationTests
		{
			[Fact]
			public void DuplicateArguments_Throws()
			{
				var builder = new RunInfoBuilder<TestRunInfo>();

				Assert.Throws<ProgramArgumentsValidationException>(
					() => builder.Build(new string[] { "a", "b", "a" }));
			}

			[Fact]
			public void HelpArgument_NotFirst_Throws()
			{
				var setup = new BuilderSetup<TestRunInfo>();

				setup.ConfigureHelp(config =>
				{
					config
						.SetTriggers("-help")
						.SetCallback(context => { });
				});

				RunInfoBuilder<TestRunInfo> builder = setup.Create();

				Assert.Throws<ProgramArgumentsValidationException>(
					() => builder.Build(new string[] { "a", "b", "-help" }));
			}

			[Fact]
			public void VersionArgument_NotFirst_Throws()
			{
				var setup = new BuilderSetup<TestRunInfo>();

				setup.ConfigureVersion(config =>
				{
					config
						.SetTriggers("-version")
						.SetCallback(() => { });
				});

				RunInfoBuilder<TestRunInfo> builder = setup.Create();

				Assert.Throws<ProgramArgumentsValidationException>(
					() => builder.Build(new string[] { "a", "b", "-version" }));
			}
		}
	}
}
