using R5.RunInfoBuilder.Processor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace R5.RunInfoBuilder.Configuration
{
	internal static class ValidationRules
	{
		// Arguments
		internal static class Arguments
		{
			//internal static class Property
			//{
			//	internal static List<Action<int>> Rules<TRunInfo, TProperty>(PropertyArgument<TRunInfo, TProperty> argument)
			//		where TRunInfo : class
			//	{
			//		return new List<Action<int>>
			//		{
			//			PropertyMappingIsSet(argument),
			//			MappedPropertyIsWritable(argument)
			//		};
			//	}
			//}

			//internal static class Custom
			//{
			//	internal static List<Action<int>> Rules<TRunInfo>(CustomArgument<TRunInfo> argument)
			//		where TRunInfo : class
			//	{
			//		return new List<Action<int>>
			//		{
			//			CountMustBeGreaterThanZero(argument),
			//			HandlerMustBeSet(argument)
			//		};
			//	}
			//}

			//internal static class Sequence
			//{
			//	internal static List<Action<int>> Rules<TRunInfo, TListProperty>(SequenceArgument<TRunInfo, TListProperty> argument)
			//		where TRunInfo : class
			//	{
			//		return new List<Action<int>>
			//		{
			//			MappedPropertyMustBeSet(argument),
			//			MappedPropertyIsWritable(argument)
			//		};
			//	}
			//}

			//internal static class Set
			//{
			//	internal static List<Action<int>> Rules<TRunInfo, TProperty>(SetArgument<TRunInfo, TProperty> argument)
			//		where TRunInfo : class
			//	{
			//		return new List<Action<int>>
			//		{
			//			MappedPropertyMustBeSet(argument),
			//			MappedPropertyIsWritable(argument),
			//			ValuesMustBeSet(argument),
			//			ValuesMustContainAtLeastTwoItems(argument),
			//			ValueLabelsMustBeUnique(argument),
			//			ValueValuesMustBeUnique(argument)
			//		};
			//	}	
			//}
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

					if (command.GlobalOptions != null)
					{
						rules.Add(Options.OptionsCannotBeNull(command.GlobalOptions));
						rules.Add(Options.KeysMustMatchRegex(command.GlobalOptions));
						rules.Add(Options.KeysMustBeUnique(command.GlobalOptions, null));
						rules.Add(Options.OptionsAreValid(command.GlobalOptions));
					}

					rules.AddRange(Base.Rules(command, command.GlobalOptions));

					if (command.SubCommands != null)
					{
						rules.Add(SubCommandsCannotBeNull(command));
						rules.Add(SubCommandKeysMustBeUnique(command));
						rules.Add(SubCommandsAreValid(command, command.GlobalOptions));
					}

					return rules;
				}

				
			}
			

			//internal static class Default
			//{
			//	internal static List<Action<int>> Rules<TRunInfo>(DefaultCommand<TRunInfo> command)
			//		where TRunInfo : class
			//	{
			//		return Base.Rules(command, globalOptions: null);
			//	}
			//}

			internal static class SubCommand
			{
				// TODO: compare to default/commands and ensure all checks
				internal static List<Action<int>> Rules<TRunInfo>(SubCommand<TRunInfo> command,
					List<OptionBase<TRunInfo>> globalOptions)
					where TRunInfo : class
				{
					var rules = new List<Action<int>>
					{
						KeyIsNotNullOrEmpty(command)
					};

					rules.AddRange(Base.Rules(command, globalOptions));

					return rules;
				}
			}

			internal static class Base
			{
				internal static List<Action<int>> Rules<TRunInfo>(CommandBase<TRunInfo> command,
					List<OptionBase<TRunInfo>> globalOptions)
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
						rules.Add(Options.OptionsCannotBeNull(command.Options));
						rules.Add(Options.KeysMustMatchRegex(command.Options));
						rules.Add(Options.KeysMustBeUnique(command.Options, globalOptions));
						rules.Add(Options.OptionsAreValid(command.Options));
					}

					return rules;
				}
				
			}
		}

		//internal static class Options
		//{
		//	internal static List<Action<int>> Rules<TRunInfo, TProperty>(Option<TRunInfo, TProperty> option)
		//		where TRunInfo : class
		//	{
		//		return new List<Action<int>>
		//		{
		//			PropertyMappingIsSet(option),
		//			MappedPropertyIsWritable(option),
		//			OnProcessCallbackNotAllowedForBoolOptions(option)
		//		};
		//	}

			

		//	// shared rules between Options and GlobalOptions
			
		//}
	}
}
