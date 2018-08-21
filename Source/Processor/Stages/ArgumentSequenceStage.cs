﻿using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class ArgumentSequenceStage<TRunInfo, TListProperty> : Stage<TRunInfo>
		where TRunInfo : class
	{
		private IArgumentParser _parser { get; }
		private Expression<Func<TRunInfo, List<TListProperty>>> _listProperty { get; }
		
		internal ArgumentSequenceStage(
			IArgumentParser parser,
			Expression<Func<TRunInfo, List<TListProperty>>> listProperty)
		{
			_parser = parser;
			_listProperty = listProperty;
		}

		internal override ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context)
		{
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
			while (context.ProgramArguments.HasMore())
			{
				if (context.NextIsOption())
				{
					return ProcessResult.Continue;
				}

				if (context.NextIsSubCommand())
				{
					return ProcessResult.Continue;
				}

				string next = context.ProgramArguments.Dequeue();

				if (!_parser.TryParseAs(next, out TListProperty parsed))
				{
					throw new ArgumentException($"Failed to parse '{next}' as type '{typeof(TListProperty).Name}'.");
				}

				list.Add(parsed);
			}

			return ProcessResult.End;
		}
	}
}
