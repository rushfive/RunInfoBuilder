using R5.RunInfoBuilder.Pipeline;

namespace R5.RunInfoBuilder
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
