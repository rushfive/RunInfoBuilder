using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Command.Models
{
	public class CommandMapped<TRunInfo, TProperty> : CommandBase<TRunInfo>
		where TRunInfo : class
	{
		public PropertyMapping<TRunInfo, TProperty> Mapping { get; set; } = new PropertyMapping<TRunInfo, TProperty>();

		internal override void Validate(Type parentType, string parentKey)
		{
			if (Mapping.Property == null)
			{
				throw new ConfigurationException("Property mapping must be provided.",
					typeof(ExclusiveArgumentSet<TRunInfo>), parentType, parentKey);
			}

			ValidateBase(typeof(CommandMapped<TRunInfo, TProperty>), parentType, parentKey);
		}
	}
}
