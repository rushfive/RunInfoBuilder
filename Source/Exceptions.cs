using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder
{
	// todo standard overloads
	public class CommandValidationException : Exception
	{
		public CommandValidationError ErrorType { get; }
		public int CommandLevel { get; }
		public object[] Metadata { get; }

		public CommandValidationException(
			string message,
			CommandValidationError errorType,
			int commandLevel,
			params object[] metadata)
			: base(message)
		{
			ErrorType = errorType;
			CommandLevel = commandLevel;
			Metadata = metadata;
		}
	}

	public enum CommandValidationError
	{
		RestrictedKey,
		KeyNotProvided,
		NullObject
	}
}
