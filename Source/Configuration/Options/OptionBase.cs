using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Processor.Functions;
using R5.RunInfoBuilder.Processor.Models;
using System;

namespace R5.RunInfoBuilder
{
	public abstract class OptionBase<TRunInfo> : CoreConfigurable
		where TRunInfo : class
	{
		public string Key { get; set; } // potentially move this to Option and grab from processinfo
		internal Type Type { get; }
		internal object OnProcess { get; } // type = Func<TProperty, ProcessStageResult>

		protected OptionBase(Type type)
		{
			Type = type;
		}

		internal abstract OptionProcessInfo<TRunInfo> GetProcessInfo();

		internal abstract string GetHelpToken();
	}
}
