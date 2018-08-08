using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
    public class DefaultCommand<TRunInfo> where TRunInfo : class
    {
		public string Description { get; set; }
		public string Usage { get; set; }
		public Func<CallbackContext<TRunInfo>, CallbackResult> Callback { get; set; }
		public List<ArgumentBase<TRunInfo>> Arguments { get; set; } = new List<ArgumentBase<TRunInfo>>();
		public List<OptionWithArguments<TRunInfo>> Options { get; set; } = new List<OptionWithArguments<TRunInfo>>();

		internal void Validate()
		{

		}
	}
}
