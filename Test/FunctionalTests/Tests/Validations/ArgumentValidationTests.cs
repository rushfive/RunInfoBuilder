using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Validations
{
	public class ArgumentValidationTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		public class PropertyArgumentTests
		{
			[Fact]
			public void NullPropertyExpression_Throws()
			{
				RunInfoBuilder builder = GetBuilder();

				Assert.Throws<InvalidOperationException>(() =>
				{
					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Arguments =
						{
							new PropertyArgument<TestRunInfo, bool>
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
						Arguments =
						{
							new PropertyArgument<TestRunInfo, bool>
							{
								Property = ri => ri.UnwritableBool
							}
						}
					});
				});
			}
		}

		public class SequenceArgumentTests
		{
			[Fact]
			public void NullPropertyExpression_Throws()
			{
				RunInfoBuilder builder = GetBuilder();

				Assert.Throws<InvalidOperationException>(() =>
				{
					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Arguments =
						{
							new SequenceArgument<TestRunInfo, string>
							{
								ListProperty = null
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
						Arguments =
						{
							new SequenceArgument<TestRunInfo, string>
							{
								ListProperty = ri => ri.UnwritableStringList
							}
						}
					});
				});
			}
		}
	}
}
