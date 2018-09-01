//using R5.RunInfoBuilder.Commands;
//using R5.RunInfoBuilder.FunctionalTests.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;

//namespace R5.RunInfoBuilder.FunctionalTests.Tests.SingleLevel
//{
//	public class ArgumentTests
//	{
//		public class PropertyArgument
//		{
//			[Fact]
//			public void MissingArgument_Throws()
//			{
//				var builder = new RunInfoBuilder();

//				builder.Commands.Add<TestRunInfo>(new Command<TestRunInfo>
//				{
//					Key = "test",
//					Arguments =
//					{
//						new PropertyArgument<TestRunInfo, bool>
//						{
//							Property = ri => ri.Bool1
//						}
//					}
//				});

//				Assert.Throws<InvalidOperationException>(
//					() => builder.Build(new string[] { "test" }));
//			}
//		}

//	}
//}
