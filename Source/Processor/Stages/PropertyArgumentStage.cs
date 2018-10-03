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
		private Expression<Func<TRunInfo, TProperty>> _property { get; }

		internal PropertyArgumentStage(Expression<Func<TRunInfo, TProperty>> property)
		{
			_property = property;
		}

		internal override ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context,
			Action<CommandBase<TRunInfo>> resetContextFunc)
		{
			if (!context.ProgramArguments.HasMore())
			{
				throw new ProcessException("Expected an argument but reached the end of program args.",
					ProcessError.ExpectedProgramArgument, context.CommandLevel);
			}

			string valueToken = context.ProgramArguments.Dequeue();

			if (!context.Parser.HandlesType<TProperty>())
			{
				throw new ProcessException($"Failed to process program argument '{valueToken}' because the "
					+ $"parser cannot handle the property type of '{typeof(TProperty).Name}'.",
					ProcessError.ParserUnhandledType, context.CommandLevel);
			}
			
			if (!context.Parser.TryParseAs(valueToken, out TProperty parsed))
			{
				throw new ProcessException($"Failed to process program argument '{valueToken}' because it "
					+ $"couldn't be parsed into a '{typeof(TProperty).Name}'.",
					ProcessError.ParserInvalidValue, context.CommandLevel);
			}

			PropertyInfo propertyInfo = ReflectionHelper<TRunInfo>.GetPropertyInfoFromExpression(_property);
			propertyInfo.SetValue(context.RunInfo, parsed);

			return ProcessResult.Continue;
		}
	}
}
