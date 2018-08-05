using OLD.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OLD.ArgumentParser
{
	public interface IParser
	{
		IParser SetPredicateForType<T>(Func<string, (bool isValid, T parsed)> predicateFunc);

		bool TryParseAs(Type type, string value, out object parsed);

		bool TryParseAs<T>(string value, out T parsed);

		bool HandlesType(Type type);

		bool HandlesType<T>();
	}

	internal class Parser : IParser
	{
		private bool _autoParseEnumTypes { get; set; }
		private bool _enumParseIgnoreCase { get; set; }

		// Value is of type Func<string, (bool, T)>. Storing as an object
		// to both allow users to generically add predicates AND retrieve 
		// and parse generically
		private Dictionary<Type, object> _predicatesMap { get; }

		public Parser(ParserConfig config)
		{
			_autoParseEnumTypes = false;
			_enumParseIgnoreCase = false;
			_predicatesMap = new Dictionary<Type, object>();

			Configure(config);
		}

		private void Configure(ParserConfig config)
		{
			if (config.AutoParseEnum == AutoParseEnum.ParseCaseSensitive)
			{
				_autoParseEnumTypes = true;
				_enumParseIgnoreCase = false;
			}

			if (config.AutoParseEnum == AutoParseEnum.ParseCaseInsensitive)
			{
				_autoParseEnumTypes = true;
				_enumParseIgnoreCase = true;
			}

			if (!config.ParseTypePredicates.IsNullOrEmpty())
			{
				foreach ((Type type, Func<string, (bool, object)> predicate) in config.ParseTypePredicates)
				{
					_predicatesMap.Add(type, predicate);
				}
			}
		}

		public IParser SetPredicateForType<T>(Func<string, (bool isValid, T parsed)> predicateFunc)
		{
			Type type = typeof(T);

			if (_predicatesMap.ContainsKey(type))
			{
				_predicatesMap[type] = predicateFunc;
			}
			else
			{
				_predicatesMap.Add(typeof(T), predicateFunc);
			}

			return this;
		}

		public bool TryParseAs(Type type, string value, out object parsed)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type), "Type must be provided.");
			}
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value), "Value must be provided.");
			}

			parsed = null;

			if (_autoParseEnumTypes && type.IsEnum)
			{
				try
				{
					parsed = Enum.Parse(type, value, ignoreCase: _enumParseIgnoreCase);
					return true;
				}
				catch (Exception)
				{
					// ignore exception info. Enum.TryParse with ignoreCase not available in netstandard2.0
					return false;
				}
			}

			if (!_predicatesMap.ContainsKey(type))
			{
				throw new ArgumentException($"Predicate for type '{type.Name}' is not set.", nameof(type));
			}

			dynamic predicate = _predicatesMap[type];

			var result = predicate.Invoke(value);

			bool valid = result.Item1;
			var parsedResult = result.Item2;

			if (valid)
			{
				parsed = parsedResult;
				return true;
			}

			return false;
		}

		public bool TryParseAs<T>(string value, out T parsed)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value), "Value must be provided.");
			}

			parsed = default;

			Type type = typeof(T);

			if (_autoParseEnumTypes && type.IsEnum)
			{
				try
				{
					parsed = (T)Enum.Parse(type, value, ignoreCase: _enumParseIgnoreCase);
					return true;
				}
				catch (Exception)
				{
					// ignore exception info. Enum.TryParse with ignoreCase not available in netstandard2.0
					return false;
				}
			}

			if (!_predicatesMap.ContainsKey(type))
			{
				throw new ArgumentException($"Predicate for type '{type.Name}' is not set.", nameof(type));
			}

			var predicate = _predicatesMap[type] as Func<string, (bool, T)>;

			Debug.Assert(predicate != null, "Predicate should always be of the correct type.");

			(bool valid, T result) = predicate(value);

			if (valid)
			{
				parsed = result;
				return true;
			}

			return false;
		}

		public bool HandlesType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type), "Type argument must be provided.");
			}

			return _predicatesMap.ContainsKey(type);
		}

		public bool HandlesType<T>()
		{
			return _predicatesMap.ContainsKey(typeof(T));
		}
	}
}
