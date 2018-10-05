using R5.RunInfoBuilder.Processor.Stages;

namespace R5.RunInfoBuilder
{
	public abstract class ArgumentBase<TRunInfo>
		where TRunInfo : class
	{
		public string HelpToken { get; set; }

		internal abstract void Validate(int commandLevel);

		internal abstract Stage<TRunInfo> ToStage();

		internal abstract string GetHelpToken();
	}
}
