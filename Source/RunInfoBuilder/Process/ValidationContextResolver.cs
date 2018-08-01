using R5.RunInfoBuilder.Configuration;

namespace R5.RunInfoBuilder.Process
{
	internal interface IValidationContextFactory
	{
		ValidationContext Create();
	}

	internal class ValidationContextFactory : IValidationContextFactory
	{
		private ArgumentConfig _argumentConfig { get; }
		private CommandConfig _commandConfig { get; }
		private OptionConfig _optionConfig { get; }
		private ProcessConfig _processConfig { get; }

		public ValidationContextFactory(
			ArgumentConfig argumentConfig,
			CommandConfig commandConfig,
			OptionConfig optionConfig,
			ProcessConfig processConfig)
		{
			_argumentConfig = argumentConfig;
			_commandConfig = commandConfig;
			_optionConfig = optionConfig;
			_processConfig = processConfig;
		}

		public ValidationContext Create() => new ValidationContext(_argumentConfig, _commandConfig, _optionConfig, _processConfig);
	}
}
