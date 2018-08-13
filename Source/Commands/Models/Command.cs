using R5.RunInfoBuilder.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public class Command<TRunInfo>
		where TRunInfo : class
	{
		public string Key { get; set; }
		public string Description { get; set; }
		public string HelpText { get; set; }
		public Func<ProcessContext<TRunInfo>, ProcessStageResult> Callback { get; set; }

		public List<Command<TRunInfo>> SubCommands { get; set; } = new List<Command<TRunInfo>>();
		public List<ArgumentBase<TRunInfo>> Arguments { get; set; } = new List<ArgumentBase<TRunInfo>>();
		public List<OptionBase<TRunInfo>> Options { get; set; } = new List<OptionBase<TRunInfo>>();

		internal void Validate(Type parentType, string parentKey)
		{
			var type = typeof(Command<TRunInfo>);

			if (string.IsNullOrWhiteSpace(Key))
			{
				throw new ConfigurationException("Key must be provided.",
					type, parentType, parentKey);
			}

			if (!Arguments.NullOrEmpty())
			{
				Arguments.ForEach(a => a.Validate(type, Key));
			}

			if (!Options.NullOrEmpty())
			{
				Options.ForEach(o => o.Validate(type, Key));
			}

			if (!SubCommands.NullOrEmpty())
			{
				SubCommands.ForEach(o => o.Validate(type, Key));
			}
		}
	}
}
