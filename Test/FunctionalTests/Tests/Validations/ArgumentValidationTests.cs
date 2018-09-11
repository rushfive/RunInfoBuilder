using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Validations
{
	public class ArgumentValidationTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		public class PropertyArgumentTests
		{
			[Fact]
			public void NullPropertyExpression_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Arguments =
						{
							new PropertyArgument<TestRunInfo, bool>
							{
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
						Key = "key",
						Arguments =
						{
							new PropertyArgument<TestRunInfo, bool>
							{
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
		}

		public class SequenceArgumentTests
		{
			[Fact]
			public void NullPropertyExpression_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Arguments =
						{
							new SequenceArgument<TestRunInfo, string>
							{
								ListProperty = null
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
						Key = "key",
						Arguments =
						{
							new SequenceArgument<TestRunInfo, string>
							{
								ListProperty = ri => ri.UnwritableStringList
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
		}
	}
}
