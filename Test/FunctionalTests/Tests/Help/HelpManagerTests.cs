using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Help
{
	public class HelpManagerTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		private static readonly string[] _defaultTriggers = new string[]
		{
			"--help", "-h", "/help"
		};

		public class DefaultTriggers
		{
			[Fact]
			public void InvokesHelp_If_CustomTriggersNotSet()
			{
				var builder = GetBuilder();

				builder.Help.OnTrigger(() =>
				{
					throw new TestException();
				});

				foreach (var trigger in _defaultTriggers)
				{
					Action testCode = () =>
					{
						builder.Build(new string[] { trigger });
					};

					Exception exception = Record.Exception(testCode);

					var testException = exception as TestException;
					Assert.NotNull(testException);
				}
			}

			[Fact]
			public void DoesntInvokeHelp_If_CustomTriggersAreSet()
			{
				var builder = GetBuilder();

				builder.Help.OnTrigger(() =>
				{
					throw new TestException();
				});

				builder.Help.SetTriggers("not_default");

				foreach (var trigger in _defaultTriggers)
				{
					Action testCode = () =>
					{
						builder.Build(new string[] { trigger });
					};

					Exception exception = Record.Exception(testCode);

					var testException = exception as TestException;
					Assert.Null(testException);
				}
			}
		}

		public class SetProgramName
		{
			[Theory]
			[InlineData(null)]
			[InlineData("")]
			public void NullOrEmpty_Throws(string programName)
			{
				Action testCode = () =>
				{
					var builder = GetBuilder();
					builder.Help.SetProgramName(programName);
				};

				Exception exception = Record.Exception(testCode);

				var nullException = exception as ArgumentNullException;
				Assert.NotNull(nullException);
			}

			[Fact]
			public void Setting_IncludesIt_InHelpText_ForDefaultCommand()
			{
				var builder = GetBuilder();
				builder.Help.SetProgramName("test.exe");

				builder.Commands.AddDefault(new DefaultCommand<TestRunInfo>());

				var text = builder.Help.ToString();

				Assert.Contains("test.exe", text);
			}

			[Fact]
			public void Setting_IncludesIt_InHelpText_ForCommands()
			{
				var builder = GetBuilder();
				builder.Help.SetProgramName("test.exe");

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "command"
				});

				var text = builder.Help.ToString();

				Assert.Contains("test.exe", text);
			}

			[Fact]
			public void NotSetting_ExcludesIt_InHelpText_ForDefaultCommand()
			{
				var builder = GetBuilder();

				builder.Commands.AddDefault(new DefaultCommand<TestRunInfo>());

				var text = builder.Help.ToString();

				Assert.DoesNotContain("test.exe", text);
			}

			[Fact]
			public void NotSetting_ExcludesIt_InHelpText_ForCommands()
			{
				var builder = GetBuilder();

				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "command"
				});

				var text = builder.Help.ToString();

				Assert.DoesNotContain("test.exe", text);
			}
		}

		public class SetTriggers
		{
			[Fact]
			public void NullArgument_Throws()
			{
				Action testCode = () =>
				{
					var builder = GetBuilder();
					builder.Help.SetTriggers(null);
				};

				Exception exception = Record.Exception(testCode);

				var nullException = exception as ArgumentNullException;
				Assert.NotNull(nullException);
			}

			[Fact]
			public void EmptyArgument_Throws()
			{
				Action testCode = () =>
				{
					var builder = GetBuilder();
					builder.Help.SetTriggers(new string[] { });
				};

				Exception exception = Record.Exception(testCode);

				var nullException = exception as ArgumentNullException;
				Assert.NotNull(nullException);
			}

			[Fact]
			public void InvokesHelp_IfSet()
			{
				var customTriggers = new string[]
				{
					"custom1", "custom2"
				};

				var builder = GetBuilder();

				builder.Help.OnTrigger(() =>
				{
					throw new TestException();
				});

				builder.Help.SetTriggers(customTriggers);

				foreach (var trigger in customTriggers)
				{
					Action testCode = () =>
					{
						builder.Build(new string[] { trigger });
					};

					Exception exception = Record.Exception(testCode);

					var testException = exception as TestException;
					Assert.NotNull(testException);
				}
			}
		}

		public class InvokeOnBuildFail
		{
			[Fact]
			public void Invokes_OnBuildError()
			{
				var builder = GetBuilder();

				builder.Help
					.InvokeOnBuildFail(suppressException: false)
					.OnTrigger(() =>
					{
						throw new TestException();
					});

				builder.Commands.AddDefault(new DefaultCommand<TestRunInfo>
				{
					Arguments =
					{
						new PropertyArgument<TestRunInfo, int>
						{
							Property = ri => ri.Int1
						}
					}
				});

				Action testCode = () =>
				{
					builder.Build(new string[] { "not an int" });
				};

				Exception exception = Record.Exception(testCode);

				var testException = exception as TestException;
				Assert.NotNull(testException);
			}

			[Fact]
			public void SupressingException_OnlyInvokesHelp()
			{
				var builder = GetBuilder();

				builder.Help
					.InvokeOnBuildFail(suppressException: true);

				builder.Commands.AddDefault(new DefaultCommand<TestRunInfo>
				{
					Arguments =
					{
						new PropertyArgument<TestRunInfo, int>
						{
							Property = ri => ri.Int1
						}
					}
				});

				Action testCode = () =>
				{
					builder.Build(new string[] { "not an int" });
				};

				Exception exception = Record.Exception(testCode);
				Assert.Null(exception);
			}
		}

		public class OnTrigger
		{
			[Fact]
			public void Null_Throws()
			{
				Action testCode = () =>
				{
					var builder = GetBuilder();
					builder.Help.OnTrigger(null);
				};

				Exception exception = Record.Exception(testCode);

				var nullException = exception as ArgumentNullException;
				Assert.NotNull(nullException);
			}

			[Fact]
			public void InvokesCallback_IfSet()
			{
				Action testCode = () =>
				{
					var builder = GetBuilder();
					builder.Help
						.InvokeOnBuildFail(suppressException: false)
						.OnTrigger(() =>
						{
							throw new TestException();
						});

					builder.Commands.AddDefault(new DefaultCommand<TestRunInfo>
					{
						Arguments =
						{
							new PropertyArgument<TestRunInfo, int>
							{
								Property = ri => ri.Int1
							}
						}
					});

					builder.Build(new string[] { "not an int" });
				};

				Exception exception = Record.Exception(testCode);

				var testException = exception as TestException;
				Assert.NotNull(testException);
			}
		}









	}
}
