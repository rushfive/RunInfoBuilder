//using Moq;
//using R5.RunInfoBuilder.ArgumentStore;
//using R5.RunInfoBuilder.ArgumentStore.Abstractions;
//using R5.RunInfoBuilder.Help;
//using R5.RunInfoBuilder.Help.Models;
//using R5.RunInfoBuilder.ProcessPipeline;
//using R5.RunInfoBuilder.Tests.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Xunit;

//namespace R5.RunInfoBuilder.Tests.UnitTests
//{
//	public class HelpManagerTests
//	{
//		static (HelpManager<ITestRunInfo>, KeyValidator) GetTestObjects()
//		{
//			var validator = new KeyValidator();
//			var manager = new HelpManager<ITestRunInfo>(null, null, validator);
//			return (manager, validator);
//		}

//		public class Initialization
//		{
//			[Fact]
//			public void InitializesWithDefaultTriggers()
//			{
//				var keyValidator = new KeyValidator();
//				var manager = new HelpManager<ITestRunInfo>(null, null, keyValidator);

//				foreach (var trigger in Constants.DefaultHelpTriggers)
//				{
//					Assert.True(manager.IsTrigger(trigger));
//				}
//			}

//			[Fact]
//			public void AddsDefaultTriggers_ExceptThoseRestricted()
//			{
//				var validator = new KeyValidator();
//				validator.AddRestrictedKey("-h");

//				var manager = new HelpManager<ITestRunInfo>(null, null, validator);

//				Assert.False(manager.IsTrigger("-h"));
//			}
//		}

//		public class IsHelpTriggerMethod
//		{
//			[Fact]
//			public void ValidTrigger_ReturnsTrue()
//			{
//				var keyValidator = new KeyValidator();
//				var helper = new HelpManager<ITestRunInfo>(null, null, keyValidator);

//				foreach (var trigger in Constants.DefaultHelpTriggers)
//				{
//					Assert.True(helper.IsTrigger(trigger));
//				}
//			}

//			[Fact]
//			public void InvalidTrigger_ReturnsFalse()
//			{
//				var keyValidator = new KeyValidator();
//				var helper = new HelpManager<ITestRunInfo>(null, null, keyValidator);
//				Assert.False(helper.IsTrigger("invalid"));
//			}
//		}

//		public class AddHelpTriggerMethod
//		{
//			[Fact]
//			public void NullOrEmpty_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var helper = new HelpManager<ITestRunInfo>(null, null, keyValidator);
//				Assert.Throws<ArgumentNullException>(() => helper.AddTrigger(null));
//				Assert.Throws<ArgumentNullException>(() => helper.AddTrigger(""));
//			}

//			[Fact]
//			public void TriggerAlreadyExists_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var helper = new HelpManager<ITestRunInfo>(null, null, keyValidator);
//				Assert.Throws<ArgumentException>(() => helper.AddTrigger(Constants.DefaultHelpTriggers.First()));
//			}

//			[Fact]
//			public void Trigger_IsRestrictedKey_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				keyValidator.AddRestrictedKey("restricted");

//				var helper = new HelpManager<ITestRunInfo>(null, null, keyValidator);
//				Assert.Throws<ArgumentException>(() => helper.AddTrigger("restricted"));
//			}

//			[Fact]
//			public void Valid_SuccessfullyAdds()
//			{
//				var keyValidator = new KeyValidator();
//				var helper = new HelpManager<ITestRunInfo>(null, null, keyValidator);
//				Assert.False(helper.IsTrigger("new"));
//				helper.AddTrigger("new");
//				Assert.True(helper.IsTrigger("new"));
//			}

//			[Fact]
//			public void Valid_AddsToKeyValidator()
//			{
//				var keyValidator = new KeyValidator();

//				Assert.False(keyValidator.IsRestrictedKey("new"));

//				var helper = new HelpManager<ITestRunInfo>(null, null, keyValidator);
//				helper.AddTrigger("new");

//				Assert.True(keyValidator.IsRestrictedKey("new"));
//			}
//		}

//		public class ClearTriggersMethod
//		{
//			[Fact]
//			public void Invoking_SuccessfullyClears()
//			{
//				var keyValidator = new KeyValidator();
//				var helper = new HelpManager<ITestRunInfo>(null, null, keyValidator);

//				foreach (var trigger in Constants.DefaultHelpTriggers)
//				{
//					Assert.True(helper.IsTrigger(trigger));
//				}

//				helper.ClearTriggers();

//				foreach (var trigger in Constants.DefaultHelpTriggers)
//				{
//					Assert.False(helper.IsTrigger(trigger));
//				}
//			}

//			[Fact]
//			public void Invoking_ClearsFrom_KeyValidator()
//			{
//				var keyValidator = new KeyValidator();
//				var helper = new HelpManager<ITestRunInfo>(null, null, keyValidator);

//				foreach (var trigger in Constants.DefaultHelpTriggers)
//				{
//					Assert.True(keyValidator.IsRestrictedKey(trigger));
//				}

//				helper.ClearTriggers();

//				foreach (var trigger in Constants.DefaultHelpTriggers)
//				{
//					Assert.False(keyValidator.IsRestrictedKey(trigger));
//				}
//			}
//		}

//		public class AddProgramDescriptionMethod
//		{
//			[Fact]
//			public void NullOrEmpty_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var helper = new HelpManager<ITestRunInfo>(null, null, keyValidator);
//				Assert.Throws<ArgumentNullException>(() => helper.AddProgramDescription(null));
//				Assert.Throws<ArgumentNullException>(() => helper.AddProgramDescription(""));
//			}

//			[Fact]
//			public void Valid_SuccessfullyAdds()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				var helper = new HelpManager<ITestRunInfo>(store, null, keyValidator);

//				Assert.Null(helper.GetMetadata().ProgramDescription);

//				helper.AddProgramDescription("description");
//				Assert.NotNull(helper.GetMetadata().ProgramDescription);
				
//			}
//		}

//		public class GetFormattedMethod
//		{
//			// TODO
//		}

//		public class GetMetadataMethod
//		{
//			[Fact]
//			public void InitialManager_ReturnsCorrectResult()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				var helper = new HelpManager<ITestRunInfo>(store, null, keyValidator);

//				HelpMetadata metadata = helper.GetMetadata();

//				Assert.Null(metadata.ProgramDescription);
//				Assert.Equal(Constants.DefaultHelpTriggers.Count(), metadata.Triggers.Count);
//				Assert.True(metadata.Triggers.All(t => Constants.DefaultHelpTriggers.Contains(t)));
//				Assert.Empty(metadata.Arguments);
//				Assert.Empty(metadata.Commands);
//				Assert.Empty(metadata.Options);
//			}
//		}

//		public class InvokeCallbackMethod
//		{
//			[Fact]
//			public void NoTriggerSet_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var helper = new HelpManager<ITestRunInfo>(null, null, keyValidator);
//				Assert.Throws<InvalidOperationException>(() => helper.InvokeCallback());
//			}
//		}
//	}
//}
