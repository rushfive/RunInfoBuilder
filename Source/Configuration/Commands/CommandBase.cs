using R5.RunInfoBuilder.Configuration;
using System.Collections.Generic;

namespace R5.RunInfoBuilder
{
	public abstract class CommandBase<TRunInfo> : CoreConfigurable
		where TRunInfo : class
	{
		public string Description { get; set; }
		public string HelpText { get; set; }
		public List<ArgumentBase<TRunInfo>> Arguments { get; set; } = new List<ArgumentBase<TRunInfo>>();
		public List<OptionBase<TRunInfo>> Options { get; set; } = new List<OptionBase<TRunInfo>>();
	}
}
