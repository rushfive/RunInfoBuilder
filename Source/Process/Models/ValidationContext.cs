using OLD.Configuration;
using System.Collections.Generic;

namespace OLD.Process
{
	internal class ValidationContext
	{
		internal bool ArgumentSeen { get; private set; }
		internal bool CommandSeen { get; private set; }
		internal bool OptionSeen { get; private set; }

		internal ArgumentConfig ArgumentConfig { get; }
		internal CommandConfig CommandConfig { get; }
		internal OptionConfig OptionConfig { get; }
		internal ProcessConfig ProcessConfig { get; }

		private HashSet<string> _seenHash { get; }

		internal ValidationContext(
			ArgumentConfig argumentConfig,
			CommandConfig commandConfig,
			OptionConfig optionConfig,
			ProcessConfig processConfig)
		{
			_seenHash = new HashSet<string>();

			ArgumentConfig = argumentConfig;
			CommandConfig = commandConfig;
			OptionConfig = optionConfig;
			ProcessConfig = processConfig;
		}

		internal bool HasSeenArgument() => ArgumentSeen;
		internal bool HasSeenCommand() => CommandSeen;
		internal bool HasSeenOption() => OptionSeen;
		internal bool HasSeen(string argumentToken) => _seenHash.Contains(argumentToken);

		internal void MarkArgumentSeen(string token)
		{
			ArgumentSeen = true;
			_seenHash.Add(token);
		}

		internal void MarkCommandSeen(string token)
		{
			CommandSeen = true;
			_seenHash.Add(token);
		}

		internal void MarkOptionSeen(string token)
		{
			OptionSeen = true;
			_seenHash.Add(token);
		}
	}
}
