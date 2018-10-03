using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing.CustomArgument
{
	public class CustomArgumentFailTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		[Fact]
		public void NotEnoughProgramArguments_ForHandlerContext_Throws()
		{
			Action testCode = () =>
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
							Handler = context => ProcessResult.Continue
						}
					}
				});

				builder.Build(new string[] { "command", "one" });
			};

			Exception exception = Record.Exception(testCode);

			var processException = exception as ProcessException;

			Assert.NotNull(processException);
			Assert.Equal(ProcessError.ExpectedProgramArgument, processException.ErrorType);
			Assert.Equal(0, processException.CommandLevel);
		}
	}
}
