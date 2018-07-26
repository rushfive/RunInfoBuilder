using R5.RunInfoBuilder.Pipeline;

namespace R5.RunInfoBuilder
{
    public class PostProcessContext<TRunInfo> : CallbackContext<TRunInfo>
		where TRunInfo : class
	{
		internal PostProcessContext(
			string[] programArguments,
			TRunInfo runInfo)
			: base(programArguments, runInfo)
		{
		}
    }
}
