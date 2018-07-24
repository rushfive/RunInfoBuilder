//using R5.RunInfoBuilder.ArgumentStore;
//using R5.RunInfoBuilder.Help;
//using R5.RunInfoBuilder.Help.Models;
//using R5.RunInfoBuilder.ProcessPipeline;
//using R5.RunInfoBuilder.ProcessPipeline.Stages;
//using R5.RunInfoBuilder.Tests.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Xunit;

//namespace R5.RunInfoBuilder.Tests.UnitTests
//{
//	public class PipelineProcessorTests
//	{
//		private static PipelineProcessor<ITestRunInfo, TestRunInfo> GetPipelineProcessor()
//		{
//			var runInfo = new TestRunInfo();
//			var keyValidator = new KeyValidator();
//			var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//			return PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);
//		}

//		public class Initialization
//		{
//			[Fact]
//			public void DefaultPipeline_ContainsExpectedStages()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				List<Type> stages = pipeline.GetPipelineStageTypes();

//				Assert.Equal(typeof(CommandStage<ITestRunInfo>), stages[0]);
//				Assert.Equal(typeof(OptionStage<ITestRunInfo, TestRunInfo>), stages[1]);
//				Assert.Equal(typeof(ArgumentStage<ITestRunInfo, TestRunInfo>), stages[2]);
//			}
//		}

//		public class SetParserMethod
//		{
//			[Fact]
//			public void NullParserArgument_Throws()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				Assert.Throws<ArgumentNullException>(() => pipeline.SetParser(null));
//			}
//		}

//		public class GetArgumentTypeMethod
//		{
//			[Fact]
//			public void NullArgumentToken_Throws()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				Assert.Throws<ArgumentNullException>(() => pipeline.GetArgumentType(null));
//				Assert.Throws<ArgumentNullException>(() => pipeline.GetArgumentType(""));
//			}

//			[Fact]
//			public void IsCommand_ReturnsCorrectResult()
//			{
//				var runInfo = new TestRunInfo();
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//				store.AddCommand("command", context => ProcessStageResult.Success());

//				ProgramArgumentType returned = pipeline.GetArgumentType("command");
//				Assert.Equal(ProgramArgumentType.Command, returned);
//			}

//			[Fact]
//			public void IsArgument_ReturnsCorrectResult()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				ProgramArgumentType returned = pipeline.GetArgumentType("key=value");
//				Assert.Equal(ProgramArgumentType.Argument, returned);
//			}

//			[Fact]
//			public void IsOption_ReturnsCorrectResult()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				ProgramArgumentType returned = pipeline.GetArgumentType("--option");
//				Assert.Equal(ProgramArgumentType.Option, returned);

//				returned = pipeline.GetArgumentType("-o");
//				Assert.Equal(ProgramArgumentType.Option, returned);

//				returned = pipeline.GetArgumentType("-abcdef");
//				Assert.Equal(ProgramArgumentType.Option, returned);
//			}

//			[Fact]
//			public void ArgumentToken_IsNot_AnyValidArgumentType_Throws()
//			{
//				var runInfo = new TestRunInfo();
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);
				
//				Assert.Throws<ArgumentException>(() => pipeline.GetArgumentType("invalid_argument_token"));
//			}
//		}

//		public class AddPreProcessArgumentStageMethod
//		{
//			[Fact]
//			public void AlreadyConfigured_Throws()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				pipeline.AddPreProcessArgumentStage(context => ProcessStageResult.Success());

//				Assert.Throws<InvalidOperationException>(() => pipeline.AddPreProcessArgumentStage(context => ProcessStageResult.Success()));
//			}

//			[Fact]
//			public void NullCallbackArgument_Throws()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				Assert.Throws<ArgumentNullException>(() => pipeline.AddPreProcessArgumentStage(null));
//			}

//			[Fact]
//			public void ValidAdd_WithoutHelpStageConfigured_InsertsIntoCorrectPipelinePosition()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				pipeline.AddPreProcessArgumentStage(context => ProcessStageResult.Success());

//				List<Type> stages = pipeline.GetPipelineStageTypes();

//				Assert.Equal(typeof(PreProcessStage<ITestRunInfo>), stages.First());
//			}
			
//			[Fact]
//			public void ValidInvoke_StageProcessing_FiresCallback()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				pipeline.AddPreProcessArgumentStage(context => throw new TestException());

//				var args = new string[] { "-h" };

//				Assert.Throws<TestException>(() => pipeline.ProcessArgs(args));
//			}

//			[Fact]
//			public void ValidInvoke_ReturnsItself()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				IPipelineProcessor<ITestRunInfo> returned = pipeline.AddPreProcessArgumentStage(context => throw new TestException());

//				Assert.Equal(pipeline, returned);
//			}
//		}

//		public class AddPostProcessArgumentStageMethod
//		{
//			[Fact]
//			public void AlreadyConfigured_Throws()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				pipeline.AddPostProcessArgumentStage(context => ProcessStageResult.Success());

//				Assert.Throws<InvalidOperationException>(() => pipeline.AddPostProcessArgumentStage(context => ProcessStageResult.Success()));
//			}

//			[Fact]
//			public void CallbackArgumentNull_Throws()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				Assert.Throws<ArgumentNullException>(() => pipeline.AddPostProcessArgumentStage(null));
//			}

//			[Fact]
//			public void ValidAdd_InsertsIntoEndOfPipeline()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				List<Type> initialStages = pipeline.GetPipelineStageTypes();
//				Type postProcessStageType = typeof(PostProcessStage<ITestRunInfo>);

//				Assert.NotEqual(postProcessStageType, initialStages.Last());

//				pipeline.AddPostProcessArgumentStage(context => ProcessStageResult.Success());

//				List<Type> postAddStages = pipeline.GetPipelineStageTypes();

//				Assert.Equal(postProcessStageType, postAddStages.Last());
//			}

//			[Fact]
//			public void ValidInvoke_StageProcessing_FiresCallback()
//			{
//				var runInfo = new TestRunInfo();
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				store.AddCommand("command", context => ProcessStageResult.Success());

//				var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//				pipeline.AddPostProcessArgumentStage(context => throw new TestException());

//				var args = new string[] { "command" };

//				Assert.Throws<TestException>(() => pipeline.ProcessArgs(args));
//			}

//			[Fact]
//			public void ValidInvoke_ReturnsItself()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				IPipelineProcessor<ITestRunInfo> returned = pipeline.AddPostProcessArgumentStage(context => throw new TestException());

//				Assert.Equal(pipeline, returned);
//			}
//		}

//		public class AddPreProcessPipelineCallbackMethod
//		{
//			[Fact]
//			public void AlreadyConfigured_Throws()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				pipeline.AddPreProcessPipelineCallback(context => ProcessStageResult.Success());

//				Assert.Throws<InvalidOperationException>(() => pipeline.AddPreProcessPipelineCallback(context => ProcessStageResult.Success()));
//			}

//			[Fact]
//			public void NullCallbackArgument_Throws()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				Assert.Throws<ArgumentNullException>(() => pipeline.AddPreProcessPipelineCallback(null));
//			}

//			[Fact]
//			public void ValidInvoke_StageProcessing_FiresCallback()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				pipeline.AddPreProcessPipelineCallback(context => throw new TestException());

//				Assert.Throws<TestException>(() => pipeline.ProcessArgs(new string[] { "arg" }));
//			}

//			[Fact]
//			public void ValidInvoke_ReturnsItself()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				IPipelineProcessor<ITestRunInfo> returned = pipeline.AddPreProcessPipelineCallback(context => throw new TestException());

//				Assert.Equal(pipeline, returned);
//			}
//		}

//		public class AddPostProcessPipelineCallbackMethod
//		{
//			[Fact]
//			public void AlreadyConfigured_Throws()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				pipeline.AddPostProcessPipelineCallback(context => ProcessStageResult.Success());

//				Assert.Throws<InvalidOperationException>(() => pipeline.AddPostProcessPipelineCallback(context => ProcessStageResult.Success()));
//			}

//			[Fact]
//			public void NullCallbackArgument_Throws()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				Assert.Throws<ArgumentNullException>(() => pipeline.AddPostProcessPipelineCallback(null));
//			}

//			[Fact]
//			public void ValidInvoke_StageProcessing_FiresCallback()
//			{
//				var runInfo = new TestRunInfo();
//				var keyValidator = new KeyValidator();
//				var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//				store.AddCommand("command", context => ProcessStageResult.Success());

//				var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//				pipeline.AddPostProcessPipelineCallback(context => throw new TestException());

//				Assert.Throws<TestException>(() => pipeline.ProcessArgs(new string[] { "command" }));
//			}

//			[Fact]
//			public void ValidInvoke_ReturnsItself()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				IPipelineProcessor<ITestRunInfo> returned = pipeline.AddPostProcessPipelineCallback(context => throw new TestException());

//				Assert.Equal(pipeline, returned);
//			}
//		}

//		public class ProcessArgsMethod
//		{
//			[Fact]
//			public void NullArgsArgument_Throws()
//			{
//				PipelineProcessor<ITestRunInfo, TestRunInfo> pipeline = GetPipelineProcessor();

//				Assert.Throws<ArgumentNullException>(() => pipeline.ProcessArgs(null));
//			}

//			public class PreProcessPipelineCallback
//			{
//				[Fact]
//				public void Context_ProvidesCloneOfProgramArgs()
//				{
//					var runInfo = new TestRunInfo();
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//					store
//						.AddCommand("command1", context => ProcessStageResult.Success())
//						.AddCommand("command2", context => ProcessStageResult.Success());

//					var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//					var args = new string[] { "command1", "command2" };

//					pipeline.AddPreProcessPipelineCallback(context =>
//					{
//						string[] contextArgs = context.Args;

//						Assert.Equal(args[0], contextArgs[0]);
//						Assert.Equal(args[1], contextArgs[1]);

//						Assert.NotSame(args, contextArgs);
//					});

//					pipeline.ProcessArgs(args);
//				}
//			}

//			public class ArgumentStage
//			{
//				[Fact]
//				public void Parser_NotConfigured_Throws()
//				{
//					var runInfo = new TestRunInfo();
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//					store.AddArgument("argument", ri => ri.Bool1);

//					var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//					Assert.Throws<InvalidOperationException>(() => pipeline.ProcessArgs(new string[] { "argument=true" }));
//				}

//				[Fact]
//				public void Parser_DoesntHandleType_Throws()
//				{
//					var runInfo = new TestRunInfo();
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//					store.AddArgument("argument", ri => ri.Bool1);

//					var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//					var parser = new Parser.Parser(true, true, null);
//					pipeline.SetParser(parser);

//					Assert.Throws<TypeArgumentException>(() => pipeline.ProcessArgs(new string[] { "argument=true" }));
//				}

//				[Fact]
//				public void Parser_HandlesType_InvalidValue_Throws()
//				{
//					var runInfo = new TestRunInfo();
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//					store.AddArgument("argument", ri => ri.Bool1);

//					var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//					var parser = new Parser.Parser(true, true, null);
//					parser.AddTypeParserPredicate<bool>(val =>
//					{
//						if (bool.TryParse(val, out bool parsed))
//						{
//							return (true, parsed);
//						}
//						return (false, null);
//					});

//					pipeline.SetParser(parser);

//					Assert.Throws<ProcessStageException<ITestRunInfo>>(() => pipeline.ProcessArgs(new string[] { "argument=notbool" }));
//				}

//				[Fact]
//				public void Parser_HandlesType_ValidValue_SuccessfullySetsRunInfoProperty()
//				{
//					var runInfo = new TestRunInfo();

//					Assert.False(runInfo.Bool1);

//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//					store.AddArgument("argument", ri => ri.Bool1);

//					var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//					var parser = new Parser.Parser(true, true, null);
//					parser.AddTypeParserPredicate<bool>(val =>
//					{
//						if (bool.TryParse(val, out bool parsed))
//						{
//							return (true, parsed);
//						}
//						return (false, null);
//					});

//					pipeline.SetParser(parser);

//					pipeline.ProcessArgs(new string[] { "argument=true" });

//					Assert.True(runInfo.Bool1);
//				}

//				[Fact]
//				public void Validation_Fails_Throws()
//				{
//					var runInfo = new TestRunInfo();
//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//					store.AddArgument("argument", ri => ri.Bool1, val => (bool)val == false);

//					var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//					var parser = new Parser.Parser(true, true, null);
//					parser.AddTypeParserPredicate<bool>(val =>
//					{
//						if (bool.TryParse(val, out bool parsed))
//						{
//							return (true, parsed);
//						}
//						return (false, null);
//					});

//					pipeline.SetParser(parser);

//					Assert.Throws<ProcessStageException<ITestRunInfo>>(() => pipeline.ProcessArgs(new string[] { "argument=true" }));
//				}

//				[Fact]
//				public void Validation_Passes_SuccessfullySetsRunInfoProperty()
//				{
//					var runInfo = new TestRunInfo();

//					Assert.False(runInfo.Bool1);

//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//					store.AddArgument("argument", ri => ri.Bool1, val => (bool)val == true);

//					var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//					var parser = new Parser.Parser(true, true, null);
//					parser.AddTypeParserPredicate<bool>(val =>
//					{
//						if (bool.TryParse(val, out bool parsed))
//						{
//							return (true, parsed);
//						}
//						return (false, null);
//					});

//					pipeline.SetParser(parser);

//					pipeline.ProcessArgs(new string[] { "argument=true" });

//					Assert.True(runInfo.Bool1);
//				}
//			}

//			public class CommandStage
//			{
//				[Fact]
//				public void Command_NotConfigured_Throws()
//				{
//					var runInfo = new TestRunInfo();

//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//					var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//					Assert.Throws<ArgumentException>(() => pipeline.ProcessArgs(new string[] { "invalid_command" }));
//				}

//				[Fact]
//				public void Command_Configured_FiresCallback()
//				{
//					var runInfo = new TestRunInfo();

//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//					store.AddCommand("command", context => throw new TestException());

//					var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//					Assert.Throws<TestException>(() => pipeline.ProcessArgs(new string[] { "command" }));
//				}
//			}

//			public class OptionStage
//			{
//				[Fact]
//				public void FullOption_NotConfigured_Throws()
//				{
//					var runInfo = new TestRunInfo();

//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//					var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//					Assert.Throws<ArgumentException>(() => pipeline.ProcessArgs(new string[] { "--option" }));
//				}

//				[Fact]
//				public void FullOption_Configured_SuccessfullySetsRunInfoProperty()
//				{
//					var runInfo = new TestRunInfo();

//					Assert.False(runInfo.Bool1);

//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//					store.AddOption("option", ri => ri.Bool1);

//					var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//					pipeline.ProcessArgs(new string[] { "--option" });

//					Assert.True(runInfo.Bool1);
//				}

//				[Fact]
//				public void ShortOption_NotConfigured_Throws()
//				{
//					var runInfo = new TestRunInfo();

//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);

//					var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//					Assert.Throws<ArgumentException>(() => pipeline.ProcessArgs(new string[] { "-o" }));
//				}

//				[Fact]
//				public void ShortOption_Configured_SuccessfullySetsRunInfoProperty()
//				{
//					var runInfo = new TestRunInfo();

//					Assert.False(runInfo.Bool1);

//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//					store.AddOption("option", ri => ri.Bool1, 'o');

//					var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//					pipeline.ProcessArgs(new string[] { "-o" });

//					Assert.True(runInfo.Bool1);
//				}

//				[Fact]
//				public void ShortCompoundOption_ContainsInvalidKey_Throws()
//				{
//					var runInfo = new TestRunInfo();

//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//					store
//						.AddOption("option1", ri => ri.Bool1, 'a')
//						.AddOption("option2", ri => ri.Bool2, 'b')
//						.AddOption("option3", ri => ri.Bool3, 'c');
					
//					var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//					Assert.Throws<ArgumentException>(() => pipeline.ProcessArgs(new string[] { "-abz" }));
//					Assert.Throws<ArgumentException>(() => pipeline.ProcessArgs(new string[] { "-zbc" }));
//					Assert.Throws<ArgumentException>(() => pipeline.ProcessArgs(new string[] { "-azc" }));
//				}

//				[Fact]
//				public void ShortCompountOption_Valid_SuccessfullySetsRunInfoProperties()
//				{
//					var runInfo = new TestRunInfo();

//					Assert.False(runInfo.Bool1);
//					Assert.False(runInfo.Bool2);
//					Assert.False(runInfo.Bool3);

//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//					store
//						.AddOption("option1", ri => ri.Bool1, 'a')
//						.AddOption("option2", ri => ri.Bool2, 'b')
//						.AddOption("option3", ri => ri.Bool3, 'c');

//					var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//					pipeline.ProcessArgs(new string[] { "-abc" });

//					Assert.True(runInfo.Bool1);
//					Assert.True(runInfo.Bool2);
//					Assert.True(runInfo.Bool3);
//				}
//			}

//			public class ProcessStageResultHandling
//			{
//				public class SkipArgsCount
//				{
//					[Fact]
//					public void SkipArgsCount_CorrectlySkipsNextProgramArguments()
//					{
//						var infoWithoutSkip = new TestRunInfo();

//						var keyValidator = new KeyValidator();
//						var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//						store
//							.AddOption("option1", ri => ri.Bool1)
//							.AddOption("option2", ri => ri.Bool2)
//							.AddOption("option3", ri => ri.Bool3);

//						var pipelineWithoutSkip = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(infoWithoutSkip, store, Constants.OptionRegex, Constants.ArgumentRegex);

//						pipelineWithoutSkip.ProcessArgs(new string[] { "--option1", "--option2", "--option3" });
//						Assert.True(infoWithoutSkip.Bool1);
//						Assert.True(infoWithoutSkip.Bool2);
//						Assert.True(infoWithoutSkip.Bool3);

//						var infoWithSkip = new TestRunInfo();

//						var pipelineWithSkip = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(infoWithSkip, store, Constants.OptionRegex, Constants.ArgumentRegex);
//						pipelineWithSkip.AddPreProcessArgumentStage(context =>
//						{
//							if (context.Token == "--option1")
//							{
//								return ProcessStageResult.Success(1, true);
//							}
//							return ProcessStageResult.Success();
//						});

//						pipelineWithSkip.ProcessArgs(new string[] { "--option1", "--option2", "--option3" });
//						Assert.True(infoWithSkip.Bool1);
//						Assert.False(infoWithSkip.Bool2);
//						Assert.True(infoWithSkip.Bool3);
//					}

//					[Fact]
//					public void NextEnumeratorIndex_GreaterThanProgramArgumentsBounds_HaltsProcessing()
//					{
//						var runInfo = new TestRunInfo();

//						Assert.False(runInfo.Bool1);
//						Assert.False(runInfo.Bool2);
//						Assert.False(runInfo.Bool3);

//						var keyValidator = new KeyValidator();
//						var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//						store
//							.AddOption("option1", ri => ri.Bool1)
//							.AddOption("option2", ri => ri.Bool2)
//							.AddOption("option3", ri => ri.Bool3)
//							.AddCommand("command", context => throw new TestException());

//						var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//						pipeline.AddPreProcessArgumentStage(context =>
//						{
//							if (context.Token == "--option1")
//							{
//								return ProcessStageResult.Success(3, true);
//							}
//							return ProcessStageResult.Success();
//						});

//						pipeline.ProcessArgs(new string[] { "--option1", "--option2", "--option3", "command" });

//						Assert.True(runInfo.Bool1);
//						Assert.False(runInfo.Bool2);
//						Assert.False(runInfo.Bool3);

//						// success, because command stage (and exception) doesnt occur
//					}
//				}

//				public class ContinuePipeline
//				{
//					[Fact]
//					public void True_SuccessfullyContinues()
//					{
//						var runInfo = new TestRunInfo();

//						Assert.False(runInfo.Bool1);

//						var keyValidator = new KeyValidator();
//						var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//						store.AddOption("option", ri => ri.Bool1);

//						var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);
//						pipeline.AddPreProcessArgumentStage(context => ProcessStageResult.Success(0, true));

//						pipeline.ProcessArgs(new string[] { "--option" });

//						Assert.True(runInfo.Bool1);
//					}

//					[Fact]
//					public void False_StopsContinuation_ForCurrentProgramArgument()
//					{
//						var runInfo = new TestRunInfo();

//						Assert.False(runInfo.Bool1);

//						var keyValidator = new KeyValidator();
//						var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//						store.AddOption("option", ri => ri.Bool1);

//						var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);
//						pipeline.AddPreProcessArgumentStage(context => ProcessStageResult.Success(0, false));

//						pipeline.ProcessArgs(new string[] { "--option" });

//						Assert.False(runInfo.Bool1);
//					}
//				}

//				public class HaltProcessing
//				{
//					[Fact]
//					public void True_HaltsFurtherProcessing_OfAllProgramArguments()
//					{
//						var runInfo = new TestRunInfo();

//						Assert.False(runInfo.Bool1);
//						Assert.False(runInfo.Bool2);

//						var keyValidator = new KeyValidator();
//						var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//						store
//							.AddOption("option1", ri => ri.Bool1)
//							.AddOption("option2", ri => ri.Bool2);

//						var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);
//						pipeline.AddPreProcessArgumentStage(context => ProcessStageResult.HaltProcessing());

//						pipeline.ProcessArgs(new string[] { "--option1", "--option2" });

//						Assert.False(runInfo.Bool1);
//						Assert.False(runInfo.Bool2);
//					}

//					[Fact]
//					public void False_ContinuesProcessing_RestOfProgramArguments()
//					{
//						var runInfo = new TestRunInfo();

//						Assert.False(runInfo.Bool1);
//						Assert.False(runInfo.Bool2);

//						var keyValidator = new KeyValidator();
//						var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//						store
//							.AddOption("option1", ri => ri.Bool1)
//							.AddOption("option2", ri => ri.Bool2);

//						var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);

//						// Success() sets ContinueArgsProcessing = true
//						pipeline.AddPreProcessArgumentStage(context => ProcessStageResult.Success());

//						pipeline.ProcessArgs(new string[] { "--option1", "--option2" });

//						Assert.True(runInfo.Bool1);
//						Assert.True(runInfo.Bool2);
//					}
//				}
//			}

//			public class PostProcessPipelineCallback
//			{
//				[Fact]
//				public void Context_Provides_CloneOfProgramArgs_And_RunInfoReference()
//				{
//					var runInfo = new TestRunInfo();

//					var keyValidator = new KeyValidator();
//					var store = new ArgumentStore<ITestRunInfo>(keyValidator);
//					store
//						.AddCommand("command1", context => ProcessStageResult.Success())
//						.AddCommand("command2", context => ProcessStageResult.Success());

//					var args = new string[] { "command1", "command2" };

//					var pipeline = PipelineProcessor<ITestRunInfo, TestRunInfo>.Initialize(runInfo, store, Constants.OptionRegex, Constants.ArgumentRegex);
//					pipeline.AddPostProcessPipelineCallback(context =>
//					{
//						string[] contextArgs = context.Args;

//						Assert.Equal(args[0], contextArgs[0]);
//						Assert.Equal(args[1], contextArgs[1]);

//						Assert.NotSame(args, contextArgs);

//						Assert.Same(runInfo, context.RunInfo);
//					});

//					pipeline.ProcessArgs(args);
//				}
//			}
//		}
//	}
//}
