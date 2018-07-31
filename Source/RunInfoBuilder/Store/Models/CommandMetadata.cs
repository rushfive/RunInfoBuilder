using R5.RunInfoBuilder.Pipeline;
using R5.RunInfoBuilder.Process;
using System;
using System.Reflection;

namespace R5.RunInfoBuilder.Store
{
	internal enum CommandType
	{
		PropertyMapped,
		CustomCallback,
		MappedAndCallback
	}

	internal class CommandMetadata<TRunInfo>
		where TRunInfo : class
	{
		internal string Key { get; }
		internal CommandType Type { get; }
		internal string Description { get; }
		internal PropertyInfo PropertyInfo { get; }
		internal object MappedValue { get; }
		internal Func<ProcessContext<TRunInfo>, ProcessStageResult> Callback { get; }

		internal CommandMetadata(
			string key,
			CommandType type,
			string description,
			PropertyInfo propertyInfo,
			object mappedValue,
			Func<ProcessContext<TRunInfo>, ProcessStageResult> callback)
		{
			Key = key;
			Type = type;
			Description = description;
			PropertyInfo = propertyInfo;
			MappedValue = mappedValue;
			Callback = callback;
		}
	}
}
