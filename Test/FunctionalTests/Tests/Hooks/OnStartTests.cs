using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Hooks
{
	public class OnStartTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		[Fact]
		public void NullCallback_Throws()
		{
			Action testCode = () =>
			{
				RunInfoBuilder builder = GetBuilder();

				builder.Hooks.OnStartBuild(null);
			};

			Exception exception = Record.Exception(testCode);

			Assert.IsType<ArgumentNullException>(exception);
		}

		[Fact]
		public void If_OnStartSet_Invokes()
		{
			RunInfoBuilder builder = GetBuilder();

			bool setFromCallback = false;

			builder.Hooks.OnStartBuild(args =>
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
