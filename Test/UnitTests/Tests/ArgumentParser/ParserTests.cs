using R5.RunInfoBuilder.ArgumentParser;
using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.UnitTests.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace R5.RunInfoBuilder.UnitTests.Tests.ArgumentParser
{
	public class ParserTests
	{
		public class Configure_Method
		{
			[Fact]
			public void Parser_CorrectlyResolvesPredicates()
			{
				var parserWithoutPredicates = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				Assert.False(parserWithoutPredicates.HandlesType<DateTime>());

				var predicates = new List<(Type, Func<string, (bool, object)>)>
				{
					(typeof(DateTime), value => (true, DateTime.MinValue))
				};

				var config = new ParserConfig(AutoParseEnum.Never, predicates);

				var parser = new Parser(config);

				Assert.True(parser.HandlesType<DateTime>());

				parser.TryParseAs(typeof(DateTime), "value", out object parsed);

				Assert.Equal(DateTime.MinValue, (DateTime)parsed);
			}

			[Fact]
			public void Parser_AutoParseEnumTrue()
			{
				var parserNoAutoParse = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				Assert.Throws<RunInfoBuilderException>(
					() => parserNoAutoParse.TryParseAs(typeof(TestEnum), "ValueA", out _));

				var parser = new Parser(
					new ParserConfig(AutoParseEnum.ParseCaseSensitive, null));

				bool valid = parser.TryParseAs<TestEnum>("ValueA", out TestEnum parsed);
				Assert.True(valid);
				Assert.Equal(TestEnum.ValueA, parsed);
			}

			[Fact]
			public void Parser_AutoParseEnumTrue_IgnoreCase()
			{
				var parserCaseSensitive = new Parser(
					new ParserConfig(AutoParseEnum.ParseCaseSensitive, null));

				bool validParse = parserCaseSensitive.TryParseAs(typeof(TestEnum), "valuea", out _);
				Assert.False(validParse);

				var parser = new Parser(
					new ParserConfig(AutoParseEnum.ParseCaseInsensitive, null));

				bool valid = parser.TryParseAs<TestEnum>("valuea", out TestEnum parsed);
				Assert.True(valid);
				Assert.Equal(TestEnum.ValueA, parsed);
			}
		}

		public class SetPredicateForType_Method
		{
			[Fact]
			public void ForNewType_Adds()
			{
				var parser = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				Assert.False(parser.HandlesType<DateTime>());

				parser.SetPredicateForType<DateTime>(value => (true, DateTime.MinValue));

				Assert.True(parser.HandlesType<DateTime>());
			}

			[Fact]
			public void ForAlreadyExistingType_Replaces()
			{
				var parser = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				parser.SetPredicateForType<DateTime>(value => (true, DateTime.MinValue));

				parser.TryParseAs<DateTime>("value", out DateTime parsed1);
				Assert.Equal(DateTime.MinValue, parsed1);

				parser.SetPredicateForType<DateTime>(value => (true, DateTime.MinValue.AddDays(3)));

				parser.TryParseAs<DateTime>("value", out DateTime parsed2);
				Assert.NotEqual(DateTime.MinValue, parsed2);
			}
		}

		// Enum handling tested in Configure method tests

		public class TryParseAs_TypeAsParameter_Method
		{
			[Fact]
			public void NullTypeArgument_Throws()
			{
				var parser = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				Assert.Throws<ArgumentNullException>(
					() => parser.TryParseAs(null, null, out _));
			}

			[Fact]
			public void NullValueArgument_Throws()
			{
				var parser = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				Assert.Throws<ArgumentNullException>(
					() => parser.TryParseAs(typeof(DateTime), null, out _));
			}

			[Fact]
			public void TypeUnhandled_Throws()
			{
				var parser = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				Assert.Throws<RunInfoBuilderException>(
					() => parser.TryParseAs(typeof(DateTime), "value", out _));
			}

			[Fact]
			public void ValidParse_ReturnsTrue_With_CorrectOutResult()
			{
				var parser = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				parser.SetPredicateForType<DateTime>(value => (true, DateTime.MaxValue));

				bool valid = parser.TryParseAs(typeof(DateTime), "value", out object parsed);

				Assert.True(valid);
				Assert.IsType<DateTime>(parsed);
				Assert.NotEqual(default(DateTime), parsed);
			}

			[Fact]
			public void InvalidParse_ReturnsFalse_With_DefaultOutResult()
			{
				var parser = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				parser.SetPredicateForType<DateTime>(value =>
				{
					if (value != "valid")
					{
						return (false, default);
					}
					return (true, DateTime.MaxValue);
				});

				bool valid = parser.TryParseAs(typeof(DateTime), "invalid", out object parsed);

				Assert.False(valid);
				Assert.Null(parsed);
			}
		}

		public class TryParseAs_Generic_Method
		{
			[Fact]
			public void NullValueArgument_Throws()
			{
				var parser = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				Assert.Throws<ArgumentNullException>(
					() => parser.TryParseAs<DateTime>(null, out _));
			}

			[Fact]
			public void TypeUnhandled_Throws()
			{
				var parser = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				Assert.Throws<RunInfoBuilderException>(
					() => parser.TryParseAs<DateTime>("value", out _));
			}

			[Fact]
			public void ValidParse_ReturnsTrue_With_CorrectOutResult()
			{
				var parser = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				parser.SetPredicateForType<DateTime>(value => (true, DateTime.MaxValue));

				bool valid = parser.TryParseAs<DateTime>("value", out DateTime parsed);

				Assert.True(valid);
				Assert.IsType<DateTime>(parsed);
				Assert.NotEqual(default(DateTime), parsed);
			}

			[Fact]
			public void InvalidParse_ReturnsFalse_With_DefaultOutResult()
			{
				var parser = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				parser.SetPredicateForType<DateTime>(value =>
				{
					if (value != "valid")
					{
						return (false, default);
					}
					return (true, DateTime.MaxValue);
				});

				bool valid = parser.TryParseAs<DateTime>("invalid", out DateTime parsed);

				Assert.False(valid);
				Assert.Equal(default(DateTime), parsed);
			}
		}

		public class HandlesType_TypeAsParameter_Method
		{
			[Fact]
			public void NullTypeArgument_Throws()
			{
				var parser = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				Assert.Throws<ArgumentNullException>(
					() => parser.HandlesType(null));
			}

			[Fact]
			public void UnhandledType_False()
			{
				var parser = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				bool handles = parser.HandlesType(typeof(DateTime));

				Assert.False(handles);
			}

			[Fact]
			public void HandledType_True()
			{
				var parser = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				parser.SetPredicateForType<DateTime>(value => (true, DateTime.MinValue));

				bool handles = parser.HandlesType(typeof(DateTime));

				Assert.True(handles);
			}
		}

		public class HandlesType_Generic_Method
		{
			[Fact]
			public void UnhandledType_False()
			{
				var parser = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				bool handles = parser.HandlesType<DateTime>();

				Assert.False(handles);
			}

			[Fact]
			public void HandledType_True()
			{
				var parser = new Parser(
					new ParserConfig(AutoParseEnum.Never, null));

				parser.SetPredicateForType<DateTime>(value => (true, DateTime.MinValue));

				bool handles = parser.HandlesType<DateTime>();

				Assert.True(handles);
			}
		}
	}
}
