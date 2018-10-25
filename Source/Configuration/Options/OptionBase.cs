using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Processor.Functions;
using R5.RunInfoBuilder.Processor.Models;
using System;

namespace R5.RunInfoBuilder
{
	public abstract class OptionBase<TRunInfo> : CoreConfigurable
		where TRunInfo : class
	{
		public string Key { get; set; }
		internal Type Type { get; }

		protected OptionBase(Type type)
		{
			Type = type;
		}

		internal abstract OptionProcessInfo<TRunInfo> GetProcessInfo();

		internal abstract string GetHelpToken();
	}
}
