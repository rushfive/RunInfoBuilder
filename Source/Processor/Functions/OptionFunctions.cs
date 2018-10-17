using R5.RunInfoBuilder.Processor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R5.RunInfoBuilder.Processor.Functions
{
	internal class OptionFunctions<TRunInfo>
			where TRunInfo : class
	{
		private Dictionary<string, OptionProcessInfo<TRunInfo>> _fullOptionInfo { get; }
		private Dictionary<char, OptionProcessInfo<TRunInfo>> _shortOptionInfo { get; }
		private HashSet<string> _fullBoolTypeKeys { get; }
		private HashSet<char> _shortBoolTypeKeys { get; }

		internal OptionFunctions(List<OptionBase<TRunInfo>> options)
		{
			_fullOptionInfo = new Dictionary<string, OptionProcessInfo<TRunInfo>>();
			_shortOptionInfo = new Dictionary<char, OptionProcessInfo<TRunInfo>>();
			_fullBoolTypeKeys = new HashSet<string>();
			_shortBoolTypeKeys = new HashSet<char>();
			
			InitializeMaps(options);
		}

		private void InitializeMaps(List<OptionBase<TRunInfo>> options)
		{
			foreach (OptionBase<TRunInfo> option in options)
			{
				addProcessInfo(option);

				if (option.Type == typeof(bool))
				{
					addToBoolMaps(option);
				}
			}

			// local functions
			void addProcessInfo(OptionBase<TRunInfo> option)
			{
				var (fullKey, shortKey) = OptionTokenizer.TokenizeKeyConfiguration(option.Key);
				OptionProcessInfo<TRunInfo> processInfo = option.GetProcessInfo();

				_fullOptionInfo.Add(fullKey, processInfo);

				if (shortKey != null)
				{
					_shortOptionInfo.Add(shortKey.Value, processInfo);
				}
			}

			void addToBoolMaps(OptionBase<TRunInfo> option)
			{
				var (fullKey, shortKey) = OptionTokenizer.TokenizeKeyConfiguration(option.Key);

				_fullBoolTypeKeys.Add(fullKey);

				if (shortKey != null)
				{
					_shortBoolTypeKeys.Add(shortKey.Value);
				}
			}
		}

		internal OptionProcessInfo<TRunInfo> GetOptionProcessInfo(string fullKey)
		{
			if (!_fullOptionInfo.TryGetValue(fullKey, out OptionProcessInfo<TRunInfo> processInfo))
			{
				throw new InvalidOperationException($"'{fullKey}' is not a valid option full key.");
			}
			return processInfo;
		}

		internal OptionProcessInfo<TRunInfo> GetOptionProcessInfo(char shortKey)
		{
			if (!_shortOptionInfo.TryGetValue(shortKey, out OptionProcessInfo<TRunInfo> processInfo))
			{
				throw new InvalidOperationException($"'{shortKey}' is not a valid option short key.");
			}
			return processInfo;
		}

		internal List<OptionProcessInfo<TRunInfo>> GetOptionProcessInfos(List<char> stackedKeys)
		{
			var infos = new List<OptionProcessInfo<TRunInfo>>();

			foreach (char key in stackedKeys)
			{
				if (!_shortOptionInfo.TryGetValue(key, out OptionProcessInfo<TRunInfo> processInfo))
				{
					throw new InvalidOperationException($"'{key}' is not a valid option short key.");
				}

				if (processInfo.Type != typeof(bool))
				{
					throw new InvalidOperationException($"Key '{key}' is part of a stacked option token but not mapped to a bool type.");
				}

				infos.Add(processInfo);
			}

			return infos;
		}

		internal bool IsOption(string programArgument)
		{
			try
			{
				(OptionType type, string fullKey, List<char> shortKeys, _) = OptionTokenizer.TokenizeProgramArgument(programArgument);

				switch (type)
				{
					case OptionType.Full:
						return IsFullOption(fullKey);
					case OptionType.Short:
						return IsShortOption(shortKeys.Single());
					case OptionType.Stacked:
						return shortKeys.Count == shortKeys.Distinct().Count()
							&& shortKeys.All(_shortOptionInfo.ContainsKey);
					default:
						throw new ArgumentOutOfRangeException($"'{type}' is not a valid option type.");
				}
			}
			catch (ArgumentException)
			{
				return false;
			}

			// local functions
			bool IsFullOption(string s) => _fullOptionInfo.ContainsKey(s);

			bool IsShortOption(char c) => _shortOptionInfo.ContainsKey(c);
		}

		internal bool IsBoolType(string fullKey) => _fullBoolTypeKeys.Contains(fullKey);

		internal bool IsBoolType(char shortKey) => _shortBoolTypeKeys.Contains(shortKey);
	}
}
