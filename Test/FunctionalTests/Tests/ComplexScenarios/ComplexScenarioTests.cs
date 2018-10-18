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
			public void FirstTest_BindsCorrectly()
			{
				RunInfoBuilder builder = GetBuilder();

				var args = new string[]
				{
					"CustomToSequence", "custom1", "custom2", "1", "2", "3", "-s=hello", "--int", "101", "-ab"
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

		public class MultiLevel
		{
			[Fact]
			public void StringSubCommand_BindsCorrectly()
			{
				RunInfoBuilder builder = GetBuilder();

				var args = new string[]
				{
					"ComplexCommand", "one", "33", "5", "6", "-s", "short_string", "--int=100", "--bool1",
					"StringSubCommand", "short_string_from_sub", "two", "first", "last", "--bool2"
				};

				TestRunInfo result = (TestRunInfo)builder.Build(args);

				//Assert.Equal(2, result.Int1);
				Assert.Equal(33, result.Int2);
				Assert.Equal(11, result.Int3);
				//Assert.Equal("short_string", result.String1);
				Assert.Equal(100, result.Int1); // should override the fisrt arg "one"
				Assert.True(result.Bool1);
				Assert.Equal("short_string_from_sub", result.String1); // overrides top level command
				Assert.Equal("two", result.String2);
				Assert.Equal("firstlast", result.String3);
				Assert.True(result.Bool2);
			}

			[Fact]
			public void DoubleSubCommand_BindsCorrectly()
			{
				RunInfoBuilder builder = GetBuilder();

				var args = new string[]
				{
					"ComplexCommand", "one", "33", "5", "6", "-s", "short_string", "--int=100", "--bool1",
					"DoubleSubCommand", "3.7", "10.1", "10.2", "10.3", "9", "8", "10.5"
				};

				TestRunInfo result = (TestRunInfo)builder.Build(args);

				//Assert.Equal(2, result.Int1);
				Assert.Equal(33, result.Int2);
				Assert.Equal(11, result.Int3);
				//Assert.Equal("short_string", result.String1);
				Assert.Equal(100, result.Int1); // should override the fisrt arg "one"
				Assert.True(result.Bool1);

				Assert.Equal(3.7, result.Double1);
				Assert.Equal(3, result.DoubleList1.Count);
				Assert.Equal(10.1, result.DoubleList1[0]);
				Assert.Equal(10.2, result.DoubleList1[1]);
				Assert.Equal(10.3, result.DoubleList1[2]);
			}
		}
	}
}
