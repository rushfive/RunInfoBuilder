using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder
{
	internal class RunInfoBuilderException : Exception
	{
		public RunInfoBuilderException(string message)
			: base(message)
		{

		}
	}
	
	internal class ProgramArgumentsValidationException : RunInfoBuilderException
	{
		public List<ProgramArgumentError> Errors { get; }

		internal ProgramArgumentsValidationException(List<ProgramArgumentError> errors, string message)
			: base(message)
		{
			this.Errors = errors;
		}
	}

	internal class BuilderConfigurationValidationException : RunInfoBuilderException
	{
		internal BuilderConfigurationValidationException(string message)
			: base(message)
		{
		}
	}
}
