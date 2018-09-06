using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Validations
{
	public class CommandValidationTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void NullOrEmptyKey_Throws(string key)
		{
			RunInfoBuilder builder = GetBuilder();

			Assert.Throws<InvalidOperationException>(() =>
			{
				builder.Commands.Add(new Command<TestRunInfo>
				{
					Key = key
				});
			});
		}

		public class ArgumentsTests
		{
			[Fact]
			public void HasNullArguments_Throws()
			{
				RunInfoBuilder builder = GetBuilder();

				Assert.Throws<InvalidOperationException>(() =>
				{
					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Arguments =
					{
						null
					}
					});
				});
			}
		}

		public class OptionsTests
		{
			[Fact]
			public void HasNullOptions_Throws()
			{
				RunInfoBuilder builder = GetBuilder();

				Assert.Throws<InvalidOperationException>(() =>
				{
					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Options =
						{
							null
						}
					});
				});
			}

			[Fact]
			public void Option_WithNullKey_Throws()
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
								Key = null,
								Property = ri => ri.Bool1
							}
						}
					});
				});
			}
			
			[Theory]
			[InlineData("")]
			[InlineData("|")]
			[InlineData("*")]
			[InlineData("|x")]
			[InlineData("xxx|")]
			[InlineData("|xxx|x")]
			[InlineData("xxx|x|")]
			[InlineData("valid", "xxx|x|")]
			[InlineData("xxx|x|", "valid")]
			public void Any_InvalidKey_Throws(params string[] keys)
			{
				RunInfoBuilder builder = GetBuilder();

				var options = new List<OptionBase<TestRunInfo>>();
				
				foreach(string key in keys)
				{
					options.Add(new Option<TestRunInfo, bool>
					{
						Key = key,
						Property = ri => ri.Bool1
					});
				}

				Assert.Throws<InvalidOperationException>(() =>
				{
					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Options = options
					});
				});
			}

			[Theory]
			[InlineData("duplicate", "duplicate")]
			[InlineData("duplicate | 1", "duplicate | 2")]
			[InlineData("unique", "duplicate", "duplicate")]
			[InlineData("duplicate", "duplicate", "unique")]
			[InlineData("unique1", "duplicate", "duplicate", "unique2")]
			public void Duplicate_FullKeys_Throws(params string[] keys)
			{
				RunInfoBuilder builder = GetBuilder();

				var options = new List<OptionBase<TestRunInfo>>();

				foreach (string key in keys)
				{
					options.Add(new Option<TestRunInfo, bool>
					{
						Key = key,
						Property = ri => ri.Bool1
					});
				}

				Assert.Throws<InvalidOperationException>(() =>
				{
					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Options = options
					});
				});
			}

			[Theory]
			[InlineData("unique1 | 1", "unique2 | 1")]
			[InlineData("unique1 | 1", "unique2 | 2", "unique3 | 2")]
			[InlineData("unique1 | 1", "unique2 | 1", "unique3 | 2")]
			public void Duplicate_ShortKeys_Throws(params string[] keys)
			{
				RunInfoBuilder builder = GetBuilder();

				var options = new List<OptionBase<TestRunInfo>>();

				foreach (string key in keys)
				{
					options.Add(new Option<TestRunInfo, bool>
					{
						Key = key,
						Property = ri => ri.Bool1
					});
				}

				Assert.Throws<InvalidOperationException>(() =>
				{
					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						Options = options
					});
				});
			}
		}

		public class SubCommandsTests
		{
			[Fact]
			public void HasNullSubCommands_Throws()
			{
				RunInfoBuilder builder = GetBuilder();

				Assert.Throws<InvalidOperationException>(() =>
				{
					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						SubCommands =
						{
							null
						}
					});
				});
			}

			[Fact]
			public void DuplicateKeys_Throws()
			{
				RunInfoBuilder builder = GetBuilder();

				Assert.Throws<InvalidOperationException>(() =>
				{
					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "key",
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "duplicate"
							},
							new Command<TestRunInfo>
							{
								Key = "duplicate"
							}
						}
					});
				});
			}
		}
	}
}
