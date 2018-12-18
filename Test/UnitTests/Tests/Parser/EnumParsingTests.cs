using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.UnitTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.UnitTests.Tests.Parser
{
	public class EnumParsingTests
	{
		public class NullableEnum
		{
			public class TryParseAs
			{
				[Fact]
				public void EmptyValue_ReturnsFalse_WithNullParsedValue()
				{
					var parser = new ArgumentParser();

					Type type = typeof(TestEnum?);
					string value = "";

					bool result = parser.TryParseAs(type, value, out object parsed);

					Assert.True(result);
					Assert.Null(parsed);
				}

				[Fact]
				public void NonNullValue_ReturnsTrue_WithParsedValue()
				{
					var parser = new ArgumentParser();

					Type type = typeof(TestEnum?);
					string value = "ValueB";

					bool result = parser.TryParseAs(type, value, out object parsed);

					Assert.True(result);
					Assert.NotNull(parsed);
					Assert.Equal(TestEnum.ValueB, parsed);
				}
			}

			public class TryParseAsT
			{
				[Fact]
				public void EmptyValue_ReturnsFalse_WithNullParsedValue()
				{
					var parser = new ArgumentParser();
					
					string value = "";

					bool result = parser.TryParseAs<TestEnum?>(value, out TestEnum? parsed);

					Assert.True(result);
					Assert.Null(parsed);
				}

				[Fact]
				public void NonNullValue_ReturnsTrue_WithParsedValue()
				{
					var parser = new ArgumentParser();
					
					string value = "ValueB";

					bool result = parser.TryParseAs<TestEnum?>(value, out TestEnum? parsed);

					Assert.True(result);
					Assert.NotNull(parsed);
					Assert.Equal(TestEnum.ValueB, parsed);
				}
			}
		}
	}
}
