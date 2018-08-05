using System.Collections.Generic;
using System.Linq;

namespace OLD.Store
{
	// todo: run through and see if all these used
	internal interface IArgumentMetadata<TRunInfo>
		where TRunInfo : class
	{
		bool ArgumentExists(string key);

		bool CommandExists(string key);

		bool FullOptionExists(string key);

		bool ShortOptionExists(char key);

		ArgumentMetadata GetArgument(string key);

		CommandMetadata<TRunInfo> GetCommand(string key);

		OptionMetadata GetFullOption(string key);

		OptionMetadata GetShortOption(char key);

		List<ArgumentMetadata> GetArguments();

		List<CommandMetadata<TRunInfo>> GetCommands();

		List<OptionMetadata> GetOptions();

		void AddArgument(string key, ArgumentMetadata metadata);

		void AddCommand(string key, CommandMetadata<TRunInfo> metadata);

		void AddOption(string fullKey, char? shortKey, OptionMetadata metadata);
	}

	internal class ArgumentMetadata<TRunInfo> : IArgumentMetadata<TRunInfo>
		where TRunInfo : class
	{
		private Dictionary<string, ArgumentMetadata> _argumentMap { get; }
		private Dictionary<string, CommandMetadata<TRunInfo>> _commandMap { get; }
		private Dictionary<string, OptionMetadata> _fullOptionMap { get; }
		private Dictionary<char, string> _shortOptionMap { get; }

		public ArgumentMetadata()
		{
			_argumentMap = new Dictionary<string, ArgumentMetadata>();
			_commandMap = new Dictionary<string, CommandMetadata<TRunInfo>>();
			_fullOptionMap = new Dictionary<string, OptionMetadata>();
			_shortOptionMap = new Dictionary<char, string>();
		}

		public bool ArgumentExists(string key)
		{
			return _argumentMap.ContainsKey(key);
		}

		public bool CommandExists(string key)
		{
			return _commandMap.ContainsKey(key);
		}

		public bool FullOptionExists(string key)
		{
			return _fullOptionMap.ContainsKey(key);
		}

		public bool ShortOptionExists(char key)
		{
			return _shortOptionMap.ContainsKey(key);
		}

		public ArgumentMetadata GetArgument(string key)
		{
			return _argumentMap[key];
		}

		public CommandMetadata<TRunInfo> GetCommand(string key)
		{
			return _commandMap[key];
		}

		public OptionMetadata GetFullOption(string key)
		{
			return _fullOptionMap[key];
		}

		public OptionMetadata GetShortOption(char key)
		{
			string fullKey = _shortOptionMap[key];
			return _fullOptionMap[fullKey];
		}

		public List<ArgumentMetadata> GetArguments()
		{
			return _argumentMap.Values.ToList();
		}

		public List<CommandMetadata<TRunInfo>> GetCommands()
		{
			return _commandMap.Values.ToList();
		}

		public List<OptionMetadata> GetOptions()
		{
			return _fullOptionMap.Values.ToList();
		}

		public void AddArgument(string key, ArgumentMetadata metadata)
		{
			_argumentMap.Add(key, metadata);
		}

		public void AddCommand(string key, CommandMetadata<TRunInfo> metadata)
		{
			_commandMap.Add(key, metadata);
		}

		public void AddOption(string fullKey, char? shortKey, OptionMetadata metadata)
		{
			_fullOptionMap.Add(fullKey, metadata);

			if (shortKey.HasValue)
			{
				_shortOptionMap.Add(shortKey.Value, fullKey);
			}
		}
	}
}
