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
				Arguments =
				{
					new ArgumentPropertyMapped<TestRunInfo, bool>
					{
						Property = ri => ri.Bool1
					}
				},
				Options =
				{
					new Option<TestRunInfo, string>
					{
						Key = "string1",
						Property = ri => ri.String1
					}
				}
			});

			var runInfo = builder.Build(new string[] { "test", "1", "--string1=what" });
		}
	}
}
