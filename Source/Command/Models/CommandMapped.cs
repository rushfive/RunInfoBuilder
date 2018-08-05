using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Command.Models
{
	public class Command<TRunInfo, TProperty> : CommandBase<TRunInfo>
		where TRunInfo : class
	{
		public PropertyMapping<TRunInfo, TProperty> Mapping { get; set; } = new PropertyMapping<TRunInfo, TProperty>();
	}
}
