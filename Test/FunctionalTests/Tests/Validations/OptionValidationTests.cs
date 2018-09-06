using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Validations
{
    public class OptionValidationTests
    {
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		[Fact]
		public void NullPropertyExpression_Throws()
		{
			RunInfoBuilder builder = GetBuilder();

			Assert.Throws<InvalidOperationException>(() =>
			{
				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "key",
					Options =
					{
						new Option<TestRunInfo, bool>
						{
							Property = null
						}
					}
				});
			});
		}

		[Fact]
		public void Property_NotWritable_Throws()
		{
			RunInfoBuilder builder = GetBuilder();

			Assert.Throws<InvalidOperationException>(() =>
			{
				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = "key",
					Options =
					{
						new Option<TestRunInfo, bool>
						{
							Property = ri => ri.UnwritableBool
						}
					}
				});
			});
		}
	}
}
