using R5.RunInfoBuilder.UnitTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.UnitTests.Tests.Builder.Models
{
	public class BuildResult_StaticFactory_Tests
	{
		public class Success_Method
		{
			[Fact]
			public void ResolvesProperties()
			{
				var runInfo = new TestRunInfo();

				BuildResult<TestRunInfo> result = BuildResult<TestRunInfo>.Success(runInfo);

				Assert.Same(runInfo, result.RunInfo);
				Assert.Equal(BuildResultType.Success, result.Type);

				AssertOptionalPropertiesHaveDefaultValues(result);
			}
		}
		
		public class Fail
		{
			[Fact]
			public void ResolvesProperties()
			{
				string failMessage = "fail";
				var exception = new Exception();

				BuildResult<TestRunInfo> result = BuildResult<TestRunInfo>.Fail(failMessage, exception);

				Assert.Null(result.RunInfo);
				Assert.Equal(BuildResultType.Fail, result.Type);
				Assert.Equal(failMessage, result.FailMessage);
				Assert.Same(exception, result.Exception);
				Assert.Null(result.ProgramArgumentErrors);
			}
		}

		public class Help
		{
			[Fact]
			public void ResolvesProperties()
			{
				BuildResult<TestRunInfo> result = BuildResult<TestRunInfo>.Help();

				Assert.Null(result.RunInfo);
				Assert.Equal(BuildResultType.Help, result.Type);
				AssertOptionalPropertiesHaveDefaultValues(result);
			}
		}

		public class NotProcessed
		{
			[Fact]
			public void ResolvesProperties()
			{
				BuildResult<TestRunInfo> result = BuildResult<TestRunInfo>.NotProcessed();

				Assert.Null(result.RunInfo);
				Assert.Equal(BuildResultType.NotProcessed, result.Type);
				AssertOptionalPropertiesHaveDefaultValues(result);
			}
		}

		public class Version
		{
			[Fact]
			public void ResolvesProperties()
			{
				BuildResult<TestRunInfo> result = BuildResult<TestRunInfo>.Version();

				Assert.Null(result.RunInfo);
				Assert.Equal(BuildResultType.Version, result.Type);
				AssertOptionalPropertiesHaveDefaultValues(result);
			}
		}


		private static void AssertOptionalPropertiesHaveDefaultValues<T>(BuildResult<T> result)
			where T : class
		{
			Assert.Null(result.FailMessage);
			Assert.Null(result.Exception);
			Assert.Null(result.ProgramArgumentErrors);
		}
	}
}
