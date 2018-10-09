using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using R5.RunInfoBuilder.Processor.Models;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class SetArgumentStage<TRunInfo, TProperty> : Stage<TRunInfo>
		where TRunInfo : class
	{
		private Expression<Func<TRunInfo, TProperty>> _property { get; }
		private Dictionary<string, TProperty> _valueMap { get; }

		internal SetArgumentStage(
			Expression<Func<TRunInfo, TProperty>> property,
			List<(string, TProperty)> values)
		{
			_property = property;
			_valueMap = values.ToDictionary(v => v.Item1, v => v.Item2);
		}

		internal override ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context, Action<CommandBase<TRunInfo>> resetContextFunc)
		{
			if (!context.ProgramArguments.HasMore())
			{
				throw new ProcessException("Expected an argument but reached the end of program args.",
					ProcessError.ExpectedProgramArgument, context.CommandLevel);
			}

			string valueToken = context.ProgramArguments.Dequeue();

			if (!_valueMap.ContainsKey(valueToken))
			{
				throw new ProcessException($"'{valueToken}' is invalid for this set.",
					ProcessError.UnknownValue, context.CommandLevel);
			}

			PropertyInfo propertyInfo = ReflectionHelper<TRunInfo>.GetPropertyInfoFromExpression(_property);
			propertyInfo.SetValue(context.RunInfo, _valueMap[valueToken]);

			return ProcessResult.Continue;
		}
	}
}
