using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
    public class DefaultCommand<TRunInfo> where TRunInfo : class
    {
		public string Description { get; set; }
		public string HelpText { get; set; }
		public Func<CallbackContext<TRunInfo>, ProcessStageResult> Callback { get; set; }
		public List<IOption> Options { get; set; } = new List<IOption>();

		internal void Validate(Type parentType, string parentKey)
		{
			//if (!Options.NullOrEmpty())
			//{
			//	Options.ForEach(o => o.Validate(parentType: null, parentKey: null));
			//}
		}
	}
}
