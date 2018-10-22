using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Processor.Models
{
	internal class OptionProcessInfo<TRunInfo>
		where TRunInfo : class
	{
		internal Action<TRunInfo, object> Setter { get; }
		internal Type Type { get; }
		internal object OnParsed { get; } // type = Func<TProperty, ProcessStageResult>

		internal OptionProcessInfo(
			Action<TRunInfo, object> setter,
			Type type,
			object onParsed)
		{
			Setter = setter;
			Type = type;
			OnParsed = onParsed;
		}
	}
}
