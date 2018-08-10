using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
    public class DefaultCommand<TRunInfo> where TRunInfo : class
    {
		public string Description { get; set; }
		public string HelpText { get; set; }
		public Func<ProcessContext<TRunInfo>, ProcessNodeResult> Callback { get; set; }
		public List<OptionBase<TRunInfo>> Options { get; set; } = new List<OptionBase<TRunInfo>>();

		internal void Validate(Type parentType, string parentKey)
		{
			if (!Options.NullOrEmpty())
			{
				Options.ForEach(o => o.Validate(parentType: null, parentKey: null));
			}
		}
	}
}
