using R5.RunInfoBuilder.Pipeline;

namespace R5.RunInfoBuilder
{
	public class PreProcessContext<TRunInfo> : CallbackContext<TRunInfo>
		where TRunInfo : class
	{
		internal PreProcessContext(string[] programArguments, TRunInfo runInfo)
			: base(programArguments, runInfo)
		{
		}
	}
}
