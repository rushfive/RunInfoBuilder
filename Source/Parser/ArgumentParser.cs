using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace R5.RunInfoBuilder.Parser
{
	/// <summary>
	/// Provides methods to configure how program arguments are parsed 
	/// into various Types. Can handle system types out of the box.
	/// </summary>
	public class ArgumentParser
	{
		private bool _enumParseIgnoreCase { get; set; }

		// Value is of type Func<string, (bool, T)>. Storing as an object
		// to both allow users to generically add predicates AND retrieve 
		// and parse generically
		private Dictionary<Type, object> _predicatesMap { get; }

		internal ArgumentParser()
		{
			_predicatesMap = new Dictionary<Type, object>();

			this.AddSystemTypePredicates();
		}

		/// <summary>
		/// Call to set the auto parsing of enum Types to ignore case sensitivity.
		/// </summary>
		/// <returns>The Parser instance.</returns>
		public ArgumentParser EnumParsingIgnoresCase()
		{
			_enumParseIgnoreCase = true;
			return this;
		}

		/// <summary>
		/// Configures a custom predicate function that parses a string into T.
		/// </summary>
		/// <typeparam name="T">The Type being configured to be parseable.</typeparam>
		/// <param name="predicateFunc">
		/// Custom func used to parse the program argument string.
		/// The func should return a value tuple with the first item being a bool representing
		/// a successful parse. The second item is the parsed T value.
		/// </param>
		/// <returns>The Parser instance.</returns>
		public ArgumentParser SetPredicateForType<T>(Func<string, (bool isValid, T parsed)> predicateFunc)
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

		/// <summary>
		/// Parses a string into an object of the specified Type.
		/// </summary>
		/// <param name="type">Type of the object that the value should be parsed into.</param>
		/// <param name="value">The string token attempting to be parsed.</param>
		/// <param name="parsed">The parsed object as an out parameter.</param>
		/// <returns>A bool representing whether the value was successfully parsed.</returns>
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

			Type nullableType = Nullable.GetUnderlyingType(type);

			if (nullableType != null && nullableType.IsEnum)
			{
				if (value == "")
				{
					parsed = null;
					return true;
				}

				try
				{
					parsed = Enum.Parse(nullableType, value, ignoreCase: _enumParseIgnoreCase);
					return true;
				}
				catch (Exception)
				{
					// ignore exception. Enum.TryParse with ignoreCase not available in netstandard2.0
					return false;
				}
			}
			else if (type.IsEnum)
			{
				try
				{
					parsed = Enum.Parse(type, value, ignoreCase: _enumParseIgnoreCase);
					return true;
				}
				catch (Exception)
				{
					// ignore exception. Enum.TryParse with ignoreCase not available in netstandard2.0
					return false;
				}
			}

			if (!_predicatesMap.ContainsKey(type))
			{
				throw new InvalidOperationException($"Predicate for type '{type.Name}' is not set.");
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

		/// <summary>
		/// Parses a string into an object of the specified Type.
		/// </summary>
		/// <typeparam name="T">Type of the object that the value should be parsed into.</typeparam>
		/// <param name="value">The string token attempting to be parsed.</param>
		/// <param name="parsed">The parsed object as an out parameter.</param>
		/// <returns>A bool representing whether the value was successfully parsed.</returns>
		public bool TryParseAs<T>(string value, out T parsed)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value), "Value must be provided.");
			}

			parsed = default;

			Type type = typeof(T);
			Type nullableType = Nullable.GetUnderlyingType(type);

			if (nullableType != null && nullableType.IsEnum)
			{
				if (value == "")
				{
					parsed = default;
					return true;
				}

				try
				{
					parsed = (T)Enum.Parse(nullableType, value, ignoreCase: _enumParseIgnoreCase);
					return true;
				}
				catch (Exception)
				{
					// ignore exception. Enum.TryParse with ignoreCase not available in netstandard2.0
					return false;
				}
			}
			else if (type.IsEnum)
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

		/// <summary>
		/// Indicates whether the parser is currently configured to handle the Type.
		/// </summary>
		/// <param name="type">The type being determined to be parseable.</param>
		/// <returns>A bool indicating whether the parser currently handles the Type.</returns>
		public bool HandlesType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type), "Type argument must be provided.");
			}

			return _predicatesMap.ContainsKey(type);
		}

		/// <summary>
		/// Indicates whether the parser is currently configured to handle the Type.
		/// </summary>
		/// <typeparam name="T">The type being determined to be parseable.</typeparam>
		/// <returns>A bool indicating whether the parser currently handles the Type.</returns>
		public bool HandlesType<T>()
		{
			return _predicatesMap.ContainsKey(typeof(T));
		}
	}
}
