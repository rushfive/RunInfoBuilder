using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Processor.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class ArgumentPropertyMappedStage<TRunInfo, TProperty> : Stage<TRunInfo>
		where TRunInfo : class
	{
		private Expression<Func<TRunInfo, TProperty>> _property { get; }

		internal ArgumentPropertyMappedStage(
			Expression<Func<TRunInfo, TProperty>> property,
			ProcessContext<TRunInfo> context)
			: base(context)
		{
			_property = property;
		}

		internal override ProcessStageResult ProcessStage(CallbackContext<TRunInfo> context)
		{
			//ProcessStageResult result = InvokeCallback(context);
			//if (result != ProcessResult.Continue)
			//{
			//	return result;
			//}

			if (!context.Parser.HandlesType<TProperty>())
			{
				throw new InvalidOperationException($"Failed to process program argument '{context.Token}' because the "
					+ $"parser cannot handle the property type of '{typeof(TProperty).Name}'.");
			}

			if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(_property, out string propertyName))
			{
				throw new InvalidOperationException($"Failed to process program argument '{context.Token}' because the "
					+ $"property '{propertyName}' is not writable.");
			}

			if (!context.Parser.TryParseAs<TProperty>(context.Token, out TProperty parsed))
			{
				throw new InvalidOperationException($"Failed to process program argument '{context.Token}' because it "
					+ $"couldn't be parsed into a '{typeof(TProperty).Name}'.");
			}

			PropertyInfo propertyInfo = ReflectionHelper<TRunInfo>.GetPropertyInfoFromExpression(_property);
			propertyInfo.SetValue(context.RunInfo, parsed);

			return ProcessResult.Continue;
		}
	}
}
