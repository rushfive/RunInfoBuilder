using System;

namespace R5.RunInfoBuilder
{
	public abstract class OptionBase<TRunInfo> where TRunInfo : class
	{
		public string Key { get; set; }
		internal Type Type { get; }

		protected OptionBase(Type type)
		{
			Type = type;
		}

		internal abstract void Validate(int commandLevel);

		internal abstract string GetHelpToken();
	}
}
