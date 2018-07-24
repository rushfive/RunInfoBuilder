namespace R5.RunInfoBuilder.Pipeline
{
	public abstract class CallbackContext<TRunInfo>
		where TRunInfo : class
	{
		public string[] ProgramArguments { get; }
		public TRunInfo RunInfo { get; }
		
		public CallbackContext(string[] programArguments, TRunInfo runInfo )
		{
			ProgramArguments = programArguments;
			RunInfo = runInfo;
		}
	}
}
