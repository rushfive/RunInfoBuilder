using R5.RunInfoBuilder.Commands;
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

		internal void Validate(ValidationContext context)
		{
			if (string.IsNullOrWhiteSpace(Key))
			{
				throw new InvalidOperationException("Command key must be provided.");
			}

			if (!context.IsValidCommand(Key))
			{
				throw new InvalidOperationException($"Command key '{Key}' is invalid because "
					+ "it clashes with an already configured key.");
			}

			context.MarkCommandSeen(Key);
			
			if (Arguments != null)
			{
				if (Arguments.Any(a => a == null))
				{
					throw new InvalidOperationException($"Command '{Key}' contains a null argument.");
				}

				Arguments.ForEach(a => a.Validate(context));
			}

			if (Options != null)
			{
				if (Options.Any(o => o == null))
				{
					throw new InvalidOperationException($"Command '{Key}' contains a null option.");
				}

				Options.ForEach(o => o.Validate(context));
			}

			if (SubCommands != null)
			{
				if (SubCommands.Any(o => o == null))
				{
					throw new InvalidOperationException($"Command '{Key}' contains a null sub command.");
				}

				var subContext = new ValidationContext(Key);
				SubCommands.ForEach(o => o.Validate(subContext));
			}
		}
	}
}
