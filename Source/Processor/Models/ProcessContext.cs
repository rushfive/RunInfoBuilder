using R5.RunInfoBuilder.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace R5.RunInfoBuilder.Processor.Models
{

	// Each command that is processed gets its own shared ProcessContext
	// - processing is nested only by commands:
	//   - Always starts with a top-level command
	//   - all processing is "flat", except if it has subcommands, then it can
	//     recursively nest down n levels depending on how whether subcommands have subcommands themselves

    internal class ProcessContext<TRunInfo> where TRunInfo : class
    {
		private ArgumentsQueue _programArguments { get; }
		private HashSet<string> _subCommands { get; }
		private Dictionary<string, (Action<TRunInfo, object> setter, Type valueType)> _fullOptionSetters { get; }
		private Dictionary<char, (Action<TRunInfo, object> setter, Type valueType)> _shortOptionSetters { get; }

		internal ProcessContext(
			string[] args,
			List<Command<TRunInfo>> subCommands,
			List<IOption> options)
		{
			_programArguments = new ArgumentsQueue(args);
			_subCommands = new HashSet<string>(subCommands.Select(c => c.Key));
			_fullOptionSetters = new Dictionary<string, (Action<TRunInfo, object>, Type)>();
			_shortOptionSetters = new Dictionary<char, (Action<TRunInfo, object>, Type)>();

			InitializeSetterMaps(options);
		}

		internal bool HasNext() => _programArguments.HasNext();

		internal string Peek() => _programArguments.Peek();

		internal string Dequeue() => _programArguments.Dequeue();

		internal bool NextIsSubCommand() => HasNext() && _subCommands.Contains(_programArguments.Peek());

		internal bool NextIsOption() => HasNext() && IsOption(Peek());

		internal (Action<TRunInfo, object> Setter, Type ValueType) GetOptionValueSetter(string fullKey)
		{
			if (!_fullOptionSetters.TryGetValue(fullKey, out (Action<TRunInfo, object>, Type) setter))
			{
				throw new InvalidOperationException($"'{fullKey}' is not a valid option full key.");
			}

			return setter;
		}

		internal (Action<TRunInfo, object> Setter, Type ValueType) GetOptionValueSetter(char shortKey)
		{
			if (!_shortOptionSetters.TryGetValue(shortKey, out (Action<TRunInfo, object>, Type) setter))
			{
				throw new InvalidOperationException($"'{shortKey}' is not a valid option short key.");
			}

			return setter;
		}

		internal List<(Action<TRunInfo, object> Setter, Type ValueType)> GetOptionValueSetters(List<char> stackedKeys)
		{
			var setters = new List<(Action<TRunInfo, object>, Type)>();

			foreach (char key in stackedKeys)
			{
				if (!_shortOptionSetters.TryGetValue(key, out (Action<TRunInfo, object>, Type valueType) setter))
				{
					throw new InvalidOperationException($"'{key}' is not a valid option short key.");
				}

				if (setter.valueType != typeof(bool))
				{
					throw new InvalidOperationException($"Key '{key}' is part of a stacked option token but not mapped to a bool type.");
				}

				setters.Add(setter);
			}

			return setters;
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

			// local functions
			bool IsFullOption(string s) => _fullOptionSetters.ContainsKey(s);

			bool IsShortOption(char c) => _shortOptionSetters.ContainsKey(c);

			bool IsStackedOption(string s)
			{
				var chars = s.ToCharArray();

				return chars.Length == chars.Distinct().Count()
					&& chars.All(_shortOptionSetters.ContainsKey);
			}
		}

		private void InitializeSetterMaps(List<IOption> options)
		{
			foreach (IOption option in options)
			{
				(Action<TRunInfo, object>, Type) setter = createSetter(option);

				var (fullKey, shortKey) = OptionTokenizer.TokenizeKey(option.Key);

				_fullOptionSetters.Add(fullKey, setter);

				if (shortKey != null)
				{
					_shortOptionSetters.Add(shortKey.Value, setter);
				}
			}

			// local functions
			(Action<TRunInfo, object> setter, Type valueType) createSetter(IOption option)
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
		}
	}
}
