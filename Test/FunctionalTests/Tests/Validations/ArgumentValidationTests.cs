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

		public class CustomArgumentTests
		{
			[Theory]
			[InlineData(-1)]
			[InlineData(0)]
			public void InvalidCount_Throws(int count)
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
								Count = count,
								HelpToken = "helptoken"
							}
						}
					});
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.InvalidCount, validationException.ErrorType);
				Assert.Equal(0, validationException.CommandLevel);
			}

			[Fact]
			public void Null_CustomHandler_Throws()
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
								Count = 1,
								Handler = null,
								HelpToken = "helptoken"
							}
						}
					});
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.NullCustomHandler, validationException.ErrorType);
				Assert.Equal(0, validationException.CommandLevel);
			}

			[Theory]
			[InlineData("")]
			[InlineData(null)]
			public void Null_HelpToken_Throws(string helpToken)
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
								Count = 1,
								Handler = context => ProcessResult.Continue,
								HelpToken = helpToken
							}
						}
					});
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.NullHelpToken, validationException.ErrorType);
				Assert.Equal(0, validationException.CommandLevel);
			}
		}

		public class SetArgumentTests
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
							new SetArgument<TestRunInfo, bool>
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
			public void PropertyNotWritable_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Arguments =
						{
							new SetArgument<TestRunInfo, bool>
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

			[Fact]
			public void NullValues_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Arguments =
						{
							new SetArgument<TestRunInfo, bool>
							{
								Property = ri => ri.Bool1,
								Values = null
							}
						}
					});
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.NullObject, validationException.ErrorType);
				Assert.Equal(0, validationException.CommandLevel);
			}

			[Fact]
			public void ValuesContains_LessThanTwoValues_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Arguments =
						{
							new SetArgument<TestRunInfo, bool>
							{
								Property = ri => ri.Bool1,
								Values = new List<(string, bool)>
								{
									("true", true)
								}
							}
						}
					});
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.InsufficientCount, validationException.ErrorType);
				Assert.Equal(0, validationException.CommandLevel);
			}

			[Fact]
			public void Values_HasDuplicateLabels_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Arguments =
						{
							new SetArgument<TestRunInfo, bool>
							{
								Property = ri => ri.Bool1,
								Values = new List<(string, bool)>
								{
									("true", true),
									("true", false)
								}
							}
						}
					});
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.DuplicateKey, validationException.ErrorType);
				Assert.Equal(0, validationException.CommandLevel);
			}

			[Fact]
			public void Values_HasDuplicateValues_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Arguments =
						{
							new SetArgument<TestRunInfo, bool>
							{
								Property = ri => ri.Bool1,
								Values = new List<(string, bool)>
								{
									("true", true),
									("false", true)
								}
							}
						}
					});
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.DuplicateKey, validationException.ErrorType);
				Assert.Equal(0, validationException.CommandLevel);
			}
		}
	}
}
