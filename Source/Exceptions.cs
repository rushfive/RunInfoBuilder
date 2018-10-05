using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder
{
	public class ProcessException : Exception
	{
		public ProcessError ErrorType { get; }
		public int CommandLevel { get; }
		public object[] Metadata { get; }

		public ProcessException(
			string message,
			ProcessError errorType,
			int commandLevel,
			Exception innerException = null,
			params object[] metadata)
			: base(message)
		{
			ErrorType = errorType;
			CommandLevel = commandLevel;
			Metadata = metadata;
		}
	}

	public enum ProcessError
	{
		GeneralFailure,
		ExpectedProgramArgument,
		ParserUnhandledType,
		ParserInvalidValue,
		//OptionExpected,
		InvalidStackedOption,
		ExpectedValueFoundOption,
		ExpectedValueFoundSubCommand,
		//OptionValueRequired,
		InvalidSubCommand,
		InvalidProgramArgument,
		InvalidStageResult
	}


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
		NullObject,
		DuplicateKey,
		InvalidKey,
		NullPropertyExpression,
		NullCustomHandler,
		InvalidCount,
		PropertyNotWritable,
		InvalidType,
		NullHelpToken
	}
}
