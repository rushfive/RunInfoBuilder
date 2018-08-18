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
		
		internal ArgumentSequenceStage(
			Expression<Func<TRunInfo, List<TListProperty>>> listProperty,
			ProcessContext<TRunInfo> context)
			: base(context)
		{
			_listProperty = listProperty;
		}

		internal override ProcessStageResult ProcessStage()
		{
			PropertyInfo propertyInfo = ReflectionHelper<TRunInfo>.GetPropertyInfoFromExpression(_listProperty);

			// initialize list if null
			var list = (IList<TListProperty>)propertyInfo.GetValue(_context.RunInfo, null);
			if (list == null)
			{
				propertyInfo.SetValue(_context.RunInfo, Activator.CreateInstance(propertyInfo.PropertyType));
			}

			// Iterate over proceeding program args, adding parseable items to list.
			// Ends when all program args are exhausted or when an option or subcommand 
			// has been identified.
			while (MoreProgramArgumentsExist())
			{
				if (NextIsOption())
				{
					return ProcessResult.Continue;
				}

				if (NextIsSubCommand())
				{
					return ProcessResult.Continue;
				}

				string next = Dequeue();

				if (!Parser.TryParseAs(next, out TListProperty parsed))
				{
					throw new ArgumentException($"Failed to parse '{next}' as type '{typeof(TListProperty).Name}'.");
				}

				list.Add(parsed);
			}

			return ProcessResult.End;
		}

		//private ProcessStageResult InvokeCallback(CallbackContext<TRunInfo> context)
		//{
		//	if (_callback == null)
		//	{
		//		return ProcessResult.Continue;
		//	}

		//	return _callback(context);
		//}
	}
}
