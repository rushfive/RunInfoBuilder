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
				Key = "command",
				Arguments =
				{
					new CustomArgument<TestRunInfo>
					{
						Count = 1,
						HelpToken = "<test>",
						Handler = context =>
						{
							throw new Exception("from custom arg callback");
						}
					}
				}
			});

			builder.Help.DisplayOnBuildFail();



			var runInfo = builder.Build(new string[] { "command", "what" });
		}
	}
}
