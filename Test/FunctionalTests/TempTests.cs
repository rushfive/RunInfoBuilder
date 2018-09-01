using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests
{
	public class TempTests
	{
		[Fact]
		public void Test()
		{
			var builder = new RunInfoBuilder();

			builder.Commands.Add<TestRunInfo>(new Command<TestRunInfo>
			{
				Key = "test",
				//Arguments =
				//{
				//	new PropertyArgument<TestRunInfo, bool>
				//	{
				//		Property = ri => ri.Bool1
				//	},
				//	new SequenceArgument<TestRunInfo, string>
				//	{
				//		ListProperty = ri => ri.StringList1
				//	}
				//},
				Options =
				{
					new Option<TestRunInfo, bool>
					{
						Key = "option | a",
						Property = ri => ri.Bool1
					},
					new Option<TestRunInfo, bool>
					{
						Key = "option2 | b",
						Property = ri => ri.Bool2
					},
					new Option<TestRunInfo, bool>
					{
						Key = "option3 | c",
						Property = ri => ri.Bool3
					},
					new Option<TestRunInfo, bool>
					{
						Key = "option4 | d",
						Property = ri => ri.UnwritableBool
					}
				}
			});

			var runInfo = builder.Build(new string[] { "test", "-abcd" });
		}
	}
}
