//using R5.RunInfoBuilder.Processor;
//using R5.RunInfoBuilder.Processor.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;

//namespace R5.RunInfoBuilder.UnitTests.Tests.Processor
//{
//	public class OptionTokenizerTests
//	{
//		public class TokenizeKeyConfiguration_Method
//		{
//			[Theory]
//			[InlineData("full|s", "full", 's')]
//			[InlineData(" full|s ", "full", 's')]
//			[InlineData(" full | s ", "full", 's')]
//			[InlineData("full", "full", null)]
//			[InlineData(" full ", "full", null)]
//			public void TokenizeKeyConfiguration_ReturnsCorrectResults(string input,
//				string expectedFullKey, char? expectedShortKey)
//			{
//				(string fullKey, char? shortKey) = OptionTokenizer.TokenizeKeyConfiguration(input);

//				Assert.Equal(expectedFullKey, fullKey);
//				Assert.Equal(expectedShortKey, shortKey);
//			}
//		}

//		public class TokenizeProgramArgument_Method
//		{
//			[Fact]
//			public void DoesntStartWith_SingleOrDoubleDash_Throws()
//			{
//				Assert.Throws<ArgumentException>(
//					() => OptionTokenizer.TokenizeProgramArgument("invalid"));
//			}

//			[Fact]
//			public void MoreThanOneEquals_Throws()
//			{
//				Assert.Throws<ArgumentException>(
//					() => OptionTokenizer.TokenizeProgramArgument("--inv=ali=d"));
//			}

//			[Fact]
//			public void EndsWithEquals_Throws()
//			{
//				Assert.Throws<ArgumentException>(
//					() => OptionTokenizer.TokenizeProgramArgument("--invalid="));
//			}

//			[Theory]
//			[InlineData("--=invalid")]
//			[InlineData("-=invalid")]
//			public void KeyBeginsWithEquals_Throws(string input)
//			{
//				Assert.Throws<ArgumentException>(
//					() => OptionTokenizer.TokenizeProgramArgument(input));
//			}

//			[Theory]
//			[InlineData("-aa")]
//			[InlineData("-aba")]
//			[InlineData("-aab")]
//			[InlineData("-baa")]
//			public void Stacked_ContainsDuplicates_Throws(string input)
//			{
//				Assert.Throws<ArgumentException>(
//					() => OptionTokenizer.TokenizeProgramArgument(input));
//			}

//			[Theory]
//			[InlineData("--full", OptionType.Full, "full", null)]
//			[InlineData("--full=value", OptionType.Full, "full", "value")]
//			[InlineData("-f", OptionType.Short, null, null, 'f')]
//			[InlineData("-f=value", OptionType.Short, null, "value", 'f')]
//			[InlineData("-stacked", OptionType.Stacked, null, null, 
//				's', 't', 'a', 'c', 'k', 'e', 'd')]
//			[InlineData("-stacked=value", OptionType.Stacked, null, "value",
//				's', 't', 'a', 'c', 'k', 'e', 'd')]
//			internal void ValidInput_Returns_CorrectResult(string input,
//				OptionType expectedType, string expectedFullKey, 
//				string expectedValue, params char[] expectedShortKeys)
//			{
//				(OptionType type, string fullKey, List<char> shortKeys, string value)
//					= OptionTokenizer.TokenizeProgramArgument(input);

//				Assert.Equal(expectedType, type);
//				Assert.Equal(expectedFullKey, fullKey);
//				Assert.Equal(expectedValue, value);

//				if (expectedShortKeys.Length == 0)
//				{
//					Assert.Null(shortKeys);
//				}
//				else
//				{
//					Assert.True(listsEqualByElements(expectedShortKeys, shortKeys));
//				}
				
//				bool listsEqualByElements(char[] a, List<char> l)
//				{
//					if (a.Length != l.Count)
//					{
//						return false;
//					}

//					for (int i = 0; i < a.Length; i++)
//					{
//						if (a[i] != l[i])
//						{
//							return false;
//						}
//					}

//					return true;
//				}
//			}
//		}
//	}
//}
