using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing.GlobalOption
{
	public class FailTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		public class InSingleCommand
		{
			[Theory]
			[InlineData("invalid")]
			[InlineData("invalid=invalid=")]
			[InlineData("invalid=")]
			[InlineData("--=invalid")]
			[InlineData("-=i")]
			[InlineData("-ii")]
			// This tests that an invalid option program argument simply skips
			// the option stage and hits the invalid program argument stage.
			public void InvalidOption_TokenizeFail_Throws(string option)
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri=> ri.Int1
							}
						}
					});

					builder.Build(new string[] { "command", option });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.InvalidProgramArgument, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
			}


			[Theory]
			[InlineData("-ib")]
			[InlineData("-bi")]
			[InlineData("-cib")]
			[InlineData("-bci")]
			public void StackedOption_MappedToNonBoolProperty_Throws_1(string option)
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri=> ri.Int1
							},
							new Option<TestRunInfo, bool>
							{
								Key = "bool1 | b",
								Property = ri => ri.Bool1
							},
							new Option<TestRunInfo, bool>
							{
								Key = "bool2 | c",
								Property = ri => ri.Bool2
							}
						}
					});

					builder.Build(new string[] { "command", option });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.InvalidStackedOption, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
			}

			[Fact]
			public void NonBoolOption_MissingValueProgramArgument_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri=> ri.Int1
							}
						}
					});

					builder.Build(new string[] { "command", "--int" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ExpectedProgramArgument, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
			}

			[Theory]
			[InlineData("--int1", ProcessError.ExpectedValueFoundOption)]
			[InlineData("subcommand", ProcessError.ExpectedValueFoundSubCommand)]
			public void Expected_OptionArgumentValue_AsNext_ButOptionOrSubCommand_Throws(
				string next, ProcessError expectedErrorType)
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri=> ri.Int1
							},
							new Option<TestRunInfo, int>
							{
								Key = "int1 | j",
								Property = ri=> ri.Int2
							}
						},
						SubCommands =
						{
							new SubCommand<TestRunInfo>
							{
								Key = "subcommand"
							}
						}
					});

					builder.Build(new string[] { "command", "--int", next });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(expectedErrorType, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
			}

			[Theory]
			[InlineData("--bool1", "invalid")]
			[InlineData("-bc", "invalid")]
			public void InvalidValue_ForBoolOption_NotParseable_Throws(
				string option, string invalidBoolValue)
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
								Key = "bool1 | b",
								Property = ri=> ri.Bool1
							},
							new Option<TestRunInfo, bool>
							{
								Key = "bool2 | c",
								Property = ri=> ri.Bool2
							}
						}
					});

					builder.Build(new string[] { "command", option, invalidBoolValue });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ParserInvalidValue, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
			}

			[Fact]
			public void InvalidValueProgramArgument_ForNonBoolOption_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int1 | i",
								Property = ri=> ri.Int1
							},
							new Option<TestRunInfo, int>
							{
								Key = "int2 | j",
								Property = ri=> ri.Int2
							}
						}
					});

					builder.Build(new string[] { "command", "--int1", "invalid" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ParserInvalidValue, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
			}

			[Theory]
			[InlineData("--bool1", "invalid")]
			[InlineData("-bc", "invalid")]
			public void InvalidValue_ForBoolOption_OnParseErrorUseMessage_IsSet_ThrowsExpectedMessage(
				string option, string invalidBoolValue)
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
								Key = "bool1 | b",
								Property = ri=> ri.Bool1,
								OnParseErrorUseMessage = value => value + " from OnParseErrorUseMessage"
							},
							new Option<TestRunInfo, bool>
							{
								Key = "bool2 | c",
								Property = ri=> ri.Bool2,
								OnParseErrorUseMessage = value => value + " from OnParseErrorUseMessage"
							}
						}
					});

					builder.Build(new string[] { "command", option, invalidBoolValue });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ParserInvalidValue, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
				Assert.Equal(invalidBoolValue + " from OnParseErrorUseMessage", processException.Message);
			}

			[Fact]
			public void InvalidValueProgramArgument_ForNonBoolOption_OnParseErrorUseMessage_IsSet_ThrowsExpectedMessage()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int1 | i",
								Property = ri=> ri.Int1,
								OnParseErrorUseMessage = value => value + " from OnParseErrorUseMessage"
							}
						}
					});

					builder.Build(new string[] { "command", "--int1", "invalid" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ParserInvalidValue, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
				Assert.Equal("invalid from OnParseErrorUseMessage", processException.Message);
			}
		}

		public class InNestedSubCommand
		{
			[Theory]
			[InlineData("invalid")]
			[InlineData("invalid=invalid=")]
			[InlineData("invalid=")]
			[InlineData("--=invalid")]
			[InlineData("-=i")]
			[InlineData("-ii")]
			// This tests that an invalid option program argument simply skips
			// the option stage and hits the invalid program argument stage.
			public void InvalidOption_TokenizeFail_Throws(string option)
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri=> ri.Int1
							}
						},
						SubCommands =
						{
							new SubCommand<TestRunInfo>
							{
								Key = "subcommand"
							}
						}
					});

					builder.Build(new string[] { "command", "subcommand", option });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.InvalidProgramArgument, processException.ErrorType);
				Assert.Equal(1, processException.CommandLevel);
			}


			[Theory]
			[InlineData("-ib")]
			[InlineData("-bi")]
			[InlineData("-cib")]
			[InlineData("-bci")]
			public void StackedOption_MappedToNonBoolProperty_Throws_1(string option)
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri=> ri.Int1
							},
							new Option<TestRunInfo, bool>
							{
								Key = "bool1 | b",
								Property = ri => ri.Bool1
							},
							new Option<TestRunInfo, bool>
							{
								Key = "bool2 | c",
								Property = ri => ri.Bool2
							}
						},
						SubCommands =
						{
							new SubCommand<TestRunInfo>
							{
								Key = "subcommand"
							}
						}
					});

					builder.Build(new string[] { "command", "subcommand", option });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.InvalidStackedOption, processException.ErrorType);
				Assert.Equal(1, processException.CommandLevel);
			}

			[Fact]
			public void NonBoolOption_MissingValueProgramArgument_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri=> ri.Int1
							}
						},
						SubCommands =
						{
							new SubCommand<TestRunInfo>
							{
								Key = "subcommand"
							}
						}

					});

					builder.Build(new string[] { "command", "subcommand", "--int" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ExpectedProgramArgument, processException.ErrorType);
				Assert.Equal(1, processException.CommandLevel);
			}

			[Theory]
			[InlineData("--int1", ProcessError.ExpectedValueFoundOption)]
			[InlineData("subcommand2", ProcessError.ExpectedValueFoundSubCommand)]
			public void Expected_OptionArgumentValue_AsNext_ButOptionOrSubCommand_Throws(
				string next, ProcessError expectedErrorType)
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri=> ri.Int1
							},
							new Option<TestRunInfo, int>
							{
								Key = "int1 | j",
								Property = ri=> ri.Int2
							}
						},
						SubCommands =
						{
							new SubCommand<TestRunInfo>
							{
								Key = "subcommand1",
								SubCommands =
								{
									new SubCommand<TestRunInfo>
									{
										Key = "subcommand2"
									}
								}
							}
						}
					});

					builder.Build(new string[] { "command", "subcommand1", "--int", next });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(expectedErrorType, processException.ErrorType);
				Assert.Equal(1, processException.CommandLevel);
			}

			[Theory]
			[InlineData("--bool1")]
			[InlineData("-bc")]
			public void InvalidValue_ForBoolOption_NotParseable_Throws(string option)
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
								Key = "bool1 | b",
								Property = ri=> ri.Bool1
							},
							new Option<TestRunInfo, bool>
							{
								Key = "bool2 | c",
								Property = ri=> ri.Bool2
							}
						},
						SubCommands =
						{
							new SubCommand<TestRunInfo>
							{
								Key = "subcommand"
							}
						}
					});

					var t = builder.Build(new string[] { "command", "subcommand", option, "invalid_bool_value" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ParserInvalidValue, processException.ErrorType);
				Assert.Equal(1, processException.CommandLevel);
			}

			[Fact]
			public void InvalidValueProgramArgument_ForNonBoolOption_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int1 | i",
								Property = ri=> ri.Int1
							},
							new Option<TestRunInfo, int>
							{
								Key = "int2 | j",
								Property = ri=> ri.Int2
							}
						},
						SubCommands =
						{
							new SubCommand<TestRunInfo>
							{
								Key = "subcommand"
							}
						}
					});

					builder.Build(new string[] { "command", "subcommand", "--int1", "invalid" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ParserInvalidValue, processException.ErrorType);
				Assert.Equal(1, processException.CommandLevel);
			}

			[Theory]
			[InlineData("--bool1")]
			[InlineData("-bc")]
			public void InvalidValue_ForBoolOption_OnParseErrorUseMessage_IsSet_ThrowsExpectedMessage(string option)
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
								Key = "bool1 | b",
								Property = ri=> ri.Bool1,
								OnParseErrorUseMessage = value => value + " from OnParseErrorUseMessage"
							},
							new Option<TestRunInfo, bool>
							{
								Key = "bool2 | c",
								Property = ri=> ri.Bool2,
								OnParseErrorUseMessage = value => value + " from OnParseErrorUseMessage"
							}
						},
						SubCommands =
						{
							new SubCommand<TestRunInfo>
							{
								Key = "subcommand"
							}
						}
					});

					var t = builder.Build(new string[] { "command", "subcommand", option, "invalid_bool_value" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ParserInvalidValue, processException.ErrorType);
				Assert.Equal(1, processException.CommandLevel);
				Assert.Equal("invalid_bool_value from OnParseErrorUseMessage", processException.Message);
			}

			[Fact]
			public void InvalidValueProgramArgument_ForNonBoolOption_OnParseErrorUseMessage_IsSet_ThrowsExpectedMessage()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						GlobalOptions =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int1 | i",
								Property = ri=> ri.Int1,
								OnParseErrorUseMessage = value => value + " from OnParseErrorUseMessage"
							}
						},
						SubCommands =
						{
							new SubCommand<TestRunInfo>
							{
								Key = "subcommand"
							}
						}
					});

					builder.Build(new string[] { "command", "subcommand", "--int1", "invalid" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ParserInvalidValue, processException.ErrorType);
				Assert.Equal(1, processException.CommandLevel);
				Assert.Equal("invalid from OnParseErrorUseMessage", processException.Message);
			}
		}
	}
}
