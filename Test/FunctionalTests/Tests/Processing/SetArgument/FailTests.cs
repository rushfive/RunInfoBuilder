using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing.SetArgument
{
	public class FailTests
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
							new SetArgument<TestRunInfo, bool>
							{
								Property = ri => ri.Bool1,
								Values = new List<(string, bool)>
								{
									("true", true), ("false", false)
								}
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
			public void ProgramArgument_InvalidMatch_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Arguments =
						{
							new SetArgument<TestRunInfo, bool>
							{
								Property = ri => ri.Bool1,
								Values = new List<(string, bool)>
								{
									("true", true), ("false", false)
								}
							}
						}
					});

					builder.Build(new string[] { "command", "invalid" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.UnknownValue, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
			}
		}

		public class InNestedCommand
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
							new Command<TestRunInfo>
							{
								Key = "subcommand",
								Arguments =
								{
									new SetArgument<TestRunInfo, bool>
									{
										Property = ri => ri.Bool1,
										Values = new List<(string, bool)>
										{
											("true", true), ("false", false)
										}
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
			public void ProgramArgument_InvalidMatch_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "subcommand",
								Arguments =
								{
									new SetArgument<TestRunInfo, bool>
									{
										Property = ri => ri.Bool1,
										Values = new List<(string, bool)>
										{
											("true", true), ("false", false)
										}
									}
								}
							}
						}
					});

					builder.Build(new string[] { "command", "subcommand", "invalid" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.UnknownValue, processException.ErrorType);
				Assert.Equal(1, processException.CommandLevel);
			}
		}
	}
}
