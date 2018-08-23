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
					new Option<TestRunInfo, bool>
					{
						Key = "bool2",
						Property = ri => ri.Bool2
					}
				}
			});

			var runInfo = builder.Build(new string[] { "test", "1", "bool2=true" });
		}
	}
}
