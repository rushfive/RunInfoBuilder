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

		internal ArgumentSequenceStage(
			Expression<Func<TRunInfo, List<TListProperty>>> listProperty,
			IArgumentParser parser,
			ProcessContext<TRunInfo> context)
			: base(context)
		{
			_listProperty = listProperty;
			_parser = parser;
		}

		internal ProcessStageResult ProcessStage(CallbackContext<TRunInfo> context)
		{
			//ProcessStageResult result = InvokeCallback(context);
			//if (result != ProcessResult.Continue)
			//{
			//	return result;
			//}

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

				if (!_parser.TryParseAs(next, out TListProperty parsed))
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
