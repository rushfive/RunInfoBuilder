using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Validators
{
	internal class ValidationContext
	{
		private string _parentKey { get; set; }
		private HashSet<string> _seenCommands { get; set; }
		private HashSet<string> _seenFullOptions { get; set; }
		private HashSet<char> _seenShortOptions { get; set; }

		private HashSet<string> _versionKeys { get; set; }
		private HashSet<string> _helpKeys { get; set; }

		private bool _isTopLevel { get; }

		internal ValidationContext(HashSet<string> versionKeys,
			HashSet<string> helpKeys) : this()
		{
			_isTopLevel = true;
			_versionKeys = versionKeys;
			_helpKeys = helpKeys;
		}

		internal ValidationContext(string parentKey) : this()
		{
			_isTopLevel = false;
			_parentKey = parentKey;
		}

		private ValidationContext()
		{
			_seenCommands = new HashSet<string>();
			_seenFullOptions = new HashSet<string>();
			_seenShortOptions = new HashSet<char>();
		}

		internal bool IsValidCommand(string key)
		{
			if (_seenCommands.Contains(key))
			{
				return false;
			}

			if (!_isTopLevel)
			{
				return true;
			}

			return !_versionKeys.Contains(key) && !_helpKeys.Contains(key);
		}

		internal bool IsValidFullOption(string key) => !_seenFullOptions.Contains(key);

		internal bool IsValidShortOption(char key) => !_seenShortOptions.Contains(key);

		internal void MarkCommandSeen(string key) => _seenCommands.Add(key);
		internal void MarkFullOptionSeen(string key) => _seenFullOptions.Add(key);
		internal void MarkShortOptionSeen(char key) => _seenShortOptions.Add(key);


		

	}
}
