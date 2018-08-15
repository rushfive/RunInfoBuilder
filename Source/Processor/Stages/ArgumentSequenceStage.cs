using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor.Models;
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
		private OptionsInfo<TRunInfo> _optionsInfo { get; }
		private List<string> _availableSubCommands { get; }

		internal ArgumentSequenceStage(
			Expression<Func<TRunInfo, List<TListProperty>>> listProperty,
			IArgumentParser parser,
			OptionsInfo<TRunInfo> optionsInfo,
			List<string> availableSubCommands,
			ArgumentsQueue argumentsQueue,
			Func<ProcessContext<TRunInfo>, ProcessStageResult> callback)
			: base(argumentsQueue, callback)
		{
			_listProperty = listProperty;
			_parser = parser;
			_optionsInfo = optionsInfo;
			_availableSubCommands = availableSubCommands;
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
			while (HasNext())
			{
				string nextProgramArgument = PeekNext();

				bool nextIsOption = _optionsInfo.IsOption(nextProgramArgument);
				if (nextIsOption)
				{
					return ProcessResult.Continue;
				}

				bool nextIsSubCommand = _availableSubCommands.Contains(nextProgramArgument);
				if (nextIsSubCommand)
				{
					return ProcessResult.Continue;
				}

				nextProgramArgument = DequeueNext();

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
