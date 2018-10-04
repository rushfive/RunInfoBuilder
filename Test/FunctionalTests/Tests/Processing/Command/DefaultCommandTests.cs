using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing.Command
{
	public class DefaultCommandTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		public class Options
		{
			[Fact]
			public void Processes_Successfully()
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.AddDefault(new DefaultCommand<TestRunInfo>
				{
					Options =
					{
						new Option<TestRunInfo, bool>
						{
							Key = "bool | b",
							Property = ri => ri.Bool1
						},
						new Option<TestRunInfo, int>
						{
							Key = "int | i",
							Property = ri => ri.Int1
						}
					}
				});

				assertFromArgs(new string[] { "--bool", "--int=99" });
				assertFromArgs(new string[] { "--bool=true", "--int=99" });
				assertFromArgs(new string[] { "-b", "true", "-i=99" });
				
				void assertFromArgs(string[] args)
				{
					var result = (TestRunInfo)builder.Build(args);
					Assert.True(result.Bool1);
					Assert.Equal(99, result.Int1);
				}
			}

			[Fact]
			public void Processes_Stacked_Successfully()
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.AddDefault(new DefaultCommand<TestRunInfo>
				{
					Options =
					{
						new Option<TestRunInfo, bool>
						{
							Key = "bool1 | a",
							Property = ri => ri.Bool1
						},
						new Option<TestRunInfo, bool>
						{
							Key = "bool2 | b",
							Property = ri => ri.Bool2
						},
						new Option<TestRunInfo, bool>
						{
							Key = "bool3 | c",
							Property = ri => ri.Bool3
						},
					}
				});

				assertFromArgs(new string[] { "-abc" });
				assertFromArgs(new string[] { "-abc", "true" });
				assertFromArgs(new string[] { "-abc=true" });

				void assertFromArgs(string[] args)
				{
					var result = (TestRunInfo)builder.Build(args);
					Assert.True(result.Bool1);
					Assert.True(result.Bool2);
					Assert.True(result.Bool3);
					
				}
			}
		}

		public class Arguments
		{
			[Fact]
			public void PropertyArgument_Success()
			{
				RunInfoBuilder builder = GetBuilder();

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

				var result = (TestRunInfo)builder.Build(new string[] { "99" });
				Assert.Equal(99, result.Int1);
			}

			[Fact]
			public void SequenceArgument_Success()
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.AddDefault(new DefaultCommand<TestRunInfo>
				{
					Arguments =
					 {
						 new SequenceArgument<TestRunInfo, int>
						 {
							 ListProperty = ri => ri.IntList1
						 }
					 }
				});

				var result = (TestRunInfo)builder.Build(new string[] { "1", "2", "3" });

				Assert.Equal(3, result.IntList1.Count);
				Assert.Equal(1, result.IntList1[0]);
				Assert.Equal(2, result.IntList1[1]);
				Assert.Equal(3, result.IntList1[2]);
			}

			[Fact]
			public void CustomArgument_Success()
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.AddDefault(new DefaultCommand<TestRunInfo>
				{
					Arguments =
					 {
						 new CustomArgument<TestRunInfo>
						 {
							 Count = 3,
							 Handler = context =>
							 {
								 Assert.Equal(3, context.ProgramArguments.Count);
								 Assert.Equal("1", context.ProgramArguments[0]);
								 Assert.Equal("abc", context.ProgramArguments[1]);
								 Assert.Equal("!!!", context.ProgramArguments[2]);
								 return ProcessResult.Continue;
							 }
						 }
					 }
				});

				builder.Build(new string[] { "1", "abc", "!!!" });
			}
		}

		public class PostBuildCallback
		{
			[Fact]
			public void OnSuccessfulBuild_Invokes()
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Commands.AddDefault(
					new DefaultCommand<TestRunInfo>
					{
						Options =
						{
								new Option<TestRunInfo, bool>
								{
									Key = "bool",
									Property = ri => ri.Bool1
								}
						}
					},
					runInfo =>
					{
						Assert.True(runInfo.Bool1);
					});

				builder.Build(new string[] { "--bool" });
			}
		}
	}
}
