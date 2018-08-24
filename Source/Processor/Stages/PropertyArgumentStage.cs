using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class PropertyArgumentStage<TRunInfo, TProperty> : Stage<TRunInfo>
		where TRunInfo : class
	{
		private IArgumentParser _parser { get; }
		private Expression<Func<TRunInfo, TProperty>> _property { get; }

		internal PropertyArgumentStage(
			IArgumentParser parser,
			Expression<Func<TRunInfo, TProperty>> property)
		{
			_parser = parser;
			_property = property;
		}

		internal override ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context)
		{
			if (!context.ProgramArguments.HasMore())
			{
				throw new InvalidOperationException("Expected an argument but reached the end of program args.");
			}

			string valueToken = context.ProgramArguments.Dequeue();

			if (!_parser.HandlesType<TProperty>())
			{
				throw new InvalidOperationException($"Failed to process program argument '{valueToken}' because the "
					+ $"parser cannot handle the property type of '{typeof(TProperty).Name}'.");
			}

			if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(_property, out string propertyName))
			{
				throw new InvalidOperationException($"Failed to process program argument '{valueToken}' because the "
					+ $"property '{propertyName}' is not writable.");
			}

			if (!_parser.TryParseAs<TProperty>(valueToken, out TProperty parsed))
			{
				throw new InvalidOperationException($"Failed to process program argument '{valueToken}' because it "
					+ $"couldn't be parsed into a '{typeof(TProperty).Name}'.");
			}

			PropertyInfo propertyInfo = ReflectionHelper<TRunInfo>.GetPropertyInfoFromExpression(_property);
			propertyInfo.SetValue(context.RunInfo, parsed);

			return ProcessResult.Continue;
		}
	}
}
