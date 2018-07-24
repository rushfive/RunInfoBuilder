using R5.RunInfoBuilder.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.UnitTests.Tests.Validators
{
	public class ValidationHelperTests
	{
		public class IsPermutation
		{
			[Theory]
			[InlineData("hi", "i", false)]
			[InlineData("hi", "ab", false)]
			[InlineData("hi", "hi", true)]
			[InlineData("hi", "ih", true)]
			[InlineData("hi", "abc", false)]
			[InlineData("hi", "hji", true)]
			[InlineData("hi", "hij", true)]
			[InlineData("hi", "jhi", true)]
			public void ReturnsCorrectResults(string token, string chars, bool isPermutation)
			{
				var helper = new ValidationHelper<object>(null);

				bool result = helper.IsPermutation(token, chars);

				Assert.Equal(isPermutation, result);
			}
		}
		
	}
}
