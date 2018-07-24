using System;
using System.Collections.Generic;
using System.Linq;

namespace R5.RunInfoBuilder.Validators
{
	internal interface IRestrictedKeyValidator
	{
		bool IsRestrictedKey(string key);

		bool IsRestrictedKey(char key);

		void AddRestrictedKey(string key);

		void AddRestrictedKey(char key);

		void AddRestrictedKeys(IEnumerable<string> keys);

		void AddRestrictedKeys(IEnumerable<char> keys);

		void ClearKeys(IEnumerable<string> keys);

		void ClearKeys(IEnumerable<char> keys);
	}

	internal class RestrictedKeyValidator : IRestrictedKeyValidator
	{
		private HashSet<string> _restrictedKeys { get; }

		public RestrictedKeyValidator()
		{
			_restrictedKeys = new HashSet<string>();
		}

		public bool IsRestrictedKey(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentNullException(nameof(key), "Key must be provided.");
			}
			return _restrictedKeys.Contains(key);
		}

		public bool IsRestrictedKey(char key)
		{
			return _restrictedKeys.Contains(key.ToString());
		}

		public void AddRestrictedKey(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentNullException(nameof(key), "Key must be provided.");
			}
			if (_restrictedKeys.Contains(key))
			{
				throw new ArgumentException($"Key '{key}' has already been added.", nameof(key));
			}

			_restrictedKeys.Add(key);
		}

		public void AddRestrictedKey(char key)
		{
			string keyString = key.ToString();

			if (_restrictedKeys.Contains(keyString))
			{
				throw new ArgumentException($"Key '{keyString}' has already been added.", nameof(key));
			}

			_restrictedKeys.Add(keyString);
		}

		public void AddRestrictedKeys(IEnumerable<string> keys)
		{
			if (keys == null)
			{
				throw new ArgumentNullException(nameof(keys), "Keys must be provided.");
			}

			if (keys.Any(k => _restrictedKeys.Contains(k)))
			{
				throw new ArgumentException("At least one of the keys has already been added.", nameof(keys));
			}

			_restrictedKeys.AddRange(keys);
		}

		public void AddRestrictedKeys(IEnumerable<char> keys)
		{
			if (keys == null)
			{
				throw new ArgumentNullException(nameof(keys), "Keys must be provided.");
			}

			IEnumerable<string> keyStrings = keys.Select(k => k.ToString());

			if (keyStrings.Any(k => _restrictedKeys.Contains(k)))
			{
				throw new ArgumentException("At least one of the keys has already been added.", nameof(keys));
			}

			_restrictedKeys.AddRange(keyStrings);
		}

		public void ClearKeys(IEnumerable<string> keys)
		{
			if (keys == null)
			{
				throw new ArgumentNullException(nameof(keys), "Keys must be provided.");
			}

			var keysHash = new HashSet<string>(keys);

			_restrictedKeys.RemoveWhere(k => keysHash.Contains(k));
		}

		public void ClearKeys(IEnumerable<char> keys)
		{
			if (keys == null)
			{
				throw new ArgumentNullException(nameof(keys), "Keys must be provided.");
			}

			var keysHash = new HashSet<string>(keys.Select(k => k.ToString()));

			_restrictedKeys.RemoveWhere(k => keysHash.Contains(k));
		}
	}
}
