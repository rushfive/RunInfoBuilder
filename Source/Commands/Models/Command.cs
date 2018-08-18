using R5.RunInfoBuilder.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder
{
	public class Command<TRunInfo> : CommandBase<TRunInfo>
		where TRunInfo : class
	{
		public string Key { get; set; }
		public List<Command<TRunInfo>> SubCommands { get; set; } = new List<Command<TRunInfo>>();

		internal void Validate(Type parentType, string parentKey)
		{
			//var type = typeof(Command<TRunInfo>);

			//if (string.IsNullOrWhiteSpace(Key))
			//{
			//	throw new ConfigurationException("Key must be provided.",
			//		type, parentType, parentKey);
			//}

			//if (!Arguments.NullOrEmpty())
			//{
			//	Arguments.ForEach(a => a.Validate(type, Key));
			//}

			//if (!Options.NullOrEmpty())
			//{
			//	Options.ForEach(o => o.Validate(type, Key));
			//}

			//if (!SubCommands.NullOrEmpty())
			//{
			//	SubCommands.ForEach(o => o.Validate(type, Key));
			//}
		}
	}
}
