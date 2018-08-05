using System;
using System.Collections.Generic;

namespace OLD.Configuration
{
	internal class VersionConfig
	{
		internal Action Callback { get; }
		internal bool IgnoreCase { get; }
		internal List<string> Triggers { get; }

		internal VersionConfig(
			Action callback,
			bool ignoreCase,
			List<string> triggers)
		{
			Callback = callback;
			IgnoreCase = ignoreCase;
			Triggers = triggers;
		}
	}
}
