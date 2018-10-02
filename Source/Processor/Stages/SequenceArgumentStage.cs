using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class SequenceArgumentStage<TRunInfo, TListProperty> : Stage<TRunInfo>
		where TRunInfo : class
	{
		private Expression<Func<TRunInfo, List<TListProperty>>> _listProperty { get; }
		
		internal SequenceArgumentStage(
			Expression<Func<TRunInfo, List<TListProperty>>> listProperty)
		{
			_listProperty = listProperty;
		}

		internal override ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context,
			Action<CommandBase<TRunInfo>> resetContextFunc)
		{
			if (!context.ProgramArguments.HasMore())
			{
				throw new ProcessException("Expected a sequence of arguments but reached the end of program args.",
					ProcessError.ExpectedProgramArgument, context.CommandLevel);
			}

			PropertyInfo propertyInfo = ReflectionHelper<TRunInfo>.GetPropertyInfoFromExpression(_listProperty);

			// initialize list if null
			var list = (IList<TListProperty>)propertyInfo.GetValue(context.RunInfo, null);
			if (list == null)
			{
				list = (IList<TListProperty>)Activator.CreateInstance(propertyInfo.PropertyType);
				propertyInfo.SetValue(context.RunInfo, list);
			}

			// Iterate over proceeding program args, adding parseable items to list.
			// Ends when all program args are exhausted or when an option or subcommand 
			// has been identified.
			while (context.ProgramArguments.HasMore())
			{
				if (context.ProgramArguments.NextIsOption())
				{
					return ProcessResult.Continue;
				}

				if (context.ProgramArguments.NextIsSubCommand())
				{
					return ProcessResult.Continue;
				}

				string next = context.ProgramArguments.Dequeue();

				if (!context.Parser.TryParseAs(next, out TListProperty parsed))
				{
					throw new ProcessException($"Failed to parse '{next}' as type '{typeof(TListProperty).Name}'.",
						ProcessError.ParserInvalidValue, context.CommandLevel);
				}

				list.Add(parsed);
			}

			return ProcessResult.End;
		}
	}
}
