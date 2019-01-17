using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Processor.Stages;

namespace R5.RunInfoBuilder
{
	/// <summary>
	/// The base abstract class Arguments derive from.
	/// </summary>
	/// <typeparam name="TRunInfo">The RunInfo type the argument's associated to.</typeparam>
	public abstract class ArgumentBase<TRunInfo>
		where TRunInfo : class
	{
		/// <summary>
		/// The token displayed in the help menu that represents this argument.
		/// </summary>
		public string HelpToken { get; set; }

		internal abstract Stage<TRunInfo> ToStage();

		internal abstract string GetHelpToken();

		internal abstract void Validate(int commandLevel);
	}
}
