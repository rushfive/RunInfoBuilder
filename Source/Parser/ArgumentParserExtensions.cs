using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Parser
{
	internal static class ArgumentParserExtensions
	{
		internal static void AddSystemTypePredicates(this ArgumentParser parser)
		{
			// bool
			parser.SetPredicateForType<bool>(value =>
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
			});

			// byte
			parser.SetPredicateForType<byte>(value =>
			{
				if (byte.TryParse(value, out byte parsed))
				{
					return (true, parsed);
				}
				return (false, default);
			});

			// char
			parser.SetPredicateForType<char>(value =>
			{
				if (char.TryParse(value, out char parsed))
				{
					return (true, parsed);
				}
				return (false, default);
			});

			// datetime
			parser.SetPredicateForType<DateTime>(val =>
			{
				if (DateTime.TryParse(val, out DateTime parsed))
				{
					return (true, parsed);
				}
				return (false, default);
			});

			// decimal
			parser.SetPredicateForType<decimal>(val =>
			{
				if (decimal.TryParse(val, out decimal parsed))
				{
					return (true, parsed);
				}
				return (false, default);
			});

			// double
			parser.SetPredicateForType<double>(val =>
			{
				if (double.TryParse(val, out double parsed))
				{
					return (true, parsed);
				}
				return (false, default);
			});

			// int
			parser.SetPredicateForType<int>(val =>
			{
				if (int.TryParse(val, out int parsed))
				{
					return (true, parsed);
				}
				return (false, default);
			});
		}
	}
}
