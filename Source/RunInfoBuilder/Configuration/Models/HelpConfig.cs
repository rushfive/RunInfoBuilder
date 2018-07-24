using R5.RunInfoBuilder.Help;
using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder.Configuration
{
	internal class HelpConfig<TRunInfo>
		where TRunInfo : class
	{
		internal string ProgramDescription { get; }
		internal Action<HelpCallbackContext<TRunInfo>> Callback { get; }
		internal bool IgnoreCase { get; }
		internal List<string> Triggers { get; }
		// todo: Formatter?
		internal bool InvokeOnValidationFail { get; }

		internal HelpConfig(
			string programDescription,
			Action<HelpCallbackContext<TRunInfo>> callback,
			bool ignoreCase,
			List<string> triggers,
			bool invokeOnValidationFail)
		{
			ProgramDescription = programDescription;
			Callback = callback;
			IgnoreCase = ignoreCase;
			Triggers = triggers;
			InvokeOnValidationFail = invokeOnValidationFail;
		}
	}
}
