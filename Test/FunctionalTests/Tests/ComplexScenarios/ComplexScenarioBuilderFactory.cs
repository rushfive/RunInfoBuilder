using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.ComplexScenarios
{
	public static class ComplexScenarioBuilderFactory
	{
		public static RunInfoBuilder GetBuilder()
		{
			var builder = new RunInfoBuilder();

			builder.Commands
				.AddDefault(new DefaultCommand<TestRunInfo>
				{
					Options = Options()
				})
				.Add(Commands.SingleLevel.CustomArgToSequence())
				.Add(Commands.MultiLevel.ComplexNestedCommand());
			
			return builder;
		}

		private static List<OptionBase<TestRunInfo>> Options()
		{
			return new List<OptionBase<TestRunInfo>>
			{
				new Option<TestRunInfo, string>
				{
					Key = "string | s",
					Property = ri => ri.String1
				},
				new Option<TestRunInfo, int>
				{
					Key = "int | i",
					Property = ri => ri.Int1,
					OnProcess = (int val) =>
					{
						if (val < 100)
						{
							throw new TestException();
						}
						return ProcessResult.Continue;
					}
				},
				new Option<TestRunInfo, bool>
				{
					Key = "bool1 | a",
					Property = ri => ri.Bool1
				},
				new Option<TestRunInfo, bool>
				{
					Key = "bool2 | b",
					Property = ri => ri.Bool2
				}
			};
		}

		private static class Commands
		{
			public static class SingleLevel
			{
				// custom arg followed by sequence
				public static Command<TestRunInfo> CustomArgToSequence()
				{
					return new Command<TestRunInfo>
					{
						Key = "CustomToSequence",
						Options = Options(),
						Arguments =
						{
							new CustomArgument<TestRunInfo>
							{
								Count = 2,
								HelpToken = "custom_help_token",
								Handler = context =>
								{
									context.RunInfo.BoolFromCustomArg1 = true;
									return ProcessResult.Continue;
								}
							},
							new SequenceArgument<TestRunInfo, int>
							{
								ListProperty = ri => ri.IntList1
							}
						}
					};
				}
			}

			public static class MultiLevel
			{
				public static Command<TestRunInfo> ComplexNestedCommand()
				{
					return new Command<TestRunInfo>
					{
						Key = "ComplexCommand",
						Options = Options(),
						Arguments =
						{
							new SetArgument<TestRunInfo, int>
							{
								Property = ri => ri.Int1,
								Values =
								{
									("one", 1), ("two", 2)
								}
							},
							new PropertyArgument<TestRunInfo, int>
							{
								Property = ri => ri.Int2
							},
							new CustomArgument<TestRunInfo>
							{
								Count = 2,
								HelpToken = "<custom>",
								Handler = context =>
								{
									int first = int.Parse(context.ProgramArguments[0]);
									int second = int.Parse(context.ProgramArguments[1]);
									context.RunInfo.Int3 = first + second;
									return ProcessResult.Continue;
								}
							}
						},
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "StringSubCommand",
								Options = Options(),
								Arguments =
								{
									new PropertyArgument<TestRunInfo, string>
									{
										Property = ri => ri.String1
									},
									new SetArgument<TestRunInfo, string>
									{
										Property = ri => ri.String2,
										Values =
										{
											( "one", "one" ), ("two", "two")
										}
									},
									new CustomArgument<TestRunInfo>
									{
										Count = 2,
										HelpToken = "<custom>",
										Handler = context =>
										{
											context.RunInfo.String3 =
												context.ProgramArguments[0] + context.ProgramArguments[1];
											return ProcessResult.Continue;
										}
									}
								}
							},
							new Command<TestRunInfo>
							{
								Key = "DoubleSubCommand",
								Arguments =
								{
									new PropertyArgument<TestRunInfo, double>
									{
										Property = ri => ri.Double1
									},
									new SequenceArgument<TestRunInfo, double>
									{
										ListProperty = ri => ri.DoubleList1,
										OnProcess = val =>
										{
											if (val < 9.9)
											{
												return ProcessResult.End;
											}
											return ProcessResult.Continue;
										}
									}
								}
							}
						}
					};
				}
			}


			
		}
	}
}
