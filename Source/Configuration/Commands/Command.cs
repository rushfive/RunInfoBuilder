using R5.RunInfoBuilder.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder
{
	public class Command<TRunInfo> : CommandBase<TRunInfo>
		where TRunInfo : class
	{
		public string Key { get; set; }
		public List<Command<TRunInfo>> SubCommands { get; set; } = new List<Command<TRunInfo>>();

		internal void Validate(int commandLevel)
		{
			if (string.IsNullOrWhiteSpace(Key))
			{
				throw new CommandValidationException("Command key must be provided.",
					CommandValidationError.KeyNotProvided, commandLevel);
			}

			base.Validate(commandLevel, Key);

			if (SubCommands != null)
			{
				int nullIndex = SubCommands.IndexOfFirstNull();
				if (nullIndex != -1)
				{
					throw new CommandValidationException(
						$"Command '{Key}' contains a null subcommand (index {nullIndex}).",
						CommandValidationError.NullObject, commandLevel, nullIndex);
				}

				bool hasDuplicate = SubCommands.Count != SubCommands.Select(c => c.Key).Distinct().Count();
				if (hasDuplicate)
				{
					throw new CommandValidationException($"Command key '{Key}' is invalid because "
						+ "it clashes with an already configured key.",
						CommandValidationError.DuplicateKey, commandLevel);
				}

				SubCommands.ForEach(o => o.Validate(++commandLevel));
			}
		}
	}
}
