using R5.RunInfoBuilder.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace R5.RunInfoBuilder.Processor.Models
{
	// todo move
	internal enum OptionType
	{
		Full,
		Short,
		Stacked
	}

	internal class OptionsInfo<TRunInfo> where TRunInfo : class
	{
		private Dictionary<string, (Action<TRunInfo, object> setter, Type valueType)> _fullOptionSetters { get; }
		private Dictionary<char, (Action<TRunInfo, object> setter, Type valueType)> _shortOptionSetters { get; }

		internal OptionsInfo(List<IOption> options)
		{
			_fullOptionSetters = new Dictionary<string, (Action<TRunInfo, object>, Type)>();
			_shortOptionSetters = new Dictionary<char, (Action<TRunInfo, object>, Type)>();

			InitializeSetterMaps(options);
		}

		private void InitializeSetterMaps(List<IOption> options)
		{
			foreach (IOption option in options)
			{
				(Action<TRunInfo, object>, Type) setter = CreateSetter(option);

				var (fullKey, shortKey) = OptionHelper.Tokenize(option.Key);

				_fullOptionSetters.Add(fullKey, setter);

				if (shortKey != null)
				{
					_shortOptionSetters.Add(shortKey.Value, setter);
				}
			}
		}

		private (Action<TRunInfo, object> Setter, Type ValueType) CreateSetter(IOption option)
		{
			dynamic opt = option;
			PropertyInfo propertyInfo = ReflectionHelper<TRunInfo>.GetPropertyInfoFromExpression(opt.Property);

			Type valueType = propertyInfo.PropertyType;

			Action<TRunInfo, object> setter = (runInfo, value) =>
			{
				if (value.GetType() != valueType)
				{
					throw new InvalidOperationException($"'{value}' is not a valid '{valueType}' type.");
				}

				propertyInfo.SetValue(runInfo, value);
			};

			return (setter, valueType);
		}

		internal List<(Action<TRunInfo, object> Setter, Type ValueType)> GetSetters(string programArgument)
		{
			var result = new List<(Action<TRunInfo, object>, Type)>();

			if (!programArgument.StartsWith("--") && !programArgument.StartsWith("-"))
			{
				throw new InvalidOperationException($"'{programArgument}' is not a valid option.");
			}

			// full
			if (programArgument.StartsWith("--"))
			{
				string fullKey = programArgument.Substring(2);

				if (!_fullOptionSetters.TryGetValue(fullKey, out (Action<TRunInfo, object>, Type) setter))
				{
					throw new InvalidOperationException($"'{fullKey}' is not a valid option full key.");
				}

				result.Add(setter);
				return result;
			}

			// short
			if (programArgument.Length == 2)
			{
				char shortKey = programArgument.Skip(1).Single();

				AddSettersByShortKeys(new List<char> { shortKey }, result);
				
				return result;
			}

			// stacked short
			AddSettersByShortKeys(programArgument.Skip(1), result);
			return result;
		}

		private void AddSettersByShortKeys(IEnumerable<char> keys, List<(Action<TRunInfo, object>, Type)> setters)
		{
			foreach(char key in keys)
			{
				if (!_shortOptionSetters.TryGetValue(key, out (Action<TRunInfo, object>, Type) setter))
				{
					throw new InvalidOperationException($"'{key}' is not a valid option short key.");
				}

				setters.Add(setter);
			}
		}

		internal bool IsOption(string programArgument)
		{
			if (!programArgument.StartsWith("--") && !programArgument.StartsWith("-"))
			{
				return false;
			}

			if (programArgument.StartsWith("--"))
			{
				return IsFullOption(programArgument);
			}

			if (programArgument.Length == 2)
			{
				return IsShortOption(programArgument.Skip(1).Single());
			}

			return IsStackedOption(new string(programArgument.Skip(1).ToArray()));
		}

		internal OptionType? GetType(string programArgument)
		{
			if (!programArgument.StartsWith("--") && !programArgument.StartsWith("-"))
			{
				return null;
			}

			bool valid;

			if (programArgument.StartsWith("--"))
			{
				valid = IsFullOption(programArgument);
				return valid ? OptionType.Full : (OptionType?)null;
			}

			if (programArgument.Length == 2)
			{
				valid = IsShortOption(programArgument.Skip(1).Single());
				return valid ? OptionType.Short : (OptionType?)null;
			}

			valid = IsStackedOption(new string(programArgument.Skip(1).ToArray()));
			return valid ? OptionType.Stacked : (OptionType?)null;
		}

		internal bool IsFullOption(string s) => _fullOptionSetters.ContainsKey(s);

		internal bool IsShortOption(char c) => _shortOptionSetters.ContainsKey(c);

		internal bool IsStackedOption(string s)
		{
			var chars = s.ToCharArray();

			return chars.Length == chars.Distinct().Count()
				&& chars.All(_shortOptionSetters.ContainsKey);
		}
	}
}
