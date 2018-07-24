//using R5.RunInfoBuilder.Tests.Models;
//using R5.RunInfoBuilder.Version;
//using R5.RunInfoBuilder.Version.Abstractions;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;

//namespace R5.RunInfoBuilder.Tests.UnitTests
//{
//	public class VersionManagerTests
//	{
//		static (VersionManager, KeyValidator) GetTestObjects()
//		{
//			var validator = new KeyValidator();
//			var manager = new VersionManager(validator);
//			return (manager, validator);
//		}

//		public class Initialization
//		{
//			[Fact]
//			public void Initializes_WithCorrect_DefaultTriggers()
//			{
//				var (manager, validator) = GetTestObjects();

//				foreach(string defaultTrigger in Constants.DefaultVersionTriggers)
//				{
//					Assert.True(manager.IsTrigger(defaultTrigger));
//				}
//			}
//		}

//		public class IsEnabledMethod
//		{
//			[Fact]
//			public void ReturnsCorrectResult()
//			{
//				var (manager, validator) = GetTestObjects();

//				Assert.False(manager.IsEnabled());

//				manager.SetCallback(() => { });

//				Assert.True(manager.IsEnabled());
//			}
//		}

//		public class IsVersionTriggerMethod
//		{
//			[Fact]
//			public void NullTriggerArgument_Throws()
//			{
//				var (manager, validator) = GetTestObjects();
//				Assert.Throws<ArgumentNullException>(() => manager.IsTrigger(null));
//			}

//			[Fact]
//			public void ValidArgument_ReturnsCorrectResult()
//			{
//				var (manager, validator) = GetTestObjects();

//				Assert.False(manager.IsTrigger("--newtrigger"));

//				manager.AddTrigger("--newtrigger");

//				Assert.True(manager.IsTrigger("--newtrigger"));
//			}
//		}

//		public class InvokeCallbackMethod
//		{
//			[Fact]
//			public void CallbackNotSet_Throws()
//			{
//				var (manager, validator) = GetTestObjects();
//				Assert.Throws<InvalidOperationException>(() => manager.InvokeCallback());
//			}

//			[Fact]
//			public void CallbackSet_SuccessfullyFires()
//			{
//				var (manager, validator) = GetTestObjects();

//				manager.SetCallback(() => throw new TestException());

//				Assert.Throws<TestException>(() => manager.InvokeCallback());
//			}
//		}

//		public class AddVersionTriggerMethod
//		{
//			[Fact]
//			public void NullTriggerArgument_Throws()
//			{
//				var (manager, validator) = GetTestObjects();
//				Assert.Throws<ArgumentNullException>(() => manager.AddTrigger(null));
//				Assert.Throws<ArgumentNullException>(() => manager.AddTrigger(""));
//			}

//			[Fact]
//			public void TriggerAlreadyConfigured_Throws()
//			{
//				var (manager, validator) = GetTestObjects();

//				manager.AddTrigger("--newtrigger");

//				Assert.Throws<ArgumentException>(() => manager.AddTrigger("--newtrigger"));
//			}

//			[Fact]
//			public void Trigger_IsRestrictedKey_Throws()
//			{
//				var (manager, validator) = GetTestObjects();

//				validator.AddRestrictedKey("--newtrigger");

//				Assert.Throws<ArgumentException>(() => manager.AddTrigger("--newtrigger"));
//			}

//			[Fact]
//			public void ValidTrigger_SuccessfullyAdds()
//			{
//				var (manager, validator) = GetTestObjects();

//				manager.AddTrigger("--newtrigger");

//				Assert.True(manager.IsTrigger("--newtrigger"));
//				Assert.True(validator.IsRestrictedKey("--newtrigger"));
//			}

//			[Fact]
//			public void ValidInvoke_ReturnsItself()
//			{
//				var (manager, validator) = GetTestObjects();

//				IVersionManager returned = manager.AddTrigger("--newtrigger");

//				Assert.Equal(manager, returned);
//			}
//		}

//		public class ClearVersionTriggersMethod
//		{
//			[Fact]
//			public void Invoking_ClearsKeys()
//			{
//				// restricted too
//				var (manager, validator) = GetTestObjects();

//				var keys = new List<string> { "a", "b", "c" };
//				manager.AddTrigger(keys[0]);
//				manager.AddTrigger(keys[1]);
//				manager.AddTrigger(keys[2]);

//				foreach(string key in keys)
//				{
//					Assert.True(manager.IsTrigger(key));
//					Assert.True(validator.IsRestrictedKey(key));
//				}

//				manager.ClearTriggers();

//				foreach (string key in keys)
//				{
//					Assert.False(manager.IsTrigger(key));
//					Assert.False(validator.IsRestrictedKey(key));
//				}
//			}

//			[Fact]
//			public void Invoking_ReturnsItself()
//			{
//				var (manager, validator) = GetTestObjects();

//				IVersionManager returned = manager.ClearTriggers();

//				Assert.Equal(manager, returned);

//			}
//		}

//		public class SetVersionCallbackMethod
//		{
//			[Fact]
//			public void NullCallbackArgument_Throws()
//			{
//				var (manager, validator) = GetTestObjects();
//				Assert.Throws<ArgumentNullException>(() => manager.SetCallback(null));
//			}

//			[Fact]
//			public void Callback_AlreadyConfigured_Throws()
//			{
//				var (manager, validator) = GetTestObjects();

//				manager.SetCallback(() => { });

//				Assert.Throws<InvalidOperationException>(() => manager.SetCallback(() => { }));
//			}

//			[Fact]
//			public void ValidCallbackArgument_SuccessfullySets()
//			{
//				var (manager, validator) = GetTestObjects();

//				manager.SetCallback(() => throw new TestException());

//				Assert.Throws<TestException>(() => manager.InvokeCallback());
//			}

//			[Fact]
//			public void ValidInvoke_ReturnsItself()
//			{
//				var (manager, validator) = GetTestObjects();

//				IVersionManager returned = manager.SetCallback(() => throw new TestException());

//				Assert.Equal(manager, returned);
//			}
//		}
//	}
//}
