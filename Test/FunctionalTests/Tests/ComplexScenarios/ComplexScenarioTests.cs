using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.ComplexScenarios
{
	// TODO: better name
	public class ComplexScenarioTests
	{
		private static RunInfoBuilder GetBuilder() => ComplexScenarioBuilderFactory.GetBuilder();

		public class SingleLevel
		{


			[Fact]
			public void IDontKnowWhatToCallThis_But_EverythingWorks()
			{
				RunInfoBuilder builder = GetBuilder();

				var args = new string[]
				{
					"CustomToSequence", "custom1", "custom2", "1", "2", "3",
					"-s=hello", "--int", "101", "-ab",
					
				};

				TestRunInfo result = (TestRunInfo)builder.Build(args);

				Assert.Equal("hello", result.String1);
				Assert.Equal(101, result.Int1);
				Assert.True(result.Bool1);
				Assert.True(result.Bool2);
				Assert.True(result.BoolFromCustomArg1);
				Assert.Equal(3, result.IntList1.Count);
				Assert.Equal(1, result.IntList1[0]);
				Assert.Equal(2, result.IntList1[1]);
				Assert.Equal(3, result.IntList1[2]);
			}
		}
	}
}
