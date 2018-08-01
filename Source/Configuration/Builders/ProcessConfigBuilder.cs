namespace R5.RunInfoBuilder.Configuration
{
	public class ProcessConfigBuilder<TRunInfo>
		where TRunInfo : class
	{
		private HandleUnresolvedArgument _handleUnresolvedArgument { get; set; }
		private bool _duplicateArgumentsAllowed { get; set; }

		internal ProcessConfigBuilder()
		{
			_handleUnresolvedArgument = HandleUnresolvedArgument.NotAllowed;
			_duplicateArgumentsAllowed = true;
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

		public ProcessConfigBuilder<TRunInfo> PreventDuplicateArguments()
		{
			_duplicateArgumentsAllowed = false;
			return this;
		}

		internal ProcessConfig Build()
		{
			return new ProcessConfig(_handleUnresolvedArgument, _duplicateArgumentsAllowed);
		}
	}
}
