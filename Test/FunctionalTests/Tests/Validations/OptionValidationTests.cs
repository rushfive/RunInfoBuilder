using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Validations
{
	public class OptionValidationTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		[Fact]
		public void NullPropertyExpression_Throws()
		{
			Action testCode = () =>
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "command",
					Options =
					{
						new Option<TestRunInfo, bool>
						{
							Key = "option",
							Property = null
						}
					}
				});
			};

			Exception exception = Record.Exception(testCode);

			var validationException = exception as CommandValidationException;

			Assert.NotNull(validationException);
			Assert.Equal(CommandValidationError.NullPropertyExpression, validationException.ErrorType);
			Assert.Equal(0, validationException.CommandLevel);
		}

		[Fact]
		public void Property_NotWritable_Throws()
		{
			Action testCode = () =>
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "command",
					Options =
					{
						new Option<TestRunInfo, bool>
						{
							Key = "option",
							Property = ri => ri.UnwritableBool
						}
					}
				});
			};

			Exception exception = Record.Exception(testCode);

			var validationException = exception as CommandValidationException;

			Assert.NotNull(validationException);
			Assert.Equal(CommandValidationError.PropertyNotWritable, validationException.ErrorType);
			Assert.Equal(0, validationException.CommandLevel);
		}

		[Fact]
		public void OnProcess_NotAllowed_ForBoolOptions()
		{
			Action testCode = () =>
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "command",
					Options =
					{
						new Option<TestRunInfo, bool>
						{
							Key = "option",
							Property = ri => ri.Bool1,
							OnProcess = arg => ProcessResult.Continue
						}
					}
				});
			};

			Exception exception = Record.Exception(testCode);

			var validationException = exception as CommandValidationException;

			Assert.NotNull(validationException);
			Assert.Equal(CommandValidationError.CallbackNotAllowed, validationException.ErrorType);
			Assert.Equal(0, validationException.CommandLevel);
		}
	}
}
