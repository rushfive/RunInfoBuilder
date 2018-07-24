//using R5.RunInfoBuilder.ArgumentStore;
//using R5.RunInfoBuilder.ArgumentStore.Abstractions;
//using R5.RunInfoBuilder.ArgumentStore.Models;
//using R5.RunInfoBuilder.Help.Models;
//using R5.RunInfoBuilder.Models;
//using R5.RunInfoBuilder.ProcessPipeline;
//using R5.RunInfoBuilder.Tests.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using Xunit;

//namespace R5.RunInfoBuilder.Tests.UnitTests
//{
//    public class ArgumentStoreTests
//    {
//		public class AddOption_ParamsList_Method
//		{
//			[Fact]
//			public void FullOptionNull_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentNullException>(() => store.AddOption(null, null));
//			}

//			[Fact]
//			public void PropertyExpressionNull_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentNullException>(() => store.AddOption("option", null));
//			}

//			[Fact]
//			public void FullOption_AlreadyConfigured_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				store.AddOption("option", ri => ri.Bool1);
//				Assert.Throws<ArgumentException>(() => store.AddOption("option", ri => ri.Bool1));
//			}

//			[Fact]
//			public void FullOption_IsRestricted_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				keyValidator.AddRestrictedKey("restricted");

//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.Throws<ArgumentException>(() => store.AddOption("restricted", ri => ri.Bool1));
//			}

//			[Fact]
//			public void ShortOption_AlreadyConfigured_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				store.AddOption("option", ri => ri.Bool1, 'o');
//				Assert.Throws<ArgumentException>(() => store.AddOption("option2", ri => ri.Bool1, 'o'));
//			}

//			[Fact]
//			public void ShortOption_IsRestricted_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				keyValidator.AddRestrictedKey('r');

//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.Throws<ArgumentException>(() => store.AddOption("restricted", ri => ri.Bool1, 'r'));
//			}

//			[Fact]
//			public void PropertyUnwritable_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentException>(() => store.AddOption("option", ri => ri.UnwritableBool));
//			}

//			[Fact]
//			public void ValidFullKey_SuccessfullyAdds()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.False(store.IsValidFullOption("option"));

//				store.AddOption("option", ri => ri.Bool1);

//				Assert.True(store.IsValidFullOption("option"));
//			}

//			[Fact]
//			public void ValidShortKey_SuccessfullyAdds()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.False(store.IsValidShortOption('o'));

//				store.AddOption("option", ri => ri.Bool1, 'o');

//				Assert.True(store.IsValidShortOption('o'));
//			}

//			[Fact]
//			public void ValidAdd_SuccessfullyAdds_HelpInfo()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				HelpMetadata initialHelpMetadata = store.GetHelpMetadata();

//				Assert.Empty(initialHelpMetadata.Options);

//				store.AddOption("option", ri => ri.Bool1);

//				HelpMetadata postAddHelpMetadata = store.GetHelpMetadata();

//				Assert.Equal("option", postAddHelpMetadata.Options.Single().FullKey);
//			}

//			[Fact]
//			public void ValidAdd_ReturnsItself()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				IArgumentStore<ITestRunInfo> returned = store.AddOption("option", ri => ri.Bool1);

//				Assert.Equal(store, returned);
//			}

//			[Fact]
//			public void ValidAdd_AddsTo_KeyValidator()
//			{
//				var keyValidator = new KeyValidator();

//				Assert.False(keyValidator.IsRestrictedKey("restricted"));
//				Assert.False(keyValidator.IsRestrictedKey('r'));

//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				store.AddOption("restricted", ri => ri.Bool1, 'r');

//				Assert.True(keyValidator.IsRestrictedKey("restricted"));
//				Assert.True(keyValidator.IsRestrictedKey('r'));
//			}
//		}

//		public class AddOption_ConfigObject_Method
//		{
//			[Fact]
//			public void ConfigObjectNull_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentNullException>(() => store.AddOption(null));
//			}

//			[Fact]
//			public void FullOptionNull_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentNullException>(() => store.AddOption(new Option<ITestRunInfo>
//				{
//					FullKey = null,
//					PropertyExpression = null
//				}));
//			}

//			[Fact]
//			public void PropertyExpressionNull_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentNullException>(() => store.AddOption(new Option<ITestRunInfo>
//				{
//					FullKey = "option",
//					PropertyExpression = null
//				}));
//			}

//			[Fact]
//			public void FullOption_AlreadyConfigured_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				store.AddOption("option", ri => ri.Bool1);
//				Assert.Throws<ArgumentException>(() => store.AddOption(new Option<ITestRunInfo>
//				{
//					FullKey = "option",
//					PropertyExpression = null
//				}));
//			}

//			[Fact]
//			public void FullOption_IsRestricted_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				keyValidator.AddRestrictedKey("restricted");

//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.Throws<ArgumentException>(() => store.AddOption(new Option<ITestRunInfo>
//				{
//					FullKey = "restricted",
//					PropertyExpression = null
//				}));
//			}

//			[Fact]
//			public void ShortOption_IsRestricted_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				keyValidator.AddRestrictedKey('r');

//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.Throws<ArgumentException>(() => store.AddOption(new Option<ITestRunInfo>
//				{
//					FullKey = "restricted",
//					PropertyExpression = ri => ri.Bool1,
//					ShortKey = 'r'
//				}));
//			}

//			[Fact]
//			public void ShortOption_AlreadyConfigured_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				store.AddOption("option", ri => ri.Bool1, 'o');
//				Assert.Throws<ArgumentException>(() => store.AddOption(new Option<ITestRunInfo>
//				{
//					FullKey = "option2",
//					PropertyExpression = ri => ri.Bool1,
//					ShortKey = 'o'
//				}));
//			}

//			[Fact]
//			public void PropertyUnwritable_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentException>(() => store.AddOption(new Option<ITestRunInfo>
//				{
//					FullKey = "option",
//					PropertyExpression = ri => ri.UnwritableBool
//				}));
//			}

//			[Fact]
//			public void ValidFullKey_SuccessfullyAdds()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.False(store.IsValidFullOption("option"));

//				store.AddOption(new Option<ITestRunInfo>
//				{
//					FullKey = "option",
//					PropertyExpression = ri => ri.Bool1
//				});

//				Assert.True(store.IsValidFullOption("option"));
//			}

//			[Fact]
//			public void ValidShortKey_SuccessfullyAdds()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.False(store.IsValidShortOption('o'));

//				store.AddOption(new Option<ITestRunInfo>
//				{
//					FullKey = "option",
//					PropertyExpression = ri => ri.Bool1,
//					ShortKey = 'o'
//				});

//				Assert.True(store.IsValidShortOption('o'));
//			}

//			[Fact]
//			public void ValidAdd_SuccessfullyAdds_HelpInfo()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				HelpMetadata initialHelpMetadata = store.GetHelpMetadata();

//				Assert.Empty(initialHelpMetadata.Options);

//				store.AddOption(new Option<ITestRunInfo>
//				{
//					FullKey = "option",
//					PropertyExpression = ri => ri.Bool1
//				});

//				HelpMetadata postAddHelpMetadata = store.GetHelpMetadata();

//				Assert.Equal("option", postAddHelpMetadata.Options.Single().FullKey);
//			}

//			[Fact]
//			public void ValidAdd_ReturnsItself()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
				
//				IArgumentStore<ITestRunInfo> returned = store.AddOption(new Option<ITestRunInfo>
//				{
//					FullKey = "option",
//					PropertyExpression = ri => ri.Bool1
//				});

//				Assert.Equal(store, returned);
//			}

//			[Fact]
//			public void ValidAdd_AddsTo_KeyValidator()
//			{
//				var keyValidator = new KeyValidator();

//				Assert.False(keyValidator.IsRestrictedKey("restricted"));
//				Assert.False(keyValidator.IsRestrictedKey('r'));

//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				store.AddOption(new Option<ITestRunInfo>
//				{
//					FullKey = "restricted",
//					PropertyExpression = ri => ri.Bool1,
//					ShortKey = 'r'
//				});

//				Assert.True(keyValidator.IsRestrictedKey("restricted"));
//				Assert.True(keyValidator.IsRestrictedKey('r'));
//			}
//		}

//		public class AddArgument_ParamsList_Method
//		{
//			[Fact]
//			public void ArgumentKeyNull_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentNullException>(() => store.AddArgument<bool>(null, null));
//			}

//			[Fact]
//			public void PropertyExpressionNull_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentNullException>(() => store.AddArgument<bool>("argument", null));
//			}

//			[Fact]
//			public void ArgumentKey_AlreadyConfigured_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				store.AddArgument<bool>("argument", ri => ri.Bool1);
//				Assert.Throws<ArgumentException>(() => store.AddArgument<bool>("argument", ri => ri.Bool1));
//			}

//			[Fact]
//			public void ValidatorFunctionNull_But_ValidatorDescriptionProvided_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentException>(() =>
//					store.AddArgument<bool>("argument", ri => ri.Bool1, validatorDescription: "validator description"));
//			}

//			[Fact]
//			public void PropertyUnwritable_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentException>(() => store.AddArgument<bool>(
//					"argument", ri => ri.UnwritableBool));
//			}

//			[Fact]
//			public void Valid_SuccessfullyAdds()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.False(store.IsValidArgument("argument"));

//				store.AddArgument<bool>("argument", ri => ri.Bool1);

//				Assert.True(store.IsValidArgument("argument"));
//			}

//			[Fact]
//			public void ValidAdd_AddsHelpInfo()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				HelpMetadata initialHelpMetadata = store.GetHelpMetadata();

//				Assert.Empty(initialHelpMetadata.Arguments);

//				store.AddArgument<bool>(
//					"argument", 
//					ri => ri.Bool1,
//					obj => true,
//					"validator description",
//					"argument description");

//				HelpMetadata postAddHelpMetadata = store.GetHelpMetadata();

//				ArgumentHelpInfo helpInfo = postAddHelpMetadata.Arguments.Single();

//				Assert.Equal("argument", helpInfo.Key);
//				Assert.Equal("validator description", helpInfo.ValidatorDescription);
//				Assert.Equal("argument description", helpInfo.Description);
//			}

//			[Fact]
//			public void ValidAdd_ReturnsItself()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				IArgumentStore<ITestRunInfo> returned = store.AddArgument<bool>(
//					"argument",
//					ri => ri.Bool1,
//					obj => true,
//					"validator description",
//					"argument description");

//				Assert.Equal(store, returned);
//			}

//			[Fact]
//			public void KeyArgument_IsRestricted_Throws()
//			{
//				var validator = new KeyValidator();
//				validator.AddRestrictedKey("restricted");

//				var store = new ArgumentStore<ITestRunInfo>(validator);
//				Assert.Throws<ArgumentException>(() => store.AddArgument<bool>("restricted", ri => ri.Bool1));
//			}

//			[Fact]
//			public void ValidAdd_AddsTo_KeyValidator()
//			{
//				var validator = new KeyValidator();

//				Assert.False(validator.IsRestrictedKey("new"));

//				var store = new ArgumentStore<ITestRunInfo>(validator);
//				store.AddArgument<bool>("new", ri => ri.Bool1);

//				Assert.True(validator.IsRestrictedKey("new"));
//			}
//		}

//		public class AddArgument_ConfigObject_Method
//		{
//			[Fact]
//			public void NullConfigObject_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentNullException>(() => store.AddArgument<bool>(null));
//			}

//			[Fact]
//			public void ArgumentKeyNull_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentNullException>(() => store.AddArgument<bool>(new Argument<ITestRunInfo, bool>
//				{
//					Key = null,
//					PropertyExpression = null
//				}));
//			}

//			[Fact]
//			public void PropertyExpressionNull_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentNullException>(() => store.AddArgument<bool>(new Argument<ITestRunInfo, bool>
//				{
//					Key = "argument",
//					PropertyExpression = null
//				}));
//			}

//			[Fact]
//			public void ArgumentKey_AlreadyConfigured_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				store.AddArgument<bool>(new Argument<ITestRunInfo, bool>
//				{
//					Key = "argument",
//					PropertyExpression = ri => ri.Bool1
//				});

//				Assert.Throws<ArgumentException>(() => store.AddArgument<bool>(new Argument<ITestRunInfo, bool>
//				{
//					Key = "argument",
//					PropertyExpression = ri => ri.Bool1
//				}));
//			}

//			[Fact]
//			public void ValidatorFunctionNull_But_ValidatorDescriptionProvided_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentException>(() =>
//					store.AddArgument<bool>(new Argument<ITestRunInfo, bool>
//					{
//						Key = "argument",
//						PropertyExpression = ri => ri.Bool1,
//						ValidatorDescription = "validator description"
//					}));
//			}

//			[Fact]
//			public void PropertyUnwritable_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentException>(() => store.AddArgument<bool>(new Argument<ITestRunInfo, bool>
//				{
//					Key = "argument",
//					PropertyExpression = ri => ri.UnwritableBool
//				}));
//			}

//			[Fact]
//			public void Valid_SuccessfullyAdds()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.False(store.IsValidArgument("argument"));

//				store.AddArgument<bool>(new Argument<ITestRunInfo, bool>
//				{
//					Key = "argument",
//					PropertyExpression = ri => ri.Bool1
//				});

//				Assert.True(store.IsValidArgument("argument"));
//			}

//			[Fact]
//			public void ValidAdd_AddsHelpInfo()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				HelpMetadata initialHelpMetadata = store.GetHelpMetadata();

//				Assert.Empty(initialHelpMetadata.Arguments);
				
//				store.AddArgument<bool>(new Argument<ITestRunInfo, bool>
//				{
//					Key = "argument",
//					PropertyExpression = ri => ri.Bool1,
//					ValidateFunction = obj => true,
//					ValidatorDescription = "validator description",
//					Description = "argument description"
//				});

//				HelpMetadata postAddHelpMetadata = store.GetHelpMetadata();

//				ArgumentHelpInfo helpInfo = postAddHelpMetadata.Arguments.Single();

//				Assert.Equal("argument", helpInfo.Key);
//				Assert.Equal("validator description", helpInfo.ValidatorDescription);
//				Assert.Equal("argument description", helpInfo.Description);
//			}

//			[Fact]
//			public void ValidAdd_ReturnsItself()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
				
//				IArgumentStore<ITestRunInfo> returned = store.AddArgument<bool>(new Argument<ITestRunInfo, bool>
//				{
//					Key = "argument",
//					PropertyExpression = ri => ri.Bool1,
//					ValidateFunction = obj => true,
//					ValidatorDescription = "validator description",
//					Description = "argument description"
//				});

//				Assert.Equal(store, returned);
//			}

//			[Fact]
//			public void KeyArgument_IsRestricted_Throws()
//			{
//				var validator = new KeyValidator();
//				validator.AddRestrictedKey("restricted");

//				var store = new ArgumentStore<ITestRunInfo>(validator);
//				Assert.Throws<ArgumentException>(() => store.AddArgument<bool>(new Argument<ITestRunInfo, bool>
//				{
//					Key = "restricted",
//					PropertyExpression = ri => ri.Bool1
//				}));
//			}

//			[Fact]
//			public void ValidAdd_AddsTo_KeyValidator()
//			{
//				var validator = new KeyValidator();

//				Assert.False(validator.IsRestrictedKey("new"));

//				var store = new ArgumentStore<ITestRunInfo>(validator);
//				store.AddArgument<bool>(new Argument<ITestRunInfo, bool>
//				{
//					Key = "new",
//					PropertyExpression = ri => ri.Bool1
//				});

//				Assert.True(validator.IsRestrictedKey("new"));
//			}
//		}

//		public class AddCommand_ParamsList_Method
//		{
//			[Fact]
//			public void CommandKeyNull_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentNullException>(() => store.AddCommand(null, null));
//			}

//			[Fact]
//			public void CallbackNull_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentNullException>(() => store.AddCommand("command", null));
//			}

//			[Fact]
//			public void CommandKey_AlreadyConfigured_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				store.AddCommand("command", context => ProcessStageResult.Success());
//				Assert.Throws<ArgumentException>(() => store.AddCommand("command", context => ProcessStageResult.Success()));
//			}

//			[Fact]
//			public void ValidCommandKey_SuccessfullyAdds()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.False(store.IsValidCommand("command"));

//				store.AddCommand("command", context => ProcessStageResult.Success());

//				Assert.True(store.IsValidCommand("command"));
//			}

//			[Fact]
//			public void ValidAdd_AddsHelpInfo()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				HelpMetadata initialHelpMetadata = store.GetHelpMetadata();
//				Assert.Empty(initialHelpMetadata.Commands);

//				store.AddCommand(
//					"command", 
//					context => ProcessStageResult.Success(),
//					"description");

//				HelpMetadata postAddHelpMetadata = store.GetHelpMetadata();
//				CommandHelpInfo commandInfo = postAddHelpMetadata.Commands.Single();

//				Assert.Equal("command", commandInfo.Key);
//				Assert.Equal("description", commandInfo.Description);
//			}

//			[Fact]
//			public void ValidAdd_ReturnsItself()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				IArgumentStore<ITestRunInfo> returned = store.AddCommand(
//					"command",
//					context => ProcessStageResult.Success(),
//					"description");

//				Assert.Equal(store, returned);
//			}

//			[Fact]
//			public void CommandKey_IsRestricted_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				keyValidator.AddRestrictedKey("restricted");

//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.Throws<ArgumentException>(() => store.AddCommand("restricted", context => ProcessStageResult.Success()));
//			}

//			[Fact]
//			public void ValidAdd_AddsTo_KeyValidator()
//			{
//				var keyValidator = new KeyValidator();

//				Assert.False(keyValidator.IsRestrictedKey("command"));

//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				store.AddCommand("command", context => ProcessStageResult.Success());

//				Assert.True(keyValidator.IsRestrictedKey("command"));
//			}
//		}

//		public class AddCommand_ConfigObject_Method
//		{
//			[Fact]
//			public void ConfigObjectNull_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentNullException>(() => store.AddCommand(null));
//			}

//			[Fact]
//			public void CommandKeyNull_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentNullException>(() => store.AddCommand(new Command<ITestRunInfo>
//				{
//					Key = null,
//					Callback = null
//				}));
//			}

//			[Fact]
//			public void CallbackNull_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentNullException>(() => store.AddCommand(new Command<ITestRunInfo>
//				{
//					Key = "command",
//					Callback = null
//				}));
//			}

//			[Fact]
//			public void CommandKey_AlreadyConfigured_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				store.AddCommand(new Command<ITestRunInfo>
//				{
//					Key = "command",
//					Callback = context => ProcessStageResult.Success()
//				});

//				Assert.Throws<ArgumentException>(() => store.AddCommand(new Command<ITestRunInfo>
//				{
//					Key = "command",
//					Callback = context => ProcessStageResult.Success()
//				}));
//			}

//			[Fact]
//			public void ValidCommandKey_SuccessfullyAdds()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.False(store.IsValidCommand("command"));

//				store.AddCommand(new Command<ITestRunInfo>
//				{
//					Key = "command",
//					Callback = context => ProcessStageResult.Success()
//				});

//				Assert.True(store.IsValidCommand("command"));
//			}

//			[Fact]
//			public void ValidAdd_AddsHelpInfo()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				HelpMetadata initialHelpMetadata = store.GetHelpMetadata();
//				Assert.Empty(initialHelpMetadata.Commands);
				
//				store.AddCommand(new Command<ITestRunInfo>
//				{
//					Key = "command",
//					Callback = context => ProcessStageResult.Success(),
//					Description = "description"
//				});

//				HelpMetadata postAddHelpMetadata = store.GetHelpMetadata();
//				CommandHelpInfo commandInfo = postAddHelpMetadata.Commands.Single();

//				Assert.Equal("command", commandInfo.Key);
//				Assert.Equal("description", commandInfo.Description);
//			}

//			[Fact]
//			public void ValidAdd_ReturnsItself()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
				
//				IArgumentStore<ITestRunInfo> returned = store.AddCommand(new Command<ITestRunInfo>
//				{
//					Key = "command",
//					Callback = context => ProcessStageResult.Success(),
//					Description = "description"
//				});

//				Assert.Equal(store, returned);
//			}

//			[Fact]
//			public void CommandKey_IsRestricted_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				keyValidator.AddRestrictedKey("restricted");

//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.Throws<ArgumentException>(() => store.AddCommand(new Command<ITestRunInfo>
//				{
//					Key = "restricted",
//					Callback = context => ProcessStageResult.Success()
//				}));
//			}

//			[Fact]
//			public void ValidAdd_AddsTo_KeyValidator()
//			{
//				var keyValidator = new KeyValidator();

//				Assert.False(keyValidator.IsRestrictedKey("command"));

//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				store.AddCommand(new Command<ITestRunInfo>
//				{
//					Key = "command",
//					Callback = context => ProcessStageResult.Success()
//				});

//				Assert.True(keyValidator.IsRestrictedKey("command"));
//			}
//		}

//		public class GetHelpMetadataMethod
//		{
//			[Fact]
//			public void NoArgumentsAddedToStore_ReturnsEmptyPropertyLists()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				HelpMetadata helpMetadata = store.GetHelpMetadata();

//				Assert.Empty(helpMetadata.Arguments);
//				Assert.Empty(helpMetadata.Commands);
//				Assert.Empty(helpMetadata.Options);
//			}

//			[Fact]
//			public void ArgumentsAddedToStore_ReturnsCorrectMetadata()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				store.AddArgument<bool>("argument", ri => ri.Bool1, obj => true, "validator description", "argument description");
//				store.AddCommand("command", context => ProcessStageResult.Success(), "command description");
//				store.AddOption("option", ri => ri.Bool2, 'o', "option description");

//				HelpMetadata helpMetadata = store.GetHelpMetadata();

//				ArgumentHelpInfo argumentInfo = helpMetadata.Arguments.Single();
//				CommandHelpInfo commandInfo = helpMetadata.Commands.Single();
//				OptionHelpInfo optionInfo = helpMetadata.Options.Single();

//				Assert.Equal("argument", argumentInfo.Key);
//				Assert.Equal(typeof(bool), argumentInfo.PropertyInfo.PropertyType);
//				Assert.Equal("Bool1", argumentInfo.PropertyInfo.Name);
//				Assert.Equal("validator description", argumentInfo.ValidatorDescription);
//				Assert.Equal("argument description", argumentInfo.Description);

//				Assert.Equal("command", commandInfo.Key);
//				Assert.Equal("command description", commandInfo.Description);

//				Assert.Equal("option", optionInfo.FullKey);
//				Assert.Equal(typeof(bool), optionInfo.PropertyInfo.PropertyType);
//				Assert.Equal("Bool2", optionInfo.PropertyInfo.Name);
//				Assert.Equal('o', optionInfo.ShortKey);
//				Assert.Equal("option description", optionInfo.Description);
//			}
//		}

//		public class GetFullOptionPropertyInfoMethod
//		{
//			[Fact]
//			public void NullOptionKeyArgument_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.Throws<ArgumentNullException>(() => store.GetFullOptionPropertyInfo(null));
//				Assert.Throws<ArgumentNullException>(() => store.GetFullOptionPropertyInfo(""));
//			}

//			[Fact]
//			public void OptionKey_NotConfigured_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentException>(() => store.GetFullOptionPropertyInfo("option"));
//			}

//			[Fact]
//			public void ValidOptionKey_ReturnsCorrectResult()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				store.AddOption("option", ri => ri.Bool1);

//				PropertyInfo info = store.GetFullOptionPropertyInfo("option");

//				Assert.Equal(typeof(bool), info.PropertyType);
//				Assert.Equal("Bool1", info.Name);
//			}
//		}

//		public class GetShortOptionPropertyInfoMethod
//		{
//			[Fact]
//			public void OptionKey_NotConfigured_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentException>(() => store.GetShortOptionPropertyInfo('o'));
//			}

//			[Fact]
//			public void ValidOptionKey_ReturnsCorrectResult()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				store.AddOption("option", ri => ri.Bool1, 'o');

//				PropertyInfo info = store.GetShortOptionPropertyInfo('o');

//				Assert.Equal(typeof(bool), info.PropertyType);
//				Assert.Equal("Bool1", info.Name);
//			}
//		}

//		public class GetCompoundShortPropertyInfosMethod
//		{
//			[Fact]
//			public void NullOptionKeysArgument_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.Throws<ArgumentNullException>(() => store.GetCompoundShortPropertyInfos(null));
//			}

//			[Theory]
//			[InlineData("abz")]
//			[InlineData("azc")]
//			[InlineData("zbc")]
//			public void InvalidShortOptionKeys_InAnyPosition_Throws(string optionsString)
//			{
//				char[] options = optionsString.ToCharArray();

//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				store
//					.AddOption("option_a", ri => ri.Bool1, 'a')
//					.AddOption("option_b", ri => ri.Bool2, 'b')
//					.AddOption("option_c", ri => ri.Bool3, 'c');

//				Assert.Throws<ArgumentException>(() => store.GetCompoundShortPropertyInfos(options));
//			}

//			[Fact]
//			public void ValidOptionKeysArgument_ReturnsCorrectResult()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				store
//					.AddOption("option_a", ri => ri.Bool1, 'a')
//					.AddOption("option_b", ri => ri.Bool2, 'b')
//					.AddOption("option_c", ri => ri.Bool3, 'c');

//				List<PropertyInfo> result = store.GetCompoundShortPropertyInfos(new char[] { 'b', 'c', 'a' }) as List<PropertyInfo>;

//				Assert.Equal(3, result.Count);
//				Assert.Equal("Bool2", result[0].Name);
//				Assert.Equal("Bool3", result[1].Name);
//				Assert.Equal("Bool1", result[2].Name);
//			}
//		}

//		public class GetArgumentPropertyLinkMethod
//		{
//			[Fact]
//			public void NullKeyArgument_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.Throws<ArgumentNullException>(() => store.GetArgumentPropertyLink(null));
//				Assert.Throws<ArgumentNullException>(() => store.GetArgumentPropertyLink(""));
//			}

//			[Fact]
//			public void KeyArgument_NotConfigured_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				Assert.Throws<ArgumentException>(() => store.GetArgumentPropertyLink("argument"));
//			}

//			[Fact]
//			public void VaildKeyArgument_ReturnsCorrectResult()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				store
//					.AddArgument("argument", ri => ri.Bool1, obj => true);

//				ArgumentPropertyInfo aInfo = store.GetArgumentPropertyLink("argument");
//				Assert.Equal("Bool1", aInfo.Property.Name);
//				Assert.NotNull(aInfo.ValidateFunction);
//			}
//		}

//		public class GetCommandCallbackMethod
//		{
//			[Fact]
//			public void NullKeyArgument_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.Throws<ArgumentNullException>(() => store.GetCommandCallback(null));
//				Assert.Throws<ArgumentNullException>(() => store.GetCommandCallback(""));
//			}

//			[Fact]
//			public void KeyArgument_NotConfigured_Throws()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Assert.Throws<ArgumentException>(() => store.GetCommandCallback("command"));
//			}

//			[Fact]
//			public void ValidKeyArgument_ReturnsCorrectResult()
//			{
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//				Func<StageCallbackContext<ITestRunInfo>, ProcessStageResult> callback = context => ProcessStageResult.Success();

//				store.AddCommand("command", callback);

//				Func<StageCallbackContext<ITestRunInfo>, ProcessStageResult> returned = store.GetCommandCallback("command");

//				Assert.Equal(callback, returned);
//			}
//		}

//		public class IsValidMethods
//		{
//			public class IsValidFullOptionMethod
//			{
//				[Fact]
//				public void NullKey_Throws()
//				{
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//					Assert.Throws<ArgumentNullException>(() => store.IsValidFullOption(null));
//					Assert.Throws<ArgumentNullException>(() => store.IsValidFullOption(""));
//				}

//				[Fact]
//				public void Key_NotConfigured_ReturnsFalse()
//				{
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//					Assert.False(store.IsValidFullOption("option"));
//				}

//				[Fact]
//				public void Key_Configured_ReturnsTrue()
//				{
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//					store.AddOption("option", ri => ri.Bool1);

//					Assert.True(store.IsValidFullOption("option"));
//				}
//			}

//			public class IsValidShortOptionMethod
//			{
//				[Fact]
//				public void Key_NotConfigured_ReturnsFalse()
//				{
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//					Assert.False(store.IsValidShortOption('o'));
//				}

//				[Fact]
//				public void Key_Configured_ReturnsTrue()
//				{
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//					store.AddOption("option", ri => ri.Bool1, 'o');

//					Assert.True(store.IsValidShortOption('o'));
//				}
//			}

//			public class IsValidShortCompoundOptionMethod
//			{
//				[Fact]
//				public void NullKey_Throws()
//				{
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//					Assert.Throws<ArgumentNullException>(() => store.IsValidShortCompoundOption(null));
//					Assert.Throws<ArgumentNullException>(() => store.IsValidShortCompoundOption(""));
//				}

//				[Fact]
//				public void Key_NotConfigured_ReturnsFalse()
//				{
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//					Assert.False(store.IsValidShortCompoundOption("abc"));
//				}

//				[Fact]
//				public void Key_Configured_ReturnsTrue()
//				{
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//					store
//						.AddOption("option_a", ri => ri.Bool1, 'a')
//						.AddOption("option_b", ri => ri.Bool2, 'b')
//						.AddOption("option_c", ri => ri.Bool3, 'c');

//					Assert.True(store.IsValidShortCompoundOption("abc"));
//				}
//			}

//			public class IsValidArgumentMethod
//			{
//				[Fact]
//				public void NullKey_Throws()
//				{
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//					Assert.Throws<ArgumentNullException>(() => store.IsValidArgument(null));
//					Assert.Throws<ArgumentNullException>(() => store.IsValidArgument(""));
//				}

//				[Fact]
//				public void Key_NotConfigured_ReturnsFalse()
//				{
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//					Assert.False(store.IsValidArgument("argument"));
//				}

//				[Fact]
//				public void Key_Configured_ReturnsTrue()
//				{
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//					store.AddArgument("argument", ri => ri.Bool1);

//					Assert.True(store.IsValidArgument("argument"));
//				}
//			}

//			public class IsValidCommandMethod
//			{
//				[Fact]
//				public void NullKey_Throws()
//				{
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//					Assert.Throws<ArgumentNullException>(() => store.IsValidCommand(null));
//					Assert.Throws<ArgumentNullException>(() => store.IsValidCommand(""));
//				}

//				[Fact]
//				public void Key_NotConfigured_ReturnsFalse()
//				{
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//					Assert.False(store.IsValidCommand("command"));
//				}

//				[Fact]
//				public void Key_Configured_ReturnsTrue()
//				{
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//					store.AddCommand("command", context => ProcessStageResult.Success());

//					Assert.True(store.IsValidCommand("command"));
//				}
//			}
//		}
//	}
//}
