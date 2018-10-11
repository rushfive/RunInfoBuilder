using R5.RunInfoBuilder.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Configuration
{
	internal static class ValidationRules
	{
		// Arguments
		internal static class Arguments
		{
			internal static class Property
			{
				internal static List<Action<int>> Rules<TRunInfo, TProperty>(PropertyArgument<TRunInfo, TProperty> argument)
					where TRunInfo : class
				{
					return new List<Action<int>>
					{
						PropertyMappingIsSet(argument),
						MappedPropertyIsWritable(argument)
					};
				}

				private static Action<int> PropertyMappingIsSet<TRunInfo, TProperty>(PropertyArgument<TRunInfo, TProperty> argument)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						if (argument.Property == null)
						{
							throw new CommandValidationException("Property Argument is missing its property mapping expression.",
								CommandValidationError.NullPropertyExpression, commandLevel);
						}
					};
				}

				private static Action<int> MappedPropertyIsWritable<TRunInfo, TProperty>(PropertyArgument<TRunInfo, TProperty> argument)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(argument.Property, out string propertyName))
						{
							throw new CommandValidationException($"Property Argument's property '{propertyName}' "
								+ "is not writable. Try adding a setter.",
								CommandValidationError.PropertyNotWritable, commandLevel);
						}
					};
				}
			}

			internal static class Custom
			{
				internal static List<Action<int>> Rules<TRunInfo>(CustomArgument<TRunInfo> argument)
					where TRunInfo : class
				{
					return new List<Action<int>>
					{
						CountMustBeGreaterThanZero(argument),
						HandlerMustBeSet(argument),
						HelpTokenMustBeSet(argument)
					};
				}

				private static Action<int> CountMustBeGreaterThanZero<TRunInfo>(CustomArgument<TRunInfo> argument)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						if (argument.Count <= 0)
						{
							throw new CommandValidationException("Custom Argument has an invalid count. Must be greater than 0.",
								CommandValidationError.InvalidCount, commandLevel);
						}
					};
				}

				private static Action<int> HandlerMustBeSet<TRunInfo>(CustomArgument<TRunInfo> argument)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						if (argument.Handler == null)
						{
							throw new CommandValidationException("Custom Argument is missing its handler callback.",
								CommandValidationError.NullCustomHandler, commandLevel);
						}
					};
				}

				private static Action<int> HelpTokenMustBeSet<TRunInfo>(CustomArgument<TRunInfo> argument)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						if (string.IsNullOrWhiteSpace(argument.HelpToken))
						{
							throw new CommandValidationException("Custom Arguments must explicitly set their own help token string.",
								CommandValidationError.NullHelpToken, commandLevel);
						}
					};
				}
			}

			internal static class Sequence
			{
				internal static List<Action<int>> Rules<TRunInfo, TListProperty>(SequenceArgument<TRunInfo, TListProperty> argument)
					where TRunInfo : class
				{
					return new List<Action<int>>
					{
						MappedPropertyMustBeSet(argument),
						MappedPropertyIsWritable(argument)
					};
				}

				private static Action<int> MappedPropertyMustBeSet<TRunInfo, TListProperty>(SequenceArgument<TRunInfo, TListProperty> argument)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						if (argument.ListProperty == null)
						{
							throw new CommandValidationException("Sequence Argument is missing its property mapping expression.",
								CommandValidationError.NullPropertyExpression, commandLevel);
						}
					};
				}

				private static Action<int> MappedPropertyIsWritable<TRunInfo, TListProperty>(SequenceArgument<TRunInfo, TListProperty> argument)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(argument.ListProperty, out string propertyName))
						{
							throw new CommandValidationException($"Sequence Argument's property '{propertyName}' "
								+ "is not writable. Try adding a setter.",
								CommandValidationError.PropertyNotWritable, commandLevel);
						}
					};
				}
			}

			internal static class Set
			{
				internal static List<Action<int>> Rules<TRunInfo, TProperty>(SetArgument<TRunInfo, TProperty> argument)
					where TRunInfo : class
				{
					return new List<Action<int>>
					{
						MappedPropertyMustBeSet(argument),
						MappedPropertyIsWritable(argument),
						ValuesMustBeSet(argument),
						ValuesMustContainAtLeastTwoItems(argument),
						ValueLabelsMustBeUnique(argument),
						ValueValuesMustBeUnique(argument)
					};
				}

				private static Action<int> MappedPropertyMustBeSet<TRunInfo, TProperty>(SetArgument<TRunInfo, TProperty> argument)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						if (argument.Property == null)
						{
							throw new CommandValidationException("Set Argument is missing its property mapping expression.",
								CommandValidationError.NullPropertyExpression, commandLevel);
						}
					};
				}

				private static Action<int> MappedPropertyIsWritable<TRunInfo, TProperty>(SetArgument<TRunInfo, TProperty> argument)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(argument.Property, out string propertyName))
						{
							throw new CommandValidationException($"Set Argument's property '{propertyName}' "
								+ "is not writable. Try adding a setter.",
								CommandValidationError.PropertyNotWritable, commandLevel);
						}
					};
				}

				private static Action<int> ValuesMustBeSet<TRunInfo, TProperty>(SetArgument<TRunInfo, TProperty> argument)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						if (argument.Values == null)
						{
							throw new CommandValidationException("List of values for the set must be provided.",
								CommandValidationError.NullObject, commandLevel);
						}
					};
				}

				private static Action<int> ValuesMustContainAtLeastTwoItems<TRunInfo, TProperty>(SetArgument<TRunInfo, TProperty> argument)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						if (argument.Values.Count <= 1)
						{
							throw new CommandValidationException("Set Arguments must contain at least two items.",
								CommandValidationError.InsufficientCount, commandLevel);
						}
					};
				}

				private static Action<int> ValueLabelsMustBeUnique<TRunInfo, TProperty>(SetArgument<TRunInfo, TProperty> argument)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						if (argument.Values.Select(v => v.Label).Distinct().Count() != argument.Values.Count)
						{
							throw new CommandValidationException("Set Argument value labels must be unique within a set.",
								CommandValidationError.DuplicateKey, commandLevel);
						}
					};
				}

				private static Action<int> ValueValuesMustBeUnique<TRunInfo, TProperty>(SetArgument<TRunInfo, TProperty> argument)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						if (argument.Values.Select(v => v.Value).Distinct().Count() != argument.Values.Count)
						{
							throw new CommandValidationException("Set Argument values must be unique within a set.",
								CommandValidationError.DuplicateKey, commandLevel);
						}
					};
				}
			}
		}
		
		internal static class Commands
		{
			internal static class Command
			{
				internal static List<Action<int>> Rules<TRunInfo>(Command<TRunInfo> command)
					where TRunInfo : class
				{
					var rules = new List<Action<int>>
					{
						KeyIsNotNullOrEmpty(command)
					};

					rules.AddRange(Base.Rules(command));

					if (command.SubCommands != null)
					{
						rules.Add(SubCommandsCannotBeNull(command));
						rules.Add(SubCommandKeysMustBeUnique(command));
						rules.Add(SubCommandsAreValid(command));
					}

					return rules;
				}

				private static Action<int> KeyIsNotNullOrEmpty<TRunInfo>(Command<TRunInfo> command)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						if (string.IsNullOrWhiteSpace(command.Key))
						{
							throw new CommandValidationException("Command key must be provided.",
								CommandValidationError.KeyNotProvided, commandLevel);
						}
					};
				}

				private static Action<int> SubCommandsCannotBeNull<TRunInfo>(Command<TRunInfo> command)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						int nullIndex = command.SubCommands.IndexOfFirstNull();
						if (nullIndex != -1)
						{
							throw new CommandValidationException(
								$"Command contains a null subcommand (index {nullIndex}).",
								CommandValidationError.NullObject, commandLevel, nullIndex);
						}
					};
				}

				private static Action<int> SubCommandKeysMustBeUnique<TRunInfo>(Command<TRunInfo> command)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						bool hasDuplicate = command.SubCommands.Count != command.SubCommands.Select(c => c.Key).Distinct().Count();
						if (hasDuplicate)
						{
							throw new CommandValidationException("Command key is invalid because "
								+ "it clashes with an already configured key.",
								CommandValidationError.DuplicateKey, commandLevel);
						}
					};
				}

				private static Action<int> SubCommandsAreValid<TRunInfo>(Command<TRunInfo> command)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						command.SubCommands.ForEach(o => o.Validate(++commandLevel));
					};
				}
			}
			

			internal static class Default
			{
				internal static List<Action<int>> Rules<TRunInfo>(DefaultCommand<TRunInfo> command)
					where TRunInfo : class
				{
					return Base.Rules(command);
				}
			}

			internal static class Base
			{
				internal static List<Action<int>> Rules<TRunInfo>(CommandBase<TRunInfo> command)
					where TRunInfo : class
				{
					var rules = new List<Action<int>>();

					if (command.Arguments != null)
					{
						rules.Add(ArgumentsCannotBeNull(command));
						rules.Add(ArgumentsAreValid(command));
					}

					if (command.Options != null)
					{
						rules.Add(OptionsCannotBeNull(command));
						rules.Add(KeysMustMatchRegex(command));
						rules.Add(KeysMustBeUnique(command));
						rules.Add(OptionsAreValid(command));
					}

					return rules;
				}

				private static Action<int> ArgumentsCannotBeNull<TRunInfo>(CommandBase<TRunInfo> command)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						int nullIndex = command.Arguments.IndexOfFirstNull();
						if (nullIndex != -1)
						{
							throw new CommandValidationException(
								$"Command contains a null argument (index {nullIndex}).",
								CommandValidationError.NullObject, commandLevel, nullIndex);
						}
					};
				}

				private static Action<int> ArgumentsAreValid<TRunInfo>(CommandBase<TRunInfo> command)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						command.Arguments.ForEach(a => a.Validate(commandLevel));
					};
				}

				private static Action<int> OptionsCannotBeNull<TRunInfo>(CommandBase<TRunInfo> command)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						int nullIndex = command.Options.IndexOfFirstNull();
						if (nullIndex != -1)
						{
							throw new CommandValidationException(
								$"Command contains a null option (index {nullIndex}).",
								CommandValidationError.NullObject, commandLevel, nullIndex);
						}
					};
				}

				private static Action<int> KeysMustMatchRegex<TRunInfo>(CommandBase<TRunInfo> command)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						bool matchesRegex = command.Options
							.Select(o => o.Key)
							.All(OptionTokenizer.IsValidConfiguration);

						if (!matchesRegex)
						{
							throw new CommandValidationException("Command contains an option with an invalid key.",
								CommandValidationError.InvalidKey, commandLevel);
						}
					};
				}

				private static Action<int> KeysMustBeUnique<TRunInfo>(CommandBase<TRunInfo> command)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						var fullKeys = new List<string>();
						var shortKeys = new List<char>();

						command.Options.ForEach(o =>
						{
							var (fullKey, shortKey) = OptionTokenizer.TokenizeKeyConfiguration(o.Key);

							fullKeys.Add(fullKey);

							if (shortKey.HasValue)
							{
								shortKeys.Add(shortKey.Value);
							}
						});

						bool duplicateFull = fullKeys.Count != fullKeys.Distinct().Count();
						if (duplicateFull)
						{
							throw new CommandValidationException("Command contains options with duplicate full keys.",
								CommandValidationError.DuplicateKey, commandLevel);
						}

						bool duplicateShort = shortKeys.Count != shortKeys.Distinct().Count();
						if (duplicateShort)
						{
							throw new CommandValidationException("Command contains options with duplicate short keys.",
								CommandValidationError.DuplicateKey, commandLevel);
						}
					};
				}

				private static Action<int> OptionsAreValid<TRunInfo>(CommandBase<TRunInfo> command)
					where TRunInfo : class
				{
					return commandLevel =>
					{
						command.Options.ForEach(o => o.Validate(commandLevel));
					};
				}
			}
		}

		internal static class Options
		{
			internal static List<Action<int>> Rules<TRunInfo, TProperty>(Option<TRunInfo, TProperty> option)
				where TRunInfo : class
			{
				return new List<Action<int>>
				{
					PropertyMappingIsSet(option),
					MappedPropertyIsWritable(option),
					OnProcessCallbackNotAllowedForBoolOptions(option)
				};
			}

			private static Action<int> PropertyMappingIsSet<TRunInfo, TProperty>(Option<TRunInfo, TProperty> option)
				where TRunInfo : class
			{
				return commandLevel =>
				{
					if (option.Property == null)
					{
						throw new CommandValidationException($"Option '{option.Key}' is missing its property mapping expression.",
							CommandValidationError.NullPropertyExpression, commandLevel);
					}
				};
			}

			private static Action<int> MappedPropertyIsWritable<TRunInfo, TProperty>(Option<TRunInfo, TProperty> option)
				where TRunInfo : class
			{
				return commandLevel =>
				{
					if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(option.Property, out string propertyName))
					{
						throw new CommandValidationException($"Option '{option.Key}'s property '{propertyName}' "
							+ "is not writable. Try adding a setter.",
							CommandValidationError.PropertyNotWritable, commandLevel);
					}
				};
			}

			private static Action<int> OnProcessCallbackNotAllowedForBoolOptions<TRunInfo, TProperty>(Option<TRunInfo, TProperty> option)
				where TRunInfo : class
			{
				return commandLevel =>
				{
					if (option.OnProcess != null && typeof(TProperty) == typeof(bool))
					{
						throw new CommandValidationException(
							"OnProcess callbacks aren't allowed on bool options.",
							CommandValidationError.CallbackNotAllowed, commandLevel);
					}
				};
			}
		}
	}
}
