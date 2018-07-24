namespace R5.RunInfoBuilder.Configuration
{
	public class ProcessConfigBuilder<TRunInfo>
		where TRunInfo : class
	{
		public ProcessHooksBuilder<TRunInfo> Hooks { get; }

		private HandleUnresolvedArgument _handleUnresolvedArgument { get; set; }

		internal ProcessConfigBuilder()
		{
			Hooks = new ProcessHooksBuilder<TRunInfo>();
			_handleUnresolvedArgument = HandleUnresolvedArgument.NotAllowed;
		}

		public ProcessConfigBuilder<TRunInfo> AllowUnresolvedArgumentsButThrowOnProcess()
		{
			_handleUnresolvedArgument = HandleUnresolvedArgument.AllowedButThrowOnProcess;
			return this;
		}

		public ProcessConfigBuilder<TRunInfo> AllowUnresolvedArgumentsButSkipOnProcess()
		{
			_handleUnresolvedArgument = HandleUnresolvedArgument.AllowedButSkipOnProcess;
			return this;
		}

		internal (ProcessConfig, ProcessHooksConfig<TRunInfo>) Build()
		{
			var config = new ProcessConfig(_handleUnresolvedArgument);
			ProcessHooksConfig<TRunInfo> processHooks = Hooks.Build();

			return (config, processHooks);
		}
	}
}
