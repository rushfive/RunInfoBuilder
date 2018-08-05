using OLD.ArgumentParser;
using OLD.Configuration;
using System;
using System.Collections.Generic;

namespace OLD.Process
{
	internal interface IProcessContextsFactory<TRunInfo>
		where TRunInfo : class
	{
		ValidationContext CreateValidationContext();

		Func<ProgramArgument, ProcessContext<TRunInfo>> CreateProcessContextFactory(List<ProgramArgument> arguments, TRunInfo runInfo);
	}

	internal class ProcessContextsFactory<TRunInfo> : IProcessContextsFactory<TRunInfo>
		where TRunInfo : class
	{
		private ArgumentConfig _argumentConfig { get; }
		private CommandConfig _commandConfig { get; }
		private OptionConfig _optionConfig { get; }
		private ProcessConfig _processConfig { get; }
		private IParser _parser { get; }

		public ProcessContextsFactory(
			ArgumentConfig argumentConfig,
			CommandConfig commandConfig,
			OptionConfig optionConfig,
			ProcessConfig processConfig,
			IParser parser)
		{
			_argumentConfig = argumentConfig;
			_commandConfig = commandConfig;
			_optionConfig = optionConfig;
			_processConfig = processConfig;
			_parser = parser;
		}

		public ValidationContext CreateValidationContext() => new ValidationContext(_argumentConfig, _commandConfig, _optionConfig, _processConfig);

		public Func<ProgramArgument, ProcessContext<TRunInfo>> CreateProcessContextFactory(
			List<ProgramArgument> arguments, TRunInfo runInfo) =>
				(ProgramArgument argument) => new ProcessContext<TRunInfo>(argument, arguments, runInfo, _parser);
	}
}
