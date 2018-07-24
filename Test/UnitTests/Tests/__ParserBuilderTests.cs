//using R5.RunInfoBuilder.Parser;
//using R5.RunInfoBuilder.Tests.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;

//namespace R5.RunInfoBuilder.Tests.UnitTests
//{
//	public class ParserBuilderTests
//	{
//		public class Initialization
//		{
//			[Fact]
//			public void Initializes_WithNo_TypePredicates()
//			{
//				var builder = new ParserBuilder();
//				Parser.Parser parser = builder.Build();

//				Assert.False(parser.HandlesType(typeof(bool)));

//				builder.Bool();
//				Parser.Parser postAddParser = builder.Build();

//				Assert.True(postAddParser.HandlesType(typeof(bool)));
//			}

//			[Fact]
//			public void InvokingDefaultMethod_SuccessfullyConfigures()
//			{
//				var builder = new ParserBuilder();
//				Parser.Parser parser = builder.Build();

//				Assert.False(parser.HandlesType(typeof(bool)));
//				Assert.False(parser.HandlesType(typeof(byte)));
//				Assert.False(parser.HandlesType(typeof(char)));
//				Assert.False(parser.HandlesType(typeof(DateTime)));
//				Assert.False(parser.HandlesType(typeof(decimal)));
//				Assert.False(parser.HandlesType(typeof(double)));
//				Assert.False(parser.HandlesType(typeof(int)));
//				Assert.Throws<TypeArgumentException>(() => parser.TryParseAs(typeof(TestEnum), "ValueA", out _));

//				builder.Default();
//				Parser.Parser postAddParser = builder.Build();

//				Assert.True(postAddParser.HandlesType(typeof(bool)));
//				Assert.True(postAddParser.HandlesType(typeof(byte)));
//				Assert.True(postAddParser.HandlesType(typeof(char)));
//				Assert.True(postAddParser.HandlesType(typeof(DateTime)));
//				Assert.True(postAddParser.HandlesType(typeof(decimal)));
//				Assert.True(postAddParser.HandlesType(typeof(double)));
//				Assert.True(postAddParser.HandlesType(typeof(int)));

//				bool validParse = postAddParser.TryParseAs(typeof(TestEnum), "ValueA", out object parsedEnum);
//				Assert.True(validParse);
//				Assert.Equal(TestEnum.ValueA, parsedEnum);
//			}

//			// TODO: add tests for rest of methods (eg Char(), DateTime(), etc..)
//		}
//	}
//}
