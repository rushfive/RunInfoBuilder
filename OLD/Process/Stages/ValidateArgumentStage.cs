using OLD.ArgumentParser;
using OLD.Store;
using System;

namespace OLD.Process
{
	internal class ValidateArgumentStage<TRunInfo> : ValidationStage<TRunInfo>
		where TRunInfo : class
	{
		private IArgumentMetadata<TRunInfo> _argumentMetadata { get; }
		private IParser _parser { get; }

		internal ValidateArgumentStage(
			IArgumentMetadata<TRunInfo> argumentMetadata,
			IParser parser)
			: base(handlesType: ProgramArgumentType.Argument)
		{
			_argumentMetadata = argumentMetadata;
			_parser = parser;
		}

		protected override void Validate(ProgramArgument argument, ValidationContext validationContext)
		{
			ThrowIfParserDoesntHandleType(argument.ArgumentToken);
		}

		protected override void MarkSeen(string token, ValidationContext validationContext)
			=> validationContext.MarkArgumentSeen(token);

		private void ThrowIfParserDoesntHandleType(string argumentToken)
		{
			ArgumentMetadata metadata = _argumentMetadata.GetArgument(argumentToken);

			if (!_parser.HandlesType(metadata.PropertyInfo.PropertyType))
			{
				throw new InvalidOperationException($"Argument '{argumentToken}' cannot be processed because "
					+ $"the parser isn't configured to handle type '{metadata.PropertyInfo.PropertyType}'");
			}
		}
	}
}
