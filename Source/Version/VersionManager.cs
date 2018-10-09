using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder
{
	public class VersionManager
	{
		private static readonly string[] _defaultTriggers = new string[]
		{
			"--version", "-v", "/version"
		};

		private string _version { get; set; }
		private List<string> _triggers { get; }

		internal VersionManager()
		{
			_version = "1.0.0";
			_triggers = new List<string>(_defaultTriggers);
		}

		public VersionManager Set(string version)
		{
			if (string.IsNullOrWhiteSpace(version))
			{
				throw new ArgumentNullException(nameof(version), "Version must be provided.");
			}

			_version = version;
			return this;
		}

		public VersionManager SetTriggers(params string[] triggers)
		{
			if (triggers == null || !triggers.Any())
			{
				throw new ArgumentNullException(nameof(triggers), "Triggers must be provided.");
			}

			_triggers.Clear();
			_triggers.AddRange(triggers);

			return this;
		}

		internal void Invoke()
		{
			Console.WriteLine(_version);
		}

		internal bool IsTrigger(string token)
		{
			return _triggers.Contains(token);
		}
	}
}
