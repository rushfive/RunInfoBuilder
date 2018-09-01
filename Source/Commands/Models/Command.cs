using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Processor;
using R5.RunInfoBuilder.Validators;
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

		internal void Validate()
		{
			if (string.IsNullOrWhiteSpace(Key))
			{
				throw new InvalidOperationException("Command key must be provided.");
			}

			// validating key clashes against other same level commands should
			// be done by the caller
			
			// for now, dont care about specific error messaging.

			if (Arguments != null)
			{
				if (Arguments.Any(a => a == null))
				{
					throw new InvalidOperationException($"Command '{Key}' contains a null argument.");
				}

				Arguments.ForEach(a => a.Validate());
			}

			if (Options != null)
			{
				if (Options.Any(o => o == null))
				{
					throw new InvalidOperationException($"Command '{Key}' contains a null option.");
				}

				bool invalidFormat = Options
					.Select(o => o.Key)
					.Any(OptionTokenizer.IsValidConfiguration);

				if (invalidFormat)
				{
					throw new InvalidOperationException($"Command '{Key}' contains an option with an invalid key.");
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
					throw new InvalidCastException();
				}

				bool duplicateShort = shortKeys.Count != shortKeys.Distinct().Count();
				if (duplicateFull)
				{
					throw new InvalidCastException();
				}
				
				Options.ForEach(o => o.Validate());
			}

			if (SubCommands != null)
			{
				if (SubCommands.Any(o => o == null))
				{
					throw new InvalidOperationException($"Command '{Key}' contains a null sub command.");
				}

				bool hasDuplicate = SubCommands.Count != SubCommands.Distinct().Count();
				if (hasDuplicate)
				{
					throw new InvalidOperationException($"Command key '{Key}' is invalid because "
						+ "it clashes with an already configured key.");
				}

				SubCommands.ForEach(o => o.Validate());
			}
		}
	}
}
