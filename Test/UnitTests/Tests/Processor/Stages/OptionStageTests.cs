using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor.Models;
using R5.RunInfoBuilder.Processor.Stages;
using R5.RunInfoBuilder.UnitTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.UnitTests.Tests.Processor.Stages
{
	public class OptionStageTests
	{
		private OptionStage<TestRunInfo> GetBaseStage(
			TestRunInfo runInfo = null,
			string[] args = null,
			List<Command<TestRunInfo>> subCommands = null,
			List<IOption> options = null,
			Func<CallbackContext<TestRunInfo>> callbackContextFactory = null)
		{
			if (runInfo == null)
			{
				runInfo = new TestRunInfo();
			}

			if (args == null)
			{
				args = new string[] { };
			}
			if (subCommands == null)
			{
				subCommands = new List<Command<TestRunInfo>>();
			}
			if (options == null)
			{
				options = new List<IOption>();
			}
			if (callbackContextFactory == null)
			{
				callbackContextFactory = () => throw new NotImplementedException("TODO");
			}
			
			var context = new ProcessContext<TestRunInfo>(new ArgumentParser(), 
				runInfo, callbackContextFactory, args, subCommands, options);

			return new OptionStage<TestRunInfo>(context);
		}

		[Fact]
		public void NoMore_ProgramArguments_ReturnsEndResult()
		{
			OptionStage<TestRunInfo> stage = GetBaseStage();

			ProcessStageResult result = stage.ProcessStage();

			Assert.Equal(ProcessResult.End, result);
		}

		[Fact]
		public void NextIsSubCommand_Returns_ContinueResult()
		{
			var args = new string[] { "subcommand" };
			var subCommands = new List<Command<TestRunInfo>>
			{
				new Command<TestRunInfo>
				{
					Key = "subcommand"
				}
			};

			OptionStage<TestRunInfo> stage = GetBaseStage(args: args, subCommands: subCommands);

			ProcessStageResult result = stage.ProcessStage();

			Assert.Equal(ProcessResult.Continue, result);
		}

		// TODO: further processing tests
	}
}
