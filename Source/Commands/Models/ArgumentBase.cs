using R5.RunInfoBuilder.Processor.Stages;

namespace R5.RunInfoBuilder.Commands
{
	public abstract class ArgumentBase<TRunInfo>
		where TRunInfo : class
	{
		public string HelpText { get; set; }

		internal abstract void Validate(int commandLevel);

		internal abstract Stage<TRunInfo> ToStage();
	}
}
