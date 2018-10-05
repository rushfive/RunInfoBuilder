using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Help
{
	internal static class HelpTokenResolver
	{
		internal static string ForPropertyArgument<TPropertyType>()
		{
			var typeString = GetTypeString<TPropertyType>();
			return $"<{typeString}>";
		}

		internal static string ForSequenceArgument<TPropertyType>()
		{
			var typeString = GetTypeString<TPropertyType>();
			return $"<...{typeString}>";
		}

		// todo: maybe use a dictionary of mappings, allowing the
		// user to self-configure their own
		private static string GetTypeString<T>()
		{
			Type type = typeof(T);

			if (type == typeof(string))
			{
				return "string";
			}
			if (type == typeof(int))
			{
				return "int";
			}
			if (type == typeof(bool))
			{
				return "bool";
			}
			if (type == typeof(double))
			{
				return "double";
			}
			if (type == typeof(decimal))
			{
				return "decimal";
			}
			if (type == typeof(char))
			{
				return "char";
			}
			if (type == typeof(byte))
			{
				return "byte";
			}
			if (type == typeof(DateTime))
			{
				return "date";
			}

			throw new CommandValidationException(
				$"Failed to resolve help token for type '{type.Name}'. "
				+ "Explicitly set it during command configuration.",
				CommandValidationError.InvalidType, -1);
		}
	}
}
