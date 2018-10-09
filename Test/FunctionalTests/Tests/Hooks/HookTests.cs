using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Hooks
{
	public class HookTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		[Fact]
		public void If_OnStartSet_Invokes()
		{
			RunInfoBuilder builder = GetBuilder();

			bool setFromCallback = false;

			builder.Hooks.SetOnStartBuild(args =>
			{
				setFromCallback = true;
			});

			builder.Commands.AddDefault(new DefaultCommand<TestRunInfo>
			{
				Options =
				{
					new Option<TestRunInfo, bool>
					{
						Key = "bool",
						Property = ri => ri.Bool1
					}
				}
			});

			builder.Build(new string[] { "--bool" });

			Assert.True(setFromCallback);
		}
	}
}
