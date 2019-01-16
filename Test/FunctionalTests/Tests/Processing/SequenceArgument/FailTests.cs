using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing.SequenceArgument
{
	public class SequenceArgumentFailTests
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
							new SequenceArgument<TestRunInfo, string>
							{
								ListProperty = ri => ri.StringList1
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

			[Theory]
			[InlineData("a")]
			[InlineData("a", "1")]
			[InlineData("1", "a")]
			[InlineData("1", "2", "a")]
			[InlineData("1", "a", "2")]
			public void ProgramArguments_ContainsUnparseableValues_Throws(params string[] sequenceValues)
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Arguments =
						{
							new SequenceArgument<TestRunInfo, int>
							{
								ListProperty = ri => ri.IntList1
							}
						}
					});

					var programArgs = new List<string> { "command" };
					programArgs.AddRange(sequenceValues);

					builder.Build(programArgs.ToArray());
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ParserInvalidValue, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
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
									new SequenceArgument<TestRunInfo, string>
									{
										ListProperty = ri => ri.StringList1
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

			[Theory]
			[InlineData("a")]
			[InlineData("a", "1")]
			[InlineData("1", "a")]
			[InlineData("1", "2", "a")]
			[InlineData("1", "a", "2")]
			public void ProgramArguments_ContainsUnparseableValues_Throws(params string[] sequenceValues)
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
									new SequenceArgument<TestRunInfo, int>
									{
										ListProperty = ri => ri.IntList1
									}
								}
							}
						}
					});

					var programArgs = new List<string> { "command", "subcommand" };
					programArgs.AddRange(sequenceValues);

					builder.Build(programArgs.ToArray());
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ParserInvalidValue, processException.ErrorType);
				Assert.Equal(1, processException.CommandLevel);
			}
		}
	}
}
