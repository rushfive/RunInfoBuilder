//using R5.RunInfoBuilder.Configuration;
//using R5.RunInfoBuilder.FunctionalTests.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;

//namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing
//{
//	public class UnresolvedArgumentHandlingTests
//	{
//		[Fact]
//		public void ConfiguredToThrow_OnUnresolved_Throws()
//		{
//			var runInfo = new TestRunInfo();

//			var setup = new BuilderSetup<TestRunInfo>();

//			setup.UseImplementation(runInfo);
//			setup.Process.AllowUnresolvedArgumentsButThrowOnProcess();

//			var builder = setup.Create();

//			builder.Store.AddOption("option", ri => ri.Bool1);

//			Assert.False(runInfo.Bool1);

//			Assert.Throws<InvalidOperationException>(
//				() => builder.Build(new string[] { "unresolved", "--option" }));

//			Assert.False(runInfo.Bool1);
//		}

//		[Fact]
//		public void ConfiguredToSkip_OnUnresolved_Skips()
//		{
//			var runInfo = new TestRunInfo();

//			var setup = new BuilderSetup<TestRunInfo>();

//			setup.UseImplementation(runInfo);
//			setup.Process.AllowUnresolvedArgumentsButSkipOnProcess();

//			var builder = setup.Create();

//			builder.Store.AddOption("option", ri => ri.Bool1);

//			Assert.False(runInfo.Bool1);

//			builder.Build(new string[] { "unresolved", "--option" });

//			Assert.True(runInfo.Bool1);
//		}
//	}
//}
