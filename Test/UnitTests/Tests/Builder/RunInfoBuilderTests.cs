using R5.RunInfoBuilder.UnitTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.UnitTests.Tests.Builder
{
	public class RunInfoBuilderTests
	{
		public class PublicConstructor
		{
			[Fact]
			public void InitializesProperties_InCorrectState()
			{
				var builder = new RunInfoBuilder<TestRunInfo>();

				Assert.NotNull(builder.Parser);
				Assert.NotNull(builder.Store);

				// reflection to check other private props
			}
		}
	}
}
