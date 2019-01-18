using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Hooks
{
	public class NullOrEmptyReturnsTests
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

				builder.Hooks.ArgsNullOrEmptyReturns<object>(null);
			};

			Exception exception = Record.Exception(testCode);

			Assert.IsType<ArgumentNullException>(exception);
		}

		[Fact]
		public void Invoked_IfSet_AndArgsNull()
		{
			RunInfoBuilder builder = GetBuilder();

			builder.Hooks.ArgsNullOrEmptyReturns<int>(() => 100);

			var result = builder.Build(null);

			Assert.Equal(100, result);
		}

		[Fact]
		public void Invoked_IfSet_AndArgsEmpty()
		{
			RunInfoBuilder builder = GetBuilder();

			builder.Hooks.ArgsNullOrEmptyReturns<int>(() => 100);

			var result = builder.Build(new string[] { });

			Assert.Equal(100, result);
		}

		[Fact]
		public void OnlyInvokes_IfSet_AndArgsNullOrEmpty()
		{
			RunInfoBuilder builder = GetBuilder();

			builder.Hooks.ArgsNullOrEmptyReturns<int>(() => throw new TestException());

			builder.Commands.Add(new Command<TestRunInfo>
			{
				Key = "command"
			});

			Exception exception = Record.Exception(() =>
			{
				builder.Build(new string[] { "command" });
			});

			Assert.Null(exception);

			exception = Record.Exception(() =>
			{
				builder.Build(null);
			});

			Assert.NotNull(exception);
			Assert.IsType<TestException>(exception);

			exception = Record.Exception(() =>
			{
				builder.Build(new string[] { });
			});

			Assert.NotNull(exception);
			Assert.IsType<TestException>(exception);
		}
	}
}
