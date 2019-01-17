using R5.RunInfoBuilder.Processor;

namespace R5.RunInfoBuilder.Configuration.Validators.Rules
{
	internal static class OptionRules
	{
		internal static void PropertyMappingIsSet<TRunInfo, TProperty>(Option<TRunInfo, TProperty> option,
			int commandLevel) where TRunInfo : class
		{
			if (option.Property == null)
			{
				throw new CommandValidationException($"Option '{option.Key}' is missing its property mapping expression.",
					CommandValidationError.NullPropertyExpression, commandLevel);
			}
		}

		internal static void MappedPropertyIsWritable<TRunInfo, TProperty>(Option<TRunInfo, TProperty> option,
			int commandLevel) where TRunInfo : class
		{
			if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(option.Property, out string propertyName))
			{
				throw new CommandValidationException($"Option '{option.Key}'s property '{propertyName}' "
					+ "is not writable. Try adding a setter.",
					CommandValidationError.PropertyNotWritable, commandLevel);
			}
		}

		internal static void OnProcessCallbackNotAllowedForBoolOptions<TRunInfo, TProperty>(Option<TRunInfo, TProperty> option,
			int commandLevel) where TRunInfo : class
		{
			if (option.OnParsed != null && typeof(TProperty) == typeof(bool))
			{
				throw new CommandValidationException(
					"OnProcess callbacks aren't allowed on bool options.",
					CommandValidationError.CallbackNotAllowed, commandLevel);
			}
		}
	}
}
