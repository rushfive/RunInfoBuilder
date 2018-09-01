using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor;
using R5.RunInfoBuilder.Processor.Models;
using R5.RunInfoBuilder.Processor.Stages;
using R5.RunInfoBuilder.Validators;

namespace R5.RunInfoBuilder.Commands
{
	public class SequenceArgument<TRunInfo, TListProperty> : ArgumentBase<TRunInfo>
			where TRunInfo : class
	{
		public Expression<Func<TRunInfo, List<TListProperty>>> ListProperty { get; set; }
		
		internal override void Validate()
		{
			if (ListProperty == null)
			{
				throw new InvalidOperationException("Sequence argument must provide the mapping expression.");
			}

			if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(ListProperty, out string propertyName))
			{
				throw new InvalidOperationException($"Property '{propertyName}' is not writable. Try adding a setter.");
			}
		}

		internal override Stage<TRunInfo> ToStage(IArgumentParser parser)
		{
			return new SequenceArgumentStage<TRunInfo, TListProperty>(parser, ListProperty);
		}
	}
}
