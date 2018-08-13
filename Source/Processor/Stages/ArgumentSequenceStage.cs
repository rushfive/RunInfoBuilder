using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class ArgumentSequenceStage<TRunInfo, TListProperty> : Stage<TRunInfo>
		where TRunInfo : class
	{
		private Expression<Func<TRunInfo, List<TListProperty>>> _listProperty { get; }
		private IArgumentParser _parser { get; }
		private (List<string>, List<char>)? _availableOptions { get; }
		private List<string> _availableSubCommands { get; }

		internal ArgumentSequenceStage(
			Expression<Func<TRunInfo, List<TListProperty>>> listProperty,
			IArgumentParser parser,
			List<string> availableOptions,
			List<string> availableSubCommands,
			ArgumentsQueue argumentsQueue,
			Func<ProcessContext<TRunInfo>, ProcessStageResult> callback)
			: base(argumentsQueue, callback)
		{
			_listProperty = listProperty;
			_parser = parser;
			_availableOptions = TokenizeAvailableOptions(availableOptions);
			_availableSubCommands = availableSubCommands;
		}

		private (List<string>, List<char>)? TokenizeAvailableOptions(List<string> availableOptions)
		{
			if (!availableOptions.Any())
			{
				return null;
			}

			var fullKeys = new List<string>();
			var shortKeys = new List<char>();

			foreach (string option in availableOptions)
			{
				if (!OptionHelper.TryTokenize(option, out (string, char?)? result))
				{
					throw new ArgumentException($"Failed to tokenize option '{option}'.", nameof(availableOptions));
				}

				var (fullKey, shortKey) = result.Value;

				fullKeys.Add(fullKey);

				if (shortKey.HasValue)
				{
					shortKeys.Add(shortKey.Value);
				}
			}

			return (fullKeys, shortKeys);
		}

		protected override ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context)
		{
			ProcessStageResult result = InvokeCallback(context);
			if (result != ProcessResult.Continue)
			{
				return result;
			}

			PropertyInfo propertyInfo = ReflectionHelper<TRunInfo>.GetPropertyInfoFromExpression(_listProperty);

			// initialize list if null
			var list = (IList<TListProperty>)propertyInfo.GetValue(context.RunInfo, null);
			if (list == null)
			{
				propertyInfo.SetValue(context.RunInfo, Activator.CreateInstance(propertyInfo.PropertyType));
			}

			// Iterate over proceeding program args, adding parseable items to list.
			// Ends when all program args are exhausted or when an option or subcommand 
			// has been identified.
			while (HasNext)
			{
				string nextProgramArgument = Peek;

				bool nextIsOption = _availableOptions.HasValue && OptionHelper.IsValidOption(nextProgramArgument, _availableOptions.Value);
				if (nextIsOption)
				{
					return ProcessResult.Continue;
				}

				bool nextIsSubCommand = _availableSubCommands.Contains(nextProgramArgument);
				if (nextIsSubCommand)
				{
					return ProcessResult.Continue;
				}

				nextProgramArgument = Next;

				if (!_parser.TryParseAs(nextProgramArgument, out TListProperty parsed))
				{
					throw new ArgumentException($"Failed to parse '{nextProgramArgument}' as type '{typeof(TListProperty).Name}'.");
				}

				list.Add(parsed);
			}

			return ProcessResult.End;
		}
	}
}
