using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R5.RunInfoBuilder.Version
{
	internal interface IVersionManager
	{
		bool IsTrigger(string trigger);

		void InvokeCallback();
	}

	internal class VersionManager : IVersionManager
	{
		private IRestrictedKeyValidator _keyValidator { get; }
		private HashSet<string> _triggers { get; set; }
		private Action _callback { get; set; }

		public VersionManager(IRestrictedKeyValidator keyValidator)
		{
			_keyValidator = keyValidator;
			_triggers = null;
			_callback = null;
		}

		public VersionManager Configure(VersionConfig config)
		{
			_callback = config.Callback;

			bool hasRestrictedTrigger = config.Triggers.Any(t => _keyValidator.IsRestrictedKey(t));
			if (hasRestrictedTrigger)
			{
				throw new InvalidOperationException("At least one of the triggers is restricted.");
			}

			var comparer = config.IgnoreCase
				? StringComparer.OrdinalIgnoreCase
				: StringComparer.Ordinal;

			_triggers = new HashSet<string>(config.Triggers, comparer);

			_keyValidator.AddRestrictedKeys(config.Triggers);

			return this;
		}
		
		public bool IsTrigger(string trigger)
		{
			if (string.IsNullOrWhiteSpace(trigger))
			{
				throw new ArgumentNullException(nameof(trigger), "Version trigger must be provided.");
			}

			return _triggers.Contains(trigger);
		}

		public void InvokeCallback()
		{
			if (_callback == null)
			{
				throw new InvalidOperationException("Cannot invoke version callback because it hasn't been set.");
			}
			_callback();
		}
	}
}
