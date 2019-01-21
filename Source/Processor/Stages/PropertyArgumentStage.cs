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
		private Func<TProperty, ProcessStageResult> _onParsed { get; }
		private Func<string, string> _onParseErrorUseMessage { get; }

		internal PropertyArgumentStage(
			Expression<Func<TRunInfo, TProperty>> property,
			Func<TProperty, ProcessStageResult> onParsed,
			Func<string, string> onParseErrorUseMessage)
		{
			_property = property;
			_onParsed = onParsed;
			_onParseErrorUseMessage = onParseErrorUseMessage;
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
				string message = _onParseErrorUseMessage != null
					? _onParseErrorUseMessage(valueToken)
					: $"Failed to process program argument '{valueToken}' because it "
							+ $"couldn't be parsed into a '{typeof(TProperty).Name}'.";

				throw new ProcessException(message, ProcessError.ParserInvalidValue, context.CommandLevel);
			}

			ProcessStageResult result = _onParsed?.Invoke(parsed);
			if (result == ProcessResult.End)
			{
				return ProcessResult.End;
			}

			PropertyInfo propertyInfo = ReflectionHelper<TRunInfo>.GetPropertyInfoFromExpression(_property);
			propertyInfo.SetValue(context.RunInfo, parsed);

			return ProcessResult.Continue;
		}
	}
}
