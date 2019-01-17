using R5.RunInfoBuilder.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Configuration.Validators.Rules
{
	internal static class ArgumentRules
	{
		internal static class Property
		{
			internal static void PropertyMappingIsSet<TRunInfo, TProperty>(PropertyArgument<TRunInfo, TProperty> argument,
				int commandLevel) where TRunInfo : class
			{
				if (argument.Property == null)
				{
					throw new CommandValidationException("Property Argument is missing its property mapping expression.",
						CommandValidationError.NullPropertyExpression, commandLevel);
				}
			}

			internal static void MappedPropertyIsWritable<TRunInfo, TProperty>(PropertyArgument<TRunInfo, TProperty> argument,
					int commandLevel) where TRunInfo : class
			{
				if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(argument.Property, out string propertyName))
				{
					throw new CommandValidationException($"Property Argument's property '{propertyName}' "
						+ "is not writable. Try adding a setter.",
						CommandValidationError.PropertyNotWritable, commandLevel);
				}
			}
		}

		internal static class Custom
		{
			internal static void CountMustBeGreaterThanZero<TRunInfo>(CustomArgument<TRunInfo> argument,
				int commandLevel) where TRunInfo : class
			{
				if (argument.Count <= 0)
				{
					throw new CommandValidationException("Custom Argument has an invalid count. Must be greater than 0.",
						CommandValidationError.InvalidCount, commandLevel);
				}
			}

			internal static void HandlerMustBeSet<TRunInfo>(CustomArgument<TRunInfo> argument,
				int commandLevel) where TRunInfo : class
			{
				if (argument.Handler == null)
				{
					throw new CommandValidationException("Custom Argument is missing its handler callback.",
						CommandValidationError.NullCustomHandler, commandLevel);
				}
			}
		}

		internal static class Sequence
		{
			internal static void MappedPropertyMustBeSet<TRunInfo, TListProperty>(SequenceArgument<TRunInfo, TListProperty> argument,
				int commandLevel) where TRunInfo : class
			{
				if (argument.ListProperty == null)
				{
					throw new CommandValidationException("Sequence Argument is missing its property mapping expression.",
						CommandValidationError.NullPropertyExpression, commandLevel);
				}
			}

			internal static void MappedPropertyIsWritable<TRunInfo, TListProperty>(SequenceArgument<TRunInfo, TListProperty> argument,
				int commandLevel) where TRunInfo : class
			{
				if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(argument.ListProperty, out string propertyName))
				{
					throw new CommandValidationException($"Sequence Argument's property '{propertyName}' "
						+ "is not writable. Try adding a setter.",
						CommandValidationError.PropertyNotWritable, commandLevel);
				}
			}
		}

		internal static class Set
		{
			internal static void MappedPropertyMustBeSet<TRunInfo, TProperty>(SetArgument<TRunInfo, TProperty> argument,
				int commandLevel)where TRunInfo : class
			{
				if (argument.Property == null)
				{
					throw new CommandValidationException("Set Argument is missing its property mapping expression.",
						CommandValidationError.NullPropertyExpression, commandLevel);
				}
			}

			internal static void MappedPropertyIsWritable<TRunInfo, TProperty>(SetArgument<TRunInfo, TProperty> argument,
				int commandLevel)where TRunInfo : class
			{
				if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(argument.Property, out string propertyName))
				{
					throw new CommandValidationException($"Set Argument's property '{propertyName}' "
						+ "is not writable. Try adding a setter.",
						CommandValidationError.PropertyNotWritable, commandLevel);
				}
			}

			internal static void ValuesMustBeSet<TRunInfo, TProperty>(SetArgument<TRunInfo, TProperty> argument,
				int commandLevel)where TRunInfo : class
			{
				if (argument.Values == null)
				{
					throw new CommandValidationException("List of values for the set must be provided.",
						CommandValidationError.NullObject, commandLevel);
				}
			}

			internal static void ValuesMustContainAtLeastTwoItems<TRunInfo, TProperty>(SetArgument<TRunInfo, TProperty> argument,
				int commandLevel)where TRunInfo : class
			{
				if (argument.Values.Count <= 1)
				{
					throw new CommandValidationException("Set Arguments must contain at least two items.",
						CommandValidationError.InsufficientCount, commandLevel);
				}
			}

			internal static void ValueLabelsMustBeUnique<TRunInfo, TProperty>(SetArgument<TRunInfo, TProperty> argument,
				int commandLevel)where TRunInfo : class
			{
				if (argument.Values.Select(v => v.Label).Distinct().Count() != argument.Values.Count)
				{
					throw new CommandValidationException("Set Argument value labels must be unique within a set.",
						CommandValidationError.DuplicateKey, commandLevel);
				}
			}

			internal static void ValueValuesMustBeUnique<TRunInfo, TProperty>(SetArgument<TRunInfo, TProperty> argument,
				int commandLevel)where TRunInfo : class
			{
				if (argument.Values.Select(v => v.Value).Distinct().Count() != argument.Values.Count)
				{
					throw new CommandValidationException("Set Argument values must be unique within a set.",
						CommandValidationError.DuplicateKey, commandLevel);
				}
			}
		}
	}
}
