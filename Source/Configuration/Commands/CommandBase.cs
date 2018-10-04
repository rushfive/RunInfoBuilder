using R5.RunInfoBuilder.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder
{
	public abstract class CommandBase<TRunInfo>
		where TRunInfo : class
	{
		public string Description { get; set; }
		public string HelpText { get; set; }
		public List<ArgumentBase<TRunInfo>> Arguments { get; set; } = new List<ArgumentBase<TRunInfo>>();
		public List<OptionBase<TRunInfo>> Options { get; set; } = new List<OptionBase<TRunInfo>>();

		protected void Validate(int commandLevel, string key)
		{
			if (Arguments != null)
			{
				int nullIndex = Arguments.IndexOfFirstNull();
				if (nullIndex != -1)
				{
					throw new CommandValidationException(
						$"Command '{key}' contains a null argument (index {nullIndex}).",
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
							$"Command '{key}' contains a null option (index {nullIndex}).",
							CommandValidationError.NullObject, commandLevel, nullIndex);
					}
				}

				bool matchesRegex = Options
					.Select(o => o.Key)
					.All(OptionTokenizer.IsValidConfiguration);

				if (!matchesRegex)
				{
					throw new CommandValidationException($"Command '{key}' contains an option with an invalid key.",
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
					throw new CommandValidationException($"Command '{key}' contains options with duplicate full keys.",
						CommandValidationError.DuplicateKey, commandLevel);
				}

				bool duplicateShort = shortKeys.Count != shortKeys.Distinct().Count();
				if (duplicateShort)
				{
					throw new CommandValidationException($"Command '{key}' contains options with duplicate short keys.",
						CommandValidationError.DuplicateKey, commandLevel);
				}

				Options.ForEach(o => o.Validate(commandLevel));
			}
		}
	}
}
