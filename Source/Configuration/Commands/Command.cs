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

			if (Arguments != null)
			{
				int nullIndex = Arguments.IndexOfFirstNull();
				if (nullIndex != -1)
				{
					throw new CommandValidationException(
						$"Command '{Key}' contains a null argument (index {nullIndex}).",
						CommandValidationError.NullObject, commandLevel, nullIndex);
				}

				Arguments.ForEach(a => a.Validate(commandLevel));
			}

			if (Options != null)
			{
				if (Options.Any(o => o == null))
				{
					int nullIndex = Options.IndexOfFirstNull();
					if (nullIndex != -1)
					{
						throw new CommandValidationException(
							$"Command '{Key}' contains a null option (index {nullIndex}).",
							CommandValidationError.NullObject, commandLevel, nullIndex);
					}
				}

				bool matchesRegex = Options
					.Select(o => o.Key)
					.All(OptionTokenizer.IsValidConfiguration);

				if (!matchesRegex)
				{
					throw new CommandValidationException($"Command '{Key}' contains an option with an invalid key.",
						CommandValidationError.InvalidKey, commandLevel);
				}

				var fullKeys = new List<string>();
				var shortKeys = new List<char>();

				Options.ForEach(o =>
				{
					var (fullKey, shortKey) = OptionTokenizer.TokenizeKeyConfiguration(o.Key);

					fullKeys.Add(fullKey);

					if (shortKey.HasValue)
					{
						shortKeys.Add(shortKey.Value);
					}
				});

				bool duplicateFull = fullKeys.Count != fullKeys.Distinct().Count();
				if (duplicateFull)
				{
					throw new CommandValidationException($"Command '{Key}' contains options with duplicate full keys.",
						CommandValidationError.DuplicateKey, commandLevel);
				}

				bool duplicateShort = shortKeys.Count != shortKeys.Distinct().Count();
				if (duplicateShort)
				{
					throw new CommandValidationException($"Command '{Key}' contains options with duplicate short keys.",
						CommandValidationError.DuplicateKey, commandLevel);
				}

				Options.ForEach(o => o.Validate(commandLevel));
			}

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
