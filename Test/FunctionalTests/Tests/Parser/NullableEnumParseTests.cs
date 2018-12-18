using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Parser
{
	public class NullableEnumParseTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			var builder = new RunInfoBuilder();

			builder.Commands.AddDefault(new DefaultCommand<TestRunInfo>
			{
				Options =
				{
					new Option<TestRunInfo, TestEnum?>
					{
						Key = "enum",
						Property = ri => ri.NullableEnum
					}
				}
			});

			return builder;
		}

		[Fact]
		public void EmptyStringArgument_SetsAsNull()
		{
			RunInfoBuilder builder = GetBuilder();

			var result = (TestRunInfo)builder.Build(new string[] { "--enum", "" });

			Assert.Null(result.NullableEnum);
		}

		[Fact]
		public void ValidEnumArgument_SetsCorrectValue()
		{
			RunInfoBuilder builder = GetBuilder();

			var result = (TestRunInfo)builder.Build(new string[] { "--enum", "ValueB" });

			Assert.Equal(TestEnum.ValueB, result.NullableEnum);
		}

		[Fact]
		public void InvalidEnumArgument_Failed()
		{
			Action testCode = () =>
			{
				RunInfoBuilder builder = GetBuilder();

				var result = (TestRunInfo)builder.Build(new string[] { "--enum", "InvalidEnumValue" });
			};

			Exception exception = Record.Exception(testCode);

			var processException = exception as ProcessException;

			Assert.NotNull(processException);
			Assert.Equal(ProcessError.ParserInvalidValue, processException.ErrorType);
			Assert.Equal(0, processException.CommandLevel);
		}
	}
}
