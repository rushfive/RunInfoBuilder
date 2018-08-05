using OLD.ArgumentParser;
using System;
using System.Collections.Generic;

namespace OLD.Configuration
{
    public class ParserConfigBuilder
    {
		private AutoParseEnum _autoParseEnum { get; set; }
		private List<(Type, Func<string, (bool, object)>)> _typePredicates { get; }

		internal ParserConfigBuilder()
		{
			_autoParseEnum = AutoParseEnum.Never;
			_typePredicates = new List<(Type, Func<string, (bool, object)>)>();
		}

		public ParserConfigBuilder AutoParseEnumCaseSensitive()
		{
			_autoParseEnum = AutoParseEnum.ParseCaseSensitive;
			return this;
		}

		public ParserConfigBuilder AutoParseEnumIgnoreCase()
		{
			_autoParseEnum = AutoParseEnum.ParseCaseInsensitive;
			return this;
		}

		public ParserConfigBuilder HandleBuiltInPrimitives()
		{
			// bool
			_typePredicates.Add((typeof(bool), value =>
			{
				var trueHash = new HashSet<string>(
					new string[] { "yes", "y", "1" },
					StringComparer.OrdinalIgnoreCase);

				var falseHash = new HashSet<string>(
					new string[] { "no", "n", "0" },
					StringComparer.OrdinalIgnoreCase);

				if (trueHash.Contains(value))
				{
					return (true, true);
				}
				if (falseHash.Contains(value))
				{
					return (true, false);
				}
				if (bool.TryParse(value, out bool parsed))
				{
					return (true, parsed);
				}

				return (false, default);
			}));

			// byte
			_typePredicates.Add((typeof(byte), value =>
			{
				if (byte.TryParse(value, out byte parsed))
				{
					return (true, parsed);
				}
				return (false, default);
			}));

			// char
			_typePredicates.Add((typeof(char), value =>
			{
				if (char.TryParse(value, out char parsed))
				{
					return (true, parsed);
				}
				return (false, default);
			}));

			// datetime
			_typePredicates.Add((typeof(DateTime), val =>
			{
				if (DateTime.TryParse(val, out DateTime parsed))
				{
					return (true, parsed);
				}
				return (false, default);
			}));

			// decimal
			_typePredicates.Add((typeof(decimal), val =>
			{
				if (decimal.TryParse(val, out decimal parsed))
				{
					return (true, parsed);
				}
				return (false, default);
			}));

			// double
			_typePredicates.Add((typeof(double), val =>
			{
				if (double.TryParse(val, out double parsed))
				{
					return (true, parsed);
				}
				return (false, default);
			}));

			// int
			_typePredicates.Add((typeof(int), val =>
			{
				if (int.TryParse(val, out int parsed))
				{
					return (true, parsed);
				}
				return (false, default);
			}));

			return this;
		}

		internal ParserConfig Build()
		{
			return new ParserConfig(_autoParseEnum, _typePredicates);
		}
	}
}
