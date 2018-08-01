namespace R5.RunInfoBuilder.Configuration
{
    public class BuildContext<TRunInfo>
		where TRunInfo : class
    {
		public string[] ProgramArguments { get; }
		public TRunInfo RunInfo { get; }

		internal BuildContext(
			string[] programArguments,
			TRunInfo runInfo)
		{
			ProgramArguments = programArguments;
			RunInfo = runInfo;
		}
	}
}
