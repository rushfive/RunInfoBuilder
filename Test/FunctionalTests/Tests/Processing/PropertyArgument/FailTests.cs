﻿using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing.PropertyArgument
{
	public class PropertyArgumentFailTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		public class InSingleCommand
		{
			[Fact]
			public void ExpectedArgument_ButNoMore_ProgramArguments_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Arguments =
						{
							new PropertyArgument<TestRunInfo, bool>
							{
								Property = ri => ri.Bool1
							}
						}
					});

					builder.Build(new string[] { "command" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ExpectedProgramArgument, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
			}

			[Fact]
			public void Parser_DoesntHandleType_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Arguments =
						{
							new PropertyArgument<TestRunInfo, TestCustomType>
							{
								Property = ri => ri.CustomType,
								HelpToken = "<CustomType>"
							}
						}
					});

					builder.Build(new string[] { "command", "custom_type" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ParserUnhandledType, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
			}

			[Fact]
			public void InvalidValue_Unparseable_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Arguments =
						{
							new PropertyArgument<TestRunInfo, TestCustomType>
							{
								Property = ri => ri.CustomType,
								HelpToken = "<CustomType>"
							}
						}
					});

					builder.Parser.SetPredicateForType<TestCustomType>(value =>
					{
						if (value == "valid")
						{
							return (true, new TestCustomType());
						}
						return (false, default);
					});

					builder.Build(new string[] { "command", "invalid" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ParserInvalidValue, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
			}

			[Fact]
			public void OnProcessCallbackThrows_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Arguments =
						{
							new PropertyArgument<TestRunInfo, int>
							{
								Property = ri => ri.Int1,
								OnParsed = val =>
								{
									throw new TestException();
								}
							}
						}
					});

					builder.Build(new string[] { "command", "1" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.GeneralFailure, processException.ErrorType);

				Assert.IsType<TestException>(processException.InnerException);
			}
		}

		public class InNestedSubCommand
		{
			[Fact]
			public void ExpectedArgument_ButNoMore_ProgramArguments_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new SubCommand<TestRunInfo>
							{
								Key = "subcommand",
								Arguments =
								{
									new PropertyArgument<TestRunInfo, bool>
									{
										Property = ri => ri.Bool1
									}
								}
							}
						}
					});

					builder.Build(new string[] { "command", "subcommand" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ExpectedProgramArgument, processException.ErrorType);
				Assert.Equal(1, processException.CommandLevel);
			}

			[Fact]
			public void Parser_DoesntHandleType_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new SubCommand<TestRunInfo>
							{
								Key = "subcommand",
								Arguments =
								{
									new PropertyArgument<TestRunInfo, TestCustomType>
									{
										Property = ri => ri.CustomType,
										HelpToken = "<CustomType>"
									}
								}
							}
						}
					});

					builder.Build(new string[] { "command", "subcommand", "custom_type" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ParserUnhandledType, processException.ErrorType);
				Assert.Equal(1, processException.CommandLevel);
			}

			[Fact]
			public void InvalidValue_Unparseable_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new SubCommand<TestRunInfo>
							{
								Key = "subcommand",
								Arguments =
								{
									new PropertyArgument<TestRunInfo, TestCustomType>
									{
										Property = ri => ri.CustomType,
										HelpToken = "<CustomType>"
									}
								}
							}
						}
					});

					builder.Parser.SetPredicateForType<TestCustomType>(value =>
					{
						if (value == "valid")
						{
							return (true, new TestCustomType());
						}
						return (false, default);
					});

					builder.Build(new string[] { "command", "subcommand", "invalid" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ParserInvalidValue, processException.ErrorType);
				Assert.Equal(1, processException.CommandLevel);
			}

			[Fact]
			public void OnProcessCallbackThrows_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new SubCommand<TestRunInfo>
							{
								Key = "subcommand",
								Arguments =
								{
									new PropertyArgument<TestRunInfo, int>
									{
										Property = ri => ri.Int1,
										OnParsed = val =>
										{
											throw new TestException();
										}
									}
								}
							}
						}
					});

					builder.Build(new string[] { "command", "subcommand", "1" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.GeneralFailure, processException.ErrorType);

				Assert.IsType<TestException>(processException.InnerException);
			}
		}
	}
}
