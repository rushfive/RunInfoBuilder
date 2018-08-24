﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using R5.RunInfoBuilder.Parser;
using R5.RunInfoBuilder.Processor.Models;
using R5.RunInfoBuilder.Processor.Stages;

namespace R5.RunInfoBuilder.Commands
{
	public class SequenceArgument<TRunInfo, TListProperty> : ArgumentBase<TRunInfo>
			where TRunInfo : class
	{
		public Expression<Func<TRunInfo, List<TListProperty>>> ListProperty { get; set; }
		
		internal override void Validate(Type parentType, string parentKey)
		{
			if (ListProperty == null)
			{
				throw new ConfigurationException("Property expression for list must be provided.",
					typeof(SequenceArgument<TRunInfo, TListProperty>), parentType, parentKey);
			}
		}

		internal override Stage<TRunInfo> ToStage(IArgumentParser parser)
		{
			return new SequenceArgumentStage<TRunInfo, TListProperty>(parser, ListProperty);
		}
	}
}