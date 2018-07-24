using System;
using System.Collections.Generic;
using System.Linq;

namespace R5.RunInfoBuilder.Configuration
{
	public class VersionConfigBuilder
	{
		private Action _callback { get; set; }
		private bool _ignoreCase { get; set; }
		private List<string> _triggers { get; set; }

		public bool IsValid() => _callback != null && _triggers.Any();

		internal VersionConfigBuilder()
		{
			_callback = null;
			_ignoreCase = false;
			_triggers = new List<string>();
		}

		public VersionConfigBuilder SetCallback(Action callback)
		{
			_callback = callback ?? throw new ArgumentNullException(nameof(callback), "Version callback must be provided.");
			return this;
		}

		public VersionConfigBuilder IgnoreCase()
		{
			_ignoreCase = true;
			return this;
		}

		public VersionConfigBuilder SetTriggers(params string[] triggers)
		{
			if (triggers == null || !triggers.Any())
			{
				throw new ArgumentNullException(nameof(triggers), "Triggers must be provided.");
			}

			_triggers.Clear();
			_triggers.AddRange(triggers);
			return this;
		}

		internal VersionConfig Build()
		{
			return new VersionConfig(_callback, _ignoreCase, _triggers);
		}
	}
}
