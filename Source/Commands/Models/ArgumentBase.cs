using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public abstract class ArgumentBase<TRunInfo> where TRunInfo : class
	{
		public Func<CallbackContext<TRunInfo>, CallbackResult> Callback { get; set; }
		public string HelpText { get; set; }

		internal abstract void Validate(Type parentType, string parentKey);
	}
}
