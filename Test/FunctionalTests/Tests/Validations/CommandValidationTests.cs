﻿using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Validations
{
	public class CommandValidationTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void NullOrEmptyKey_Throws(string key)
		{
			Action testCode = () =>
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = key
				});
			};

			Exception exception = Record.Exception(testCode);

			var validationException = exception as CommandValidationException;

			Assert.NotNull(validationException);
			Assert.Equal(CommandValidationError.KeyNotProvided, validationException.ErrorType);
			Assert.Equal(0, validationException.CommandLevel);
		}

		public class ArgumentsTests
		{
			[Fact]
			public void HasNullArguments_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Arguments =
						{
							null
						}
					});
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.NullObject, validationException.ErrorType);
				Assert.Equal(0, validationException.CommandLevel);

				bool correctIndex = validationException.Metadata.Single() is int nullIndex
					&& nullIndex == 0;

				Assert.True(correctIndex);
			}
		}

		public class OptionsTests
		{
			[Fact]
			public void HasNullOptions_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Options =
						{
							null
						}
					});
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.NullObject, validationException.ErrorType);
				Assert.Equal(0, validationException.CommandLevel);

				bool correctIndex = validationException.Metadata.Single() is int nullIndex
					&& nullIndex == 0;

				Assert.True(correctIndex);
			}
			
			[Theory]
			[InlineData("")]
			[InlineData("|")]
			[InlineData("*")]
			[InlineData("|x")]
			[InlineData("xxx|")]
			[InlineData("|xxx|x")]
			[InlineData("xxx|x|")]
			[InlineData("valid", "xxx|x|")]
			[InlineData("xxx|x|", "valid")]
			public void Any_InvalidKey_Throws(params string[] keys)
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					var options = new List<OptionBase<TestRunInfo>>();

					foreach (string key in keys)
					{
						options.Add(new Option<TestRunInfo, bool>
						{
							Key = key,
							Property = ri => ri.Bool1
						});
					}

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options = options
					});
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.InvalidKey, validationException.ErrorType);
				Assert.Equal(0, validationException.CommandLevel);
			}

			[Theory]
			[InlineData("duplicate", "duplicate")]
			[InlineData("duplicate | 1", "duplicate | 2")]
			[InlineData("unique", "duplicate", "duplicate")]
			[InlineData("duplicate", "duplicate", "unique")]
			[InlineData("unique1", "duplicate", "duplicate", "unique2")]
			public void Duplicate_FullKeys_Throws(params string[] keys)
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					var options = new List<OptionBase<TestRunInfo>>();

					foreach (string key in keys)
					{
						options.Add(new Option<TestRunInfo, bool>
						{
							Key = key,
							Property = ri => ri.Bool1
						});
					}

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Options = options
					});
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.DuplicateKey, validationException.ErrorType);
				Assert.Equal(0, validationException.CommandLevel);
			}

			[Theory]
			[InlineData("unique1 | 1", "unique2 | 1")]
			[InlineData("unique1 | 1", "unique2 | 2", "unique3 | 2")]
			[InlineData("unique1 | 1", "unique2 | 1", "unique3 | 2")]
			public void Duplicate_ShortKeys_Throws(params string[] keys)
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					var options = new List<OptionBase<TestRunInfo>>();

					foreach (string key in keys)
					{
						options.Add(new Option<TestRunInfo, bool>
						{
							Key = key,
							Property = ri => ri.Bool1
						});
					}

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Options = options
					});
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.DuplicateKey, validationException.ErrorType);
				Assert.Equal(0, validationException.CommandLevel);
			}
		}

		public class SubCommandsTests
		{
			[Fact]
			public void HasNullSubCommands_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						SubCommands =
						{
							null
						}
					});
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.NullObject, validationException.ErrorType);
				Assert.Equal(0, validationException.CommandLevel);

				bool correctIndex = validationException.Metadata.Single() is int nullIndex
					&& nullIndex == 0;

				Assert.True(correctIndex);
			}

			[Fact]
			public void DuplicateKeys_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						SubCommands =
						{
							new SubCommand<TestRunInfo>
							{
								Key = "duplicate"
							},
							new SubCommand<TestRunInfo>
							{
								Key = "duplicate"
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

		public class NestedSubCommandTests
		{
			[Fact]
			public void LowerLevel_NestedCommands_AreValidated()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						SubCommands =
						{
							new SubCommand<TestRunInfo>
							{
								Key = null
							}
						}
					});
				};

				Exception exception = Record.Exception(testCode);

				var validationException = exception as CommandValidationException;

				Assert.NotNull(validationException);
				Assert.Equal(CommandValidationError.KeyNotProvided, validationException.ErrorType);
				Assert.Equal(1, validationException.CommandLevel);
			}
		}
	}
}
