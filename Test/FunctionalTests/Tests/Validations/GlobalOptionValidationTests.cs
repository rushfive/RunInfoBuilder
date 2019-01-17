﻿using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Validations
{
	public class GlobalOptionValidationTests
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
					GlobalOptions =
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
					GlobalOptions =
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
					GlobalOptions =
					{
						new Option<TestRunInfo, bool>
						{
							Key = "option",
							Property = ri => ri.Bool1,
							OnParsed = arg => ProcessResult.Continue
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

		[Fact]
		public void DuplicateFullKeys_WithinGlobalOptions_Throws()
		{
			Action testCode = () =>
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "command",
					GlobalOptions =
					{
						new Option<TestRunInfo, bool>
						{
							Key = "option",
							Property = ri => ri.Bool1
						},
						new Option<TestRunInfo, bool>
						{
							Key = "option",
							Property = ri => ri.Bool1
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
		public void DuplicateShortKeys_WithinGlobalOptions_Throws()
		{
			Action testCode = () =>
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "command",
					GlobalOptions =
					{
						new Option<TestRunInfo, bool>
						{
							Key = "option1 | a",
							Property = ri => ri.Bool1
						},
						new Option<TestRunInfo, bool>
						{
							Key = "option2 | a",
							Property = ri => ri.Bool1
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
		public void DuplicateFullKeys_GlobalVsSubcommand_Throws()
		{
			Action testCode = () =>
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "command",
					GlobalOptions =
					{
						new Option<TestRunInfo, bool>
						{
							Key = "option",
							Property = ri => ri.Bool1
						}
					},
					SubCommands =
					{
						new SubCommand<TestRunInfo>
						{
							Key = "subcommand",
							Options =
							{
								new Option<TestRunInfo, bool>
								{
									Key = "option",
									Property = ri => ri.Bool1
								}
							}
						}
					}
				});
			};

			Exception exception = Record.Exception(testCode);

			var validationException = exception as CommandValidationException;

			Assert.NotNull(validationException);
			Assert.Equal(CommandValidationError.DuplicateKey, validationException.ErrorType);
			Assert.Equal(1, validationException.CommandLevel);
		}

		[Fact]
		public void DuplicateShortKeys_GlobalVsSubcommand_Throws()
		{
			Action testCode = () =>
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "command",
					GlobalOptions =
					{
						new Option<TestRunInfo, bool>
						{
							Key = "option1 | a",
							Property = ri => ri.Bool1
						}
					},
					SubCommands =
					{
						new SubCommand<TestRunInfo>
						{
							Key = "subcommand",
							Options =
							{
								new Option<TestRunInfo, bool>
								{
									Key = "option2 | a",
									Property = ri => ri.Bool1
								}
							}
						}
					}
				});
			};

			Exception exception = Record.Exception(testCode);

			var validationException = exception as CommandValidationException;

			Assert.NotNull(validationException);
			Assert.Equal(CommandValidationError.DuplicateKey, validationException.ErrorType);
			Assert.Equal(1, validationException.CommandLevel);
		}
	}
}
