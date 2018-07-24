//using R5.RunInfoBuilder.Help;
//using R5.RunInfoBuilder.Help.Abstractions;
//using R5.RunInfoBuilder.ProcessPipeline;
//using R5.RunInfoBuilder.Tests.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;

//namespace R5.RunInfoBuilder.Tests.UnitTests
//{
//	public class HelpManagerBuilderTests
//	{
//		public class Initialization
//		{
//			[Fact]
//			public void NullKeyValidatorArgument_Throws()
//			{
//				Assert.Throws<ArgumentNullException>(() => new HelpManagerBuilder<ITestRunInfo>(null));
//			}
//		}

//		public class SetTriggerCallbackMethod
//		{
//			[Fact]
//			public void NullCallbackArgument_Throws()
//			{
//				var keyValidator = new KeyValidator();

//				var builder = new HelpManagerBuilder<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentNullException>(() => builder.SetCallback(null));
//			}

//			[Fact]
//			public void ValidCallbackArgument_SuccessfullySets()
//			{
//				var keyValidator = new KeyValidator();

//				var builder = new HelpManagerBuilder<ITestRunInfo>(keyValidator);

//				builder.SetCallback(context => throw new TestException());

//				var store = new ArgumentStore.ArgumentStore<ITestRunInfo>(keyValidator);

//				HelpManager<ITestRunInfo> manager = builder.Build(store);
				
//				Assert.Throws<TestException>(() => manager.InvokeCallback());
//			}

//			[Fact]
//			public void Invoking_ReturnsItself()
//			{
//				var keyValidator = new KeyValidator();

//				var builder = new HelpManagerBuilder<ITestRunInfo>(keyValidator);

//				IHelpManagerBuilder<ITestRunInfo> returned = builder.SetCallback(context => throw new TestException());

//				Assert.Equal(builder, returned);
//			}
//		}
//	}
//}
