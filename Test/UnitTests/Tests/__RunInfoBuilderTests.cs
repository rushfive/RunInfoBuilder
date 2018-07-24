//using R5.RunInfoBuilder.Abstractions;
//using R5.RunInfoBuilder.Help.Models;
//using R5.RunInfoBuilder.Models;
//using R5.RunInfoBuilder.ProcessPipeline;
//using R5.RunInfoBuilder.Tests.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Xunit;

//namespace R5.RunInfoBuilder.Tests.UnitTests
//{
//	public class RunInfoBuilderTests
//	{
//		public class SetupHelpMethod
//		{
//			[Fact]
//			public void BeforeSetup_AccessingHelpMethods_Throws()
//			{
//				IRunInfoBuilder<ITestRunInfo, TestRunInfo> builder = BuilderSetup<ITestRunInfo, TestRunInfo>.Default();
//				Assert.Throws<NullReferenceException>(() => builder.Help.GetMetadata());
//			}

//			[Fact]
//			public void Calling_WithoutSetupFunction_Throws()
//			{
//				IRunInfoBuilder<ITestRunInfo, TestRunInfo> builder = BuilderSetup<ITestRunInfo, TestRunInfo>.Default();
//				Assert.Throws<ArgumentNullException>(() => builder.SetupHelp(null));
//			}

//			[Fact]
//			public void SetupFunction_Build_BeforeSettingTriggerCallback_Throws()
//			{
//				IRunInfoBuilder<ITestRunInfo, TestRunInfo> builder = BuilderSetup<ITestRunInfo, TestRunInfo>.Default();

//				Assert.Throws<InvalidOperationException>(() =>
//				{
//					builder.SetupHelp(helpBuilder => { });
//				});
//			}

//			[Fact]
//			public void SettingTriggerCallback_SuccessfullyBuilds()
//			{
//				IRunInfoBuilder<ITestRunInfo, TestRunInfo> builder = BuilderSetup<ITestRunInfo, TestRunInfo>.Default();

//				builder.SetupHelp(helpBuilder =>
//				{
//					helpBuilder.SetTriggerCallback(context => { });
//				});

//				HelpMetadata metadata = builder.Help.GetMetadata();
//				Assert.NotNull(metadata);
//			}

//			[Fact]
//			public void Calling_WhenHelpAlreadyConfigured_Throws()
//			{
//				IRunInfoBuilder<ITestRunInfo, TestRunInfo> builder = BuilderSetup<ITestRunInfo, TestRunInfo>.Default();

//				builder.SetupHelp(helpBuilder =>
//				{
//					helpBuilder.SetTriggerCallback(context => { });
//				});

//				Assert.Throws<InvalidOperationException>(() =>
//				{
//					builder.SetupHelp(helpBuilder =>
//					{
//						helpBuilder.SetTriggerCallback(context => { });
//					});
//				});
//			}
//		}

//		public class SetupParserMethod
//		{
//			[Fact]
//			public void BeforeSetup_AccessingParserMethods_Throws()
//			{
//				IRunInfoBuilder<ITestRunInfo, TestRunInfo> builder = BuilderSetup<ITestRunInfo, TestRunInfo>.Default();
//				Assert.Throws<NullReferenceException>(() => builder.Parser.ClearAllPredicates());
//			}

//			[Fact]
//			public void Calling_WithoutSetupFunction_Throws()
//			{
//				IRunInfoBuilder<ITestRunInfo, TestRunInfo> builder = BuilderSetup<ITestRunInfo, TestRunInfo>.Default();
//				Assert.Throws<ArgumentNullException>(() => builder.SetupParser(null));
//			}

//			[Fact]
//			public void SettingUp_SuccessfullyConfiguresParser()
//			{
//				IRunInfoBuilder<ITestRunInfo, TestRunInfo> builder = BuilderSetup<ITestRunInfo, TestRunInfo>.Default();

//				builder.SetupParser(parserBuilder => { });

//				builder.Parser.ClearAllPredicates();
//			}

//			[Fact]
//			public void Calling_WhenParserAlreadyConfigured_Throws()
//			{
//				IRunInfoBuilder<ITestRunInfo, TestRunInfo> builder = BuilderSetup<ITestRunInfo, TestRunInfo>.Default();

//				builder.SetupParser(parserBuilder => { });

//				Assert.Throws<InvalidOperationException>(() =>
//				{
//					builder.SetupParser(parserBuilder => { });
//				});
//			}
//		}

//		public class SetupDefaultParserMethod
//		{
//			[Fact]
//			public void Calling_SuccessfullyConfiguresParser()
//			{
//				IRunInfoBuilder<ITestRunInfo, TestRunInfo> builder = BuilderSetup<ITestRunInfo, TestRunInfo>.Default();

//				builder.SetupDefaultParser();

//				builder.Parser.ClearAllPredicates();
//			}

//			[Fact]
//			public void Calling_WhenParserAlreadyConfigured_Throws()
//			{
//				IRunInfoBuilder<ITestRunInfo, TestRunInfo> builder = BuilderSetup<ITestRunInfo, TestRunInfo>.Default();

//				builder.SetupDefaultParser();

//				Assert.Throws<InvalidOperationException>(() =>
//				{
//					builder.SetupDefaultParser();
//				});
//			}
//		}

//		//public class AddCommandMethods
//		//{
//		//	[Fact]
//		//	public void Calling_WithNull_ICommandArgument_Throws()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();
//		//		Assert.Throws<ArgumentNullException>(() => builder.AddCommand(null));
//		//	}

//		//	[Fact]
//		//	public void InvalidCommandTokens_Build_ReturnsFailResult()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();

//		//		string[] args = { "invalidCommand", "invalidCommand2" };

//		//		BuildResult<ITestRunInfo> result = builder.Build(args);

//		//		Assert.Equal(BuildResultType.Fail, result.Type);
//		//		Assert.Equal(2, result.PreProcessValidationErrors.Count);
//		//	}

//		//	[Fact]
//		//	public void HelpConfigured_AddCommand_AddsHelpInfo()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();

//		//		builder.SetupHelp(helpBuilder =>
//		//		{
//		//			helpBuilder.SetTriggerCallback(context => ProcessStageResult.Success());
//		//		});

//		//		const string key = "command";
//		//		const string description = "command description";

//		//		builder.AddCommand(key, context => ProcessStageResult.Success(), description);

//		//		HelpMetadata metadata = builder.Help.GetMetadata();

//		//		Assert.Single(metadata.Commands);
//		//		Assert.Equal(key, metadata.Commands.Single().Key);
//		//		Assert.Equal(description, metadata.Commands.Single().Description);
//		//	}

//		//	[Fact]
//		//	public void ValidCommandTokens_Callback_CorrectlyTriggers()
//		//	{
//		//		var runInfo = new TestRunInfo();
//		//		Assert.False(runInfo.Bool1);
//		//		Assert.False(runInfo.Bool2);
//		//		Assert.False(runInfo.Bool3);

//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>(runInfo);

//		//		builder
//		//			.AddCommand("command", context =>
//		//			{
//		//				context.RunInfo.Bool1 = true;
//		//				return ProcessStageResult.Success();
//		//			})
//		//			.AddCommand(new Command<ITestRunInfo>
//		//			{
//		//				Key = "command2",
//		//				Callback = context =>
//		//				{
//		//					context.RunInfo.Bool2 = true;
//		//					return ProcessStageResult.Success();
//		//				}
//		//			});

//		//		BuildResult<ITestRunInfo> result = builder.Build(new string[] { "command", "command2" });
//		//		Assert.Equal(BuildResultType.Success, result.Type);
//		//		Assert.True(result.RunInfo.Bool1);
//		//		Assert.True(result.RunInfo.Bool2);
//		//		Assert.False(result.RunInfo.Bool3);
//		//	}
//		//}

//		//public class AddOptionMethods
//		//{
//		//	[Fact]
//		//	public void Calling_WithNull_IOptionArgument_Throws()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();
//		//		Assert.Throws<ArgumentNullException>(() => builder.AddOption(null));
//		//	}

//		//	[Fact]
//		//	public void InvalidOptionTokens_Build_ReturnsFailResult()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();

//		//		string[] args = { "invalidOption", "invalidOption2" };

//		//		BuildResult<ITestRunInfo> result = builder.Build(args);

//		//		Assert.Equal(BuildResultType.Fail, result.Type);
//		//		Assert.Equal(2, result.PreProcessValidationErrors.Count);
//		//	}

//		//	[Fact]
//		//	public void HelpConfigured_AddOption_AddsHelpInfo()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();

//		//		builder.SetupHelp(helpBuilder =>
//		//		{
//		//			helpBuilder.SetTriggerCallback(context => ProcessStageResult.Success());
//		//		});

//		//		const string key = "option";
//		//		const string description = "option description";

//		//		builder.AddOption(new Option<ITestRunInfo>
//		//		{
//		//			FullKey = key,
//		//			Description = description,
//		//			PropertyExpression = r => r.Bool1
//		//		});

//		//		HelpMetadata metadata = builder.Help.GetMetadata();

//		//		Assert.Single(metadata.Options);
//		//		Assert.Equal(key, metadata.Options.Single().FullKey);
//		//		Assert.Equal(description, metadata.Options.Single().Description);
//		//	}

//		//	[Fact]
//		//	public void ValidOptionArguments_CorrectlySets_RunInfoProperties()
//		//	{
//		//		var runInfo = new TestRunInfo();
//		//		Assert.False(runInfo.Bool1);
//		//		Assert.False(runInfo.Bool2);
//		//		Assert.False(runInfo.Bool3);

//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>(runInfo);

//		//		builder
//		//			.AddOption("option1", r => r.Bool1)
//		//			.AddOption(new Option<ITestRunInfo>
//		//			{
//		//				FullKey = "option2",
//		//				PropertyExpression = r => r.Bool2
//		//			});

//		//		BuildResult<ITestRunInfo> result = builder.Build(new string[] { "--option1", "--option2" });
//		//		Assert.Equal(BuildResultType.Success, result.Type);
//		//		Assert.True(result.RunInfo.Bool1);
//		//		Assert.True(result.RunInfo.Bool2);
//		//		Assert.False(result.RunInfo.Bool3);
//		//	}
//		//}

//		//public class AddArgumentMethods
//		//{
//		//	[Fact]
//		//	public void Calling_WithNull_IArgument_Throws()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();
//		//		Assert.Throws<ArgumentNullException>(() => builder.AddArgument<bool>(null));
//		//	}

//		//	[Fact]
//		//	public void InvalidArgumentTokens_Build_ReturnsFailResult()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();

//		//		string[] args = { "invalidArg1=value", "invalidArg2=value" };

//		//		BuildResult<ITestRunInfo> result = builder.Build(args);

//		//		Assert.Equal(BuildResultType.Fail, result.Type);
//		//		Assert.Equal(2, result.PreProcessValidationErrors.Count);
//		//	}

//		//	[Fact]
//		//	public void HelpConfigured_AddOption_AddsHelpInfo()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();

//		//		builder.SetupHelp(helpBuilder =>
//		//		{
//		//			helpBuilder.SetTriggerCallback(context => ProcessStageResult.Success());
//		//		});

//		//		const string key = "argument";
//		//		const string description = "argument description";

//		//		builder.AddArgument(new Argument<ITestRunInfo, bool>
//		//		{
//		//			Key = key,
//		//			Description = description,
//		//			PropertyExpression = r => r.Bool1
//		//		});

//		//		HelpMetadata metadata = builder.Help.GetMetadata();

//		//		Assert.Single(metadata.Arguments);
//		//		Assert.Equal(key, metadata.Arguments.Single().Key);
//		//		Assert.Equal(description, metadata.Arguments.Single().Description);
//		//	}

//		//	[Fact]
//		//	public void ValidOptionArguments_CorrectlySets_RunInfoProperties()
//		//	{
//		//		var runInfo = new TestRunInfo();
//		//		Assert.Equal(default, runInfo.Int1);
//		//		Assert.Equal(default, runInfo.Int1);
//		//		Assert.Equal(default, runInfo.Int1);

//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>(runInfo);

//		//		builder
//		//			.SetupDefaultParser()
//		//			.AddArgument<int>("argument1", r => r.Int1)
//		//			.AddArgument<int>(new Argument<ITestRunInfo, int>
//		//			{
//		//				Key = "argument2",
//		//				PropertyExpression = r => r.Int2
//		//			});

//		//		BuildResult<ITestRunInfo> result = builder.Build(new string[] { "argument1=999", "argument2=999" });
//		//		Assert.Equal(BuildResultType.Success, result.Type);
//		//		Assert.Equal(999, result.RunInfo.Int1);
//		//		Assert.Equal(999, result.RunInfo.Int2);
//		//		Assert.Equal(default, result.RunInfo.Int3);
//		//	}
//		//}

//		//public class AddPreProcessArgumentStageMethod
//		//{
//		//	[Fact]
//		//	public void CanOnlyBeConfiguredOnce()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();

//		//		builder.AddPreProcessArgumentStage(context => ProcessStageResult.Success());

//		//		Assert.Throws<InvalidOperationException>(() =>
//		//			builder.AddPreProcessArgumentStage(context => ProcessStageResult.Success()));
//		//	}

//		//	[Fact]
//		//	public void NullCallbackArgument_Throws()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();
//		//		Assert.Throws<ArgumentNullException>(() => builder.AddPreProcessArgumentStage(null));
//		//	}

//		//	[Fact]
//		//	public void ValidAdd_SuccessfullyInvokes_Callback()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();
//		//		builder
//		//			.AddCommand("command", context => ProcessStageResult.Success())
//		//			.AddPreProcessArgumentStage((StageCallbackContext<ITestRunInfo> context) =>
//		//			{
//		//				if (context.Args.First() == "command")
//		//				{
//		//					throw new Exception();
//		//				}
//		//				return ProcessStageResult.Success();
//		//			});

//		//		Assert.Throws<Exception>(() => builder.Build(new string[] { "command" }));
//		//	}
//		//}

//		//public class AddPostProcessArgumentStageMethod
//		//{
//		//	[Fact]
//		//	public void CanOnlyBeConfiguredOnce()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();

//		//		builder.AddPostProcessArgumentStage(context => ProcessStageResult.Success());

//		//		Assert.Throws<InvalidOperationException>(() =>
//		//			builder.AddPostProcessArgumentStage(context => ProcessStageResult.Success()));
//		//	}

//		//	[Fact]
//		//	public void NullCallbackArgument_Throws()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();
//		//		Assert.Throws<ArgumentNullException>(() => builder.AddPostProcessArgumentStage(null));
//		//	}

//		//	[Fact]
//		//	public void ValidAdd_SuccessfullyInvokes_Callback()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();
//		//		builder
//		//			.AddCommand("command", context => ProcessStageResult.Success())
//		//			.AddPostProcessArgumentStage((StageCallbackContext<ITestRunInfo> context) =>
//		//			{
//		//				if (context.Args.First() == "command")
//		//				{
//		//					throw new Exception();
//		//				}
//		//				return ProcessStageResult.Success();
//		//			});

//		//		Assert.Throws<Exception>(() => builder.Build(new string[] { "command" }));
//		//	}
//		//}

//		//public class AddPreProcessPipelineCallbackMethod
//		//{
//		//	[Fact]
//		//	public void CanOnlyBeConfiguredOnce()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();

//		//		builder.AddPreProcessPipelineCallback(context => { });

//		//		Assert.Throws<InvalidOperationException>(() =>
//		//			builder.AddPreProcessPipelineCallback(context => { }));
//		//	}

//		//	[Fact]
//		//	public void NullCallbackArgument_Throws()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();
//		//		Assert.Throws<ArgumentNullException>(() => builder.AddPreProcessPipelineCallback(null));
//		//	}

//		//	[Fact]
//		//	public void ValidAdd_SuccessfullyInvokes_Callback()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();

//		//		builder
//		//			.AddPreProcessPipelineCallback((PreProcessPipelineCallbackContext context) =>
//		//			{
//		//				if (context.Args.First() == "command")
//		//				{
//		//					throw new Exception();
//		//				}
//		//			});

//		//		Assert.Throws<Exception>(() => builder.Build(new string[] { "command" }));
//		//	}
//		//}

//		//public class AddPostProcessPipelineCallbackMethod
//		//{
//		//	[Fact]
//		//	public void CanOnlyBeConfiguredOnce()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();

//		//		builder.AddPostProcessPipelineCallback(context => { });

//		//		Assert.Throws<InvalidOperationException>(() =>
//		//			builder.AddPostProcessPipelineCallback(context => { }));
//		//	}

//		//	[Fact]
//		//	public void NullCallbackArgument_Throws()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();
//		//		Assert.Throws<ArgumentNullException>(() => builder.AddPostProcessPipelineCallback(null));
//		//	}

//		//	[Fact]
//		//	public void ValidAdd_SuccessfullyInvokes_Callback()
//		//	{
//		//		var builder = new RunInfoBuilder<ITestRunInfo, TestRunInfo>();

//		//		builder
//		//			.AddCommand("command", context => ProcessStageResult.Success())
//		//			.AddPostProcessPipelineCallback((PostProcessPipelineCallbackContext<ITestRunInfo> context) =>
//		//			{
//		//				if (context.Args.First() == "command")
//		//				{
//		//					throw new Exception();
//		//				}
//		//			});

//		//		Assert.Throws<Exception>(() => builder.Build(new string[] { "command" }));
//		//	}
//		//}
//	}
//}
