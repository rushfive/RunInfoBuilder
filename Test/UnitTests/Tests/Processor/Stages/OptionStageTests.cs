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
			List<IOption> options = null)
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
			
			var context = new ProcessContext<TestRunInfo>(runInfo, args, subCommands, options);

			return new OptionStage<TestRunInfo>(new ArgumentParser(), context);
		}

		[Fact]
		public void NoMore_ProgramArguments_ReturnsEndResult()
		{
			var stage = GetBaseStage();

			var result = stage.ProcessStage();
		}
	}
}
