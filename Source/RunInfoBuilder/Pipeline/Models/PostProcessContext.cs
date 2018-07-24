namespace R5.RunInfoBuilder.Pipeline
{
    public class PostProcessContext<TRunInfo> : CallbackContext<TRunInfo>
		where TRunInfo : class
	{
		public bool KilledBuildProcess { get; }

		internal PostProcessContext(
			string[] programArguments,
			TRunInfo runInfo,
			bool killedBuildProcess)
			: base(programArguments, runInfo)
		{
			KilledBuildProcess = killedBuildProcess;
		}
    }
}
