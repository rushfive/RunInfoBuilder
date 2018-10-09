using R5.RunInfoBuilder.Configuration;
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

		internal abstract string GetHelpToken();
	}
}
