using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing.Command
{
	public class SubCommandTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		public class RootCommandLevel
		{
			[Fact]
			public void SubCommandsConfigured_NoMoreArgs_Throws()
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
							Key = "subcommand"
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
			public void SubCommandsConfigured_NextArg_DoestMatch_Throws()
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
							Key = "subcommand"
						}
					}
					});

					builder.Build(new string[] { "command", "invalid_match" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.InvalidSubCommand, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
			}
		}

		public class BelowRootCommandLevel
		{
			[Fact]
			public void SubCommandsConfigured_NoMoreArgs_Throws()
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
								Key = "sub1",
								SubCommands =
								{
									new SubCommand<TestRunInfo>
									{
										Key = "sub2"
									}
								}
							}
						}
					});

					builder.Build(new string[] { "command", "sub1" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ExpectedProgramArgument, processException.ErrorType);
				Assert.Equal(1, processException.CommandLevel);
			}

			[Fact]
			public void SubCommandsConfigured_NextArg_DoestMatch_Throws()
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
								Key = "sub1",
								SubCommands =
								{
									new SubCommand<TestRunInfo>
									{
										Key = "sub2"
									}
								}
							}
						}
					});

					builder.Build(new string[] { "command", "sub1", "invalid_match" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.InvalidSubCommand, processException.ErrorType);
				Assert.Equal(1, processException.CommandLevel);
			}
		}

		
	}
}
