using R5.RunInfoBuilder.Processor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace R5.RunInfoBuilder.Processor.Functions
{
	

	internal class OptionFunctions<TRunInfo>
			where TRunInfo : class
	{
		//private Dictionary<string, (Action<TRunInfo, object> setter, Type valueType)> _fullOptionSetters { get; set; }
		//private Dictionary<char, (Action<TRunInfo, object> setter, Type valueType)> _shortOptionSetters { get; set; }

		private Dictionary<string, OptionProcessInfo<TRunInfo>> _fullOptionInfo { get; }
		private Dictionary<char, OptionProcessInfo<TRunInfo>> _shortOptionInfo { get; }

		private HashSet<string> _fullBoolTypeKeys { get; }
		private HashSet<char> _shortBoolTypeKeys { get; }

		internal OptionFunctions(List<OptionBase<TRunInfo>> options)
		{
			//_fullOptionSetters = new Dictionary<string, (Action<TRunInfo, object>, Type)>();
			//_shortOptionSetters = new Dictionary<char, (Action<TRunInfo, object>, Type)>();

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
			//void AddSetters(OptionBase<TRunInfo> option)
			//{
			//	(Action<TRunInfo, object>, Type) setter = createSetter(option);

			//	var (fullKey, shortKey) = OptionTokenizer.TokenizeKeyConfiguration(option.Key);

			//	_fullOptionSetters.Add(fullKey, setter);

			//	if (shortKey != null)
			//	{
			//		_shortOptionSetters.Add(shortKey.Value, setter);
			//	}
			//}

			//(Action<TRunInfo, object> setter, Type valueType) createSetter(OptionBase<TRunInfo> option)
			//{
			//	dynamic opt = option;
			//	PropertyInfo propertyInfo = ReflectionHelper<TRunInfo>.GetPropertyInfoFromExpression(opt.Property);

			//	Type valueType = propertyInfo.PropertyType;

			//	Action<TRunInfo, object> setter = (runInfo, value) =>
			//	{
			//		if (value.GetType() != valueType)
			//		{
			//			throw new InvalidOperationException($"'{value}' is not a valid '{valueType}' type.");
			//		}

			//		propertyInfo.SetValue(runInfo, value);
			//	};

			//	return (setter, valueType);
			//}

			///////// NEXT: replace the below internal GetOptionValueSetter methods with getProcessIfno

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

		//internal (Action<TRunInfo, object> Setter, Type ValueType) GetOptionValueSetter(string fullKey)
		//{
		//	if (!_fullOptionSetters.TryGetValue(fullKey, out (Action<TRunInfo, object>, Type) setter))
		//	{
		//		throw new InvalidOperationException($"'{fullKey}' is not a valid option full key.");
		//	}

		//	return setter;
		//}

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

		//internal (Action<TRunInfo, object> Setter, Type ValueType) GetOptionValueSetter(char shortKey)
		//{
		//	if (!_shortOptionSetters.TryGetValue(shortKey, out (Action<TRunInfo, object>, Type) setter))
		//	{
		//		throw new InvalidOperationException($"'{shortKey}' is not a valid option short key.");
		//	}

		//	return setter;
		//}

		//internal List<(Action<TRunInfo, object> Setter, Type ValueType)> GetOptionValueSetters(List<char> stackedKeys)
		//{
		//	var setters = new List<(Action<TRunInfo, object>, Type)>();

		//	foreach (char key in stackedKeys)
		//	{
		//		if (!_shortOptionSetters.TryGetValue(key, out (Action<TRunInfo, object>, Type valueType) setter))
		//		{
		//			throw new InvalidOperationException($"'{key}' is not a valid option short key.");
		//		}

		//		if (setter.valueType != typeof(bool))
		//		{
		//			throw new InvalidOperationException($"Key '{key}' is part of a stacked option token but not mapped to a bool type.");
		//		}

		//		setters.Add(setter);
		//	}

		//	return setters;
		//}

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
