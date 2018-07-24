//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;
//using R5.RunInfoBuilder.Parser;
//using R5.RunInfoBuilder.Parser.Abstractions;
//using R5.RunInfoBuilder.Tests.Models;

//namespace R5.RunInfoBuilder.Tests.UnitTests
//{
//	public class ParserTests
//	{
//		public class Initialization
//		{
//			[Fact]
//			public void TypePredicatesArgument_IsAdded()
//			{
//				var parserWithoutPredicates = new Parser.Parser(true, true, null);

//				Assert.False(parserWithoutPredicates.HandlesType(typeof(bool)));

//				var typePredicates = new Dictionary<Type, List<Func<string, (bool, object)>>>
//				{
//					{
//						typeof(bool),
//						new List<Func<string, (bool, object)>>()
//					}
//				};

//				var parserWithPredicates = new Parser.Parser(true, true, typePredicates);

//				Assert.True(parserWithPredicates.HandlesType(typeof(bool)));
//			}
//		}

//		public class ClearAllPredicatesMethod
//		{
//			[Fact]
//			public void Invoking_SuccessfullyClears()
//			{
//				var typePredicates = new Dictionary<Type, List<Func<string, (bool, object)>>>
//				{
//					{
//						typeof(bool),
//						new List<Func<string, (bool, object)>>()
//					},
//					{
//						typeof(int),
//						new List<Func<string, (bool, object)>>()
//					}
//				};

//				var parser = new Parser.Parser(true, true, typePredicates);

//				Assert.True(parser.HandlesType(typeof(bool)));
//				Assert.True(parser.HandlesType(typeof(int)));

//				parser.ClearAllPredicates();

//				Assert.False(parser.HandlesType(typeof(bool)));
//				Assert.False(parser.HandlesType(typeof(int)));
//			}

//			[Fact]
//			public void Invoking_ReturnsItself()
//			{
//				var parser = new Parser.Parser(true, true, null);

//				IParser returned = parser.ClearAllPredicates();

//				Assert.Equal(parser, returned);
//			}
//		}

//		public class ClearPredicatesForMethod
//		{
//			[Fact]
//			public void NullTypeArgument_Throws()
//			{
//				var parser = new Parser.Parser(true, true, null);

//				Assert.Throws<ArgumentNullException>(() => parser.ClearPredicatesFor(null));
//			}

//			[Fact]
//			public void Type_NotConfigured_Throws()
//			{
//				var parser = new Parser.Parser(true, true, null);

//				Assert.Throws<TypeArgumentException>(() => parser.ClearPredicatesFor(typeof(bool)));
//			}

//			[Fact]
//			public void Valid_SuccessfullyClearsPredicates()
//			{
//				var typePredicates = new Dictionary<Type, List<Func<string, (bool, object)>>>
//				{
//					{
//						typeof(bool),
//						new List<Func<string, (bool, object)>>()
//					},
//					{
//						typeof(int),
//						new List<Func<string, (bool, object)>>()
//					}
//				};

//				var parser = new Parser.Parser(true, true, typePredicates);

//				Assert.True(parser.HandlesType(typeof(bool)));
//				Assert.True(parser.HandlesType(typeof(int)));

//				parser.ClearPredicatesFor(typeof(bool));

//				Assert.False(parser.HandlesType(typeof(bool)));
//				Assert.True(parser.HandlesType(typeof(int)));
//			}

//			[Fact]
//			public void ValidInvoking_ReturnsItself()
//			{
//				var typePredicates = new Dictionary<Type, List<Func<string, (bool, object)>>>
//				{
//					{
//						typeof(bool),
//						new List<Func<string, (bool, object)>>()
//					}
//				};

//				var parser = new Parser.Parser(true, true, typePredicates);

//				IParser returned = parser.ClearPredicatesFor(typeof(bool));

//				Assert.Equal(parser, returned);
//			}
//		}

//		public class HandlesTypeMethod
//		{
//			[Fact]
//			public void NullTypeArgument_Throws()
//			{
//				var parser = new Parser.Parser(true, true, null);

//				Assert.Throws<ArgumentNullException>(() => parser.HandlesType(null));
//			}

//			[Fact]
//			public void Valid_ReturnsCorrectResults()
//			{
//				var parserWithoutPredicates = new Parser.Parser(true, true, null);

//				Assert.False(parserWithoutPredicates.HandlesType(typeof(bool)));

//				var typePredicates = new Dictionary<Type, List<Func<string, (bool, object)>>>
//				{
//					{
//						typeof(bool),
//						new List<Func<string, (bool, object)>>()
//					}
//				};

//				var parserWithPredicates = new Parser.Parser(true, true, typePredicates);

//				Assert.True(parserWithPredicates.HandlesType(typeof(bool)));
//			}
//		}

//		public class AddTypeParserPredicateMethod
//		{
//			[Fact]
//			public void NullPredicateArgument_Throws()
//			{
//				var parser = new Parser.Parser(true, true, null);

//				Assert.Throws<ArgumentNullException>(() => parser.AddTypeParserPredicate<bool>(null));
//			}

//			[Fact]
//			public void Valid_SuccessfullyAddsPredicate()
//			{
//				var parser = new Parser.Parser(true, true, null);

//				Assert.False(parser.HandlesType(typeof(bool)));

//				parser.AddTypeParserPredicate<bool>(val => (true, val));

//				Assert.True(parser.HandlesType(typeof(bool)));
//			}

//			[Fact]
//			public void ValidInvoke_ReturnsItself()
//			{
//				var parser = new Parser.Parser(true, true, null);

//				IParser returned = parser.AddTypeParserPredicate<bool>(val => (true, val));

//				Assert.Equal(parser, returned);
//			}
//		}

//		public class TryParseAsMethod
//		{
//			[Fact]
//			public void NullTypeArgument_Throws()
//			{
//				var parser = new Parser.Parser(true, true, null);

//				Assert.Throws<ArgumentNullException>(() => parser.TryParseAs(null, "value", out _));
//			}

//			[Fact]
//			public void AutoParseEnumsTrue_EnumParseIgnoresCase_ValidValue_ReturnsCorrectResult()
//			{
//				var parser = new Parser.Parser(true, true, null);

//				bool result1 = parser.TryParseAs(typeof(TestEnum), "ValueA", out object parsedA);

//				Assert.True(result1);
//				Assert.Equal(TestEnum.ValueA, parsedA);

//				bool result2 = parser.TryParseAs(typeof(TestEnum), "valuea", out object parsedB);

//				Assert.True(result2);
//				Assert.Equal(TestEnum.ValueA, parsedB);
//			}

//			[Fact]
//			public void AutoParseEnumsTrue_EnumParseIgnoresCase_InvalidValue_ReturnsCorrectResult()
//			{
//				var parser = new Parser.Parser(true, true, null);

//				bool result = parser.TryParseAs(typeof(TestEnum), "InvalidValue", out object parsed);

//				Assert.False(result);
//				Assert.NotEqual(TestEnum.ValueA, parsed);
//			}

//			[Fact]
//			public void AutoParseEnumsTrue_EnumParseDoesntIgnoresCase_ValidValue_ReturnsCorrectResult()
//			{
//				var parser = new Parser.Parser(true, false, null);

//				bool result1 = parser.TryParseAs(typeof(TestEnum), "ValueA", out object parsedA);

//				Assert.True(result1);
//				Assert.Equal(TestEnum.ValueA, parsedA);

//				bool result2 = parser.TryParseAs(typeof(TestEnum), "valuea", out object parsedB);

//				Assert.False(result2);
//				Assert.NotEqual(TestEnum.ValueA, parsedB);
//			}

//			[Fact]
//			public void AutoParseEnumsTrue_EnumParseDoesntIgnoresCase_InvalidValue_ReturnsCorrectResult()
//			{
//				var parser = new Parser.Parser(true, false, null);

//				bool result = parser.TryParseAs(typeof(TestEnum), "InvalidValue", out object parsed);

//				Assert.False(result);
//				Assert.NotEqual(TestEnum.ValueA, parsed);
//			}

//			[Fact]
//			public void TypeNotHandled_Throws()
//			{
//				var parser = new Parser.Parser(false, false, null);

//				Assert.Throws<TypeArgumentException>(() => parser.TryParseAs(typeof(bool), "value", out object parsed));
//			}

//			[Fact]
//			public void SinglePredicate_ValuePasses_ReturnsTrue()
//			{
//				var parser = new Parser.Parser(false, false, null);

//				parser.AddTypeParserPredicate<bool>(value =>
//				{
//					if (value == "expected")
//					{
//						return (true, true);
//					}
//					else
//					{
//						return (false, null);
//					}
//				});

//				bool result = parser.TryParseAs(typeof(bool), "expected", out object parsed);

//				Assert.True(result);
//				Assert.True((bool)parsed);
//			}

//			[Fact]
//			public void SinglePredicate_ValueNotPasses_ReturnsFalse()
//			{
//				var parser = new Parser.Parser(false, false, null);

//				parser.AddTypeParserPredicate<bool>(value =>
//				{
//					if (value == "expected")
//					{
//						return (true, true);
//					}
//					else
//					{
//						return (false, null);
//					}
//				});

//				bool result = parser.TryParseAs(typeof(bool), "not_expected", out object parsed);

//				Assert.False(result);
//			}

//			[Fact]
//			public void TwoPredicates_ValuePassesBoth_ReturnsTrue()
//			{
//				var parser = new Parser.Parser(false, false, null);

//				parser
//					.AddTypeParserPredicate<bool>(value =>
//					{
//						if (value == "expected")
//						{
//							return (true, true);
//						}
//						else
//						{
//							return (false, null);
//						}
//					})
//					.AddTypeParserPredicate<bool>(value =>
//					{
//						if (value.Contains("pect"))
//						{
//							return (true, true);
//						}
//						else
//						{
//							return (false, null);
//						}
//					});

//				bool result = parser.TryParseAs(typeof(bool), "expected", out object parsed);

//				Assert.True(result);
//				Assert.True((bool)parsed);
//			}

//			[Fact]
//			public void TwoPredicates_ValueNotPassesFirst_ReturnsFalse()
//			{
//				var parser = new Parser.Parser(false, false, null);

//				parser
//					.AddTypeParserPredicate<bool>(value =>
//					{
//						if (value == "expected")
//						{
//							return (true, true);
//						}
//						else
//						{
//							return (false, null);
//						}
//					})
//					.AddTypeParserPredicate<bool>(value =>
//					{
//						if (value.Contains("xxx"))
//						{
//							return (true, true);
//						}
//						else
//						{
//							return (false, null);
//						}
//					});

//				bool result = parser.TryParseAs(typeof(bool), "xxxxx", out object parsed);

//				Assert.False(result);
//			}

//			[Fact]
//			public void TwoPredicates_ValueNotPassesSecond_ReturnsFalse()
//			{
//				var parser = new Parser.Parser(false, false, null);

//				parser
//					.AddTypeParserPredicate<bool>(value =>
//					{
//						if (value == "expected")
//						{
//							return (true, true);
//						}
//						else
//						{
//							return (false, null);
//						}
//					})
//					.AddTypeParserPredicate<bool>(value =>
//					{
//						if (value.Contains("xxx"))
//						{
//							return (true, true);
//						}
//						else
//						{
//							return (false, null);
//						}
//					});

//				bool result = parser.TryParseAs(typeof(bool), "expected", out object parsed);

//				Assert.False(result);
//			}

//			[Fact]
//			public void MultiplePredicates_ValuePassesAll_ResolvedValueFromLastPredicate()
//			{
//				var parser = new Parser.Parser(false, false, null);

//				parser
//					.AddTypeParserPredicate<string>(value =>
//					{
//						if (value.Contains("aaa"))
//						{
//							return (true, "first_value");
//						}
//						else
//						{
//							return (false, null);
//						}
//					})
//					.AddTypeParserPredicate<string>(value =>
//					{
//						if (value.Contains("aaa"))
//						{
//							return (true, "second_value");
//						}
//						else
//						{
//							return (false, null);
//						}
//					})
//					.AddTypeParserPredicate<string>(value =>
//					{
//						if (value.Contains("aaa"))
//						{
//							return (true, "third_value");
//						}
//						else
//						{
//							return (false, null);
//						}
//					});

//				bool result = parser.TryParseAs(typeof(string), "aaa", out object parsed);

//				Assert.True(result);
//				Assert.Equal("third_value", (string)parsed);
//			}
//		}
//	}
//}
