using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder
{
    public class ConfigurationException : Exception
	{
		public Type Type { get; }
		public Type ParentType { get; }
		public string ParentKey { get; }

		public ConfigurationException(string message, Type type,
			Type parentType, string parentKey)
			: base (message)
		{
			Type = type;
			ParentType = parentType;
			ParentKey = parentKey;
		}

		public ConfigurationException(string message, Exception innerException, Type type,
			Type parentType, string parentKey)
			: base(message, innerException)
		{
			Type = type;
			ParentType = parentType;
			ParentKey = parentKey;
		}
	}
}
