using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Processor.Stages;

namespace R5.RunInfoBuilder
{
	public abstract class ArgumentBase<TRunInfo> : CoreConfigurable
		where TRunInfo : class
	{
		public string HelpToken { get; set; }

		internal abstract Stage<TRunInfo> ToStage();

		internal abstract string GetHelpToken();
	}
}
