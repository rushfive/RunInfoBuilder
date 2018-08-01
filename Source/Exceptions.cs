using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder
{
	public class RunInfoBuilderException : Exception
	{
		public RunInfoBuilderException(string message)
			: base(message)
		{

		}
	}

	public class ProgramArgumentsValidationException : RunInfoBuilderException
	{
		public List<ProgramArgumentError> Errors { get; }

		public ProgramArgumentsValidationException(List<ProgramArgumentError> errors, string message)
			: base(message)
		{
			this.Errors = errors;
		}
	}

	public class BuilderConfigurationValidationException : RunInfoBuilderException
	{
		public BuilderConfigurationValidationException(string message)
			: base(message)
		{
		}
	}
}
