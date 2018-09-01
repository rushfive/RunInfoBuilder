using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public abstract class CommandBase<TRunInfo> : ICallbackElement<TRunInfo> 
		where TRunInfo : class
	{
		public string Description { get; set; }
		public string HelpText { get; set; }
		public Func<CallbackContext<TRunInfo>, ProcessStageResult> Callback { get; set; }
		public List<ArgumentBase<TRunInfo>> Arguments { get; set; } = new List<ArgumentBase<TRunInfo>>();
		public List<OptionBase<TRunInfo>> Options { get; set; } = new List<OptionBase<TRunInfo>>();
	}

    public class DefaultCommand<TRunInfo> : CommandBase<TRunInfo>
		where TRunInfo : class
    {
		//public string Description { get; set; }
		//public string HelpText { get; set; }
		//public Func<CallbackContext<TRunInfo>, ProcessStageResult> Callback { get; set; }
		//public List<ArgumentBase<TRunInfo>> Arguments { get; set; } = new List<ArgumentBase<TRunInfo>>();
		//public List<IOption> Options { get; set; } = new List<IOption>();

		internal void Validate(Type parentType, string parentKey)
		{
			//if (!Options.NullOrEmpty())
			//{
			//	Options.ForEach(o => o.Validate(parentType: null, parentKey: null));
			//}
		}
	}
}
