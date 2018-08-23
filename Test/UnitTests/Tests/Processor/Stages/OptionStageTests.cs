//using R5.RunInfoBuilder.Commands;
//using R5.RunInfoBuilder.Parser;
//using R5.RunInfoBuilder.Processor.Models;
//using R5.RunInfoBuilder.Processor.Stages;
//using R5.RunInfoBuilder.UnitTests.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;

//namespace R5.RunInfoBuilder.UnitTests.Tests.Processor.Stages
//{
//	public class OptionStageTests
//	{
//		private OptionStage<TestRunInfo> GetBaseStage(
//			TestRunInfo runInfo = null,
//			string[] args = null,
//			List<Command<TestRunInfo>> subCommands = null,
//			List<IOption> options = null,
//			Func<CallbackContext<TestRunInfo>> callbackContextFactory = null)
//		{
//			if (runInfo == null)
//			{
//				runInfo = new TestRunInfo();
//			}

//			if (args == null)
//			{
//				args = new string[] { };
//			}
//			if (subCommands == null)
//			{
//				subCommands = new List<Command<TestRunInfo>>();
//			}
//			if (options == null)
//			{
//				options = new List<IOption>();
//			}
//			if (callbackContextFactory == null)
//			{
//				callbackContextFactory = () => throw new NotImplementedException("TODO");
//			}

//			return new OptionStage<TestRunInfo>(new ArgumentParser());
//		}

//		private ProcessContext<TestRunInfo> GetProcessContext(
//			TestRunInfo runInfo = null,
//			Func<CallbackContext<TestRunInfo>> callbackContextFactory = null,
//			StageCallbacks<TestRunInfo> stageCallbacks = null,
//			ProgramArgumentCallbacks<TestRunInfo> programArgumentCallbacks = null,
//			Action<Queue<Stage<TestRunInfo>>> extendPipelineCallback = null)
//		{
//			if (runInfo == null)
//			{
//				runInfo = new TestRunInfo();
//			}
//			if (callbackContextFactory == null)
//			{
//				callbackContextFactory = () => new CallbackContext<TestRunInfo>(null, 0, null, new string[] { });
//			}
//			if (stageCallbacks == null)
//			{
//				stageCallbacks = new StageCallbacks<TestRunInfo>(null, null);
//			}
//			if (programArgumentCallbacks == null)
//			{
//				programArgumentCallbacks = new ProgramArgumentCallbacks<TestRunInfo>(null, null, null);
//			}
//			if (extendPipelineCallback == null)
//			{
//				extendPipelineCallback = queue => { };
//			}

//			return new ProcessContext<TestRunInfo>(runInfo, callbackContextFactory, stageCallbacks, programArgumentCallbacks, extendPipelineCallback);
//		}

//		//[Fact]
//		//public void NoMore_ProgramArguments_ReturnsEndResult()
//		//{
//		//	OptionStage<TestRunInfo> stage = GetBaseStage();

//		//	ProcessStageResult result = stage.ProcessStage();

//		//	Assert.Equal(ProcessResult.End, result);
//		//}

//		//[Fact]
//		//public void NextIsSubCommand_Returns_ContinueResult()
//		//{
//		//	var args = new string[] { "subcommand" };
//		//	var subCommands = new List<Command<TestRunInfo>>
//		//	{
//		//		new Command<TestRunInfo>
//		//		{
//		//			Key = "subcommand"
//		//		}
//		//	};

//		//	OptionStage<TestRunInfo> stage = GetBaseStage(args: args, subCommands: subCommands);

//		//	ProcessStageResult result = stage.ProcessStage();

//		//	Assert.Equal(ProcessResult.Continue, result);
//		//}

//		// TODO: further processing tests
//	}
//}
