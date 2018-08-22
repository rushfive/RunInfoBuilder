using R5.RunInfoBuilder.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Validators
{
	internal interface IRestrictedKeyValidator
	{
		bool IsRestricted(string key);

		void Add(string key);

		void Add(List<string> keys);

		void Remove(string key);
	}

	// validates top-level keys only by uniqueness
	internal class RestrictedKeyValidator : IRestrictedKeyValidator
	{
		private readonly HashSet<string> _restricted = new HashSet<string>();

		public RestrictedKeyValidator()
		{
			_restricted.Add(CommandStore.DefaultKey);
		}

		public bool IsRestricted(string key)
			=> _restricted.Contains(key);

		public void Add(string key)
			=> _restricted.Add(key);

		public void Add(List<string> keys)
			=> keys.ForEach(Add);

		public void Remove(string key)
			=> _restricted.Remove(key);
	}
}
