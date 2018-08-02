using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Store;
using R5.RunInfoBuilder.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Help
{
	internal interface IHelpManager<TRunInfo>
		where TRunInfo : class
	{
		bool IsTrigger(string argumentToken);

		void InvokeCallback();
	}

	internal class HelpManager<TRunInfo> : IHelpManager<TRunInfo>
		where TRunInfo : class
	{
		private const string keyPadding = "    ";
		private const string descriptionPadding = "      ";
		private const string validationPadding = "        ";

		private IArgumentMetadata<TRunInfo> _argumentMaps { get; }
		private IRestrictedKeyValidator _keyValidator { get; }
		private IHelpBuilder<TRunInfo> _helpBuilder { get; }

		private HashSet<string> _helpTriggers { get; set; }
		private string _programDescription { get; set; }
		private Action<HelpCallbackContext<TRunInfo>> _callback { get; set; }

		public HelpManager(
			IArgumentMetadata<TRunInfo> argumentMaps,
			IRestrictedKeyValidator keyValidator,
			IHelpBuilder<TRunInfo> helpBuilder,
			HelpConfig<TRunInfo> config)
		{
			_argumentMaps = argumentMaps;
			_keyValidator = keyValidator;
			_helpBuilder = helpBuilder;

			Configure(config);
		}

		private void Configure(HelpConfig<TRunInfo> config)
		{
			_programDescription = config.ProgramDescription;
			_callback = config.Callback;

			bool hasRestrictedTrigger = config.Triggers.Any(t => _keyValidator.IsRestrictedKey(t));
			if (hasRestrictedTrigger)
			{
				throw new InvalidOperationException("At least one of the triggers is restricted.");
			}

			var comparer = config.IgnoreCase
				? StringComparer.OrdinalIgnoreCase
				: StringComparer.Ordinal;

			_helpTriggers = new HashSet<string>(config.Triggers, comparer);

			_keyValidator.AddRestrictedKeys(config.Triggers);
			// todo: formatter?
		}

		public bool IsTrigger(string triggerToken)
		{
			if (string.IsNullOrWhiteSpace(triggerToken))
			{
				throw new ArgumentNullException(nameof(triggerToken), "Trigger token must be provided.");
			}
			return _helpTriggers.Contains(triggerToken);
		}

		public string GetFormatted()
		{
			//string formatted = _helpBuilder.



			var sb = new StringBuilder();

			if (!string.IsNullOrWhiteSpace(_programDescription))
			{
				sb
					.AppendLine(_programDescription)
					.AppendLine();
			}

			HelpMetadata metadata = GetMetadata();

			if (metadata.Commands.Any())
			{
				sb.AppendLine("Commands:");
				foreach (CommandHelpInfo command in metadata.Commands)
				{
					sb.AppendLine($"{keyPadding}{command.Key}");

					if (!string.IsNullOrWhiteSpace(command.Description))
					{
						string formattedDescription = command.Description.Replace(Environment.NewLine, Environment.NewLine + descriptionPadding);
						sb.AppendLine(descriptionPadding + formattedDescription);
					}
				}
			}

			if (metadata.Arguments.Any())
			{
				sb.AppendLine("Arguments:");
				foreach (ArgumentHelpInfo argument in metadata.Arguments)
				{
					sb.AppendLine($"{keyPadding}{argument.Key}");

					if (!string.IsNullOrWhiteSpace(argument.Description))
					{
						string formattedDescription = argument.Description.Replace(Environment.NewLine, Environment.NewLine + descriptionPadding);
						sb.AppendLine(descriptionPadding + formattedDescription);
					}

					if (!string.IsNullOrWhiteSpace(argument.ValidatorDescription))
					{
						sb.Append(validationPadding + "Validation:" + Environment.NewLine);
						string formattedValidation = argument.ValidatorDescription.Replace(Environment.NewLine, Environment.NewLine + validationPadding);
						sb.AppendLine(validationPadding + formattedValidation);
					}
				}
			}

			if (metadata.Options.Any())
			{
				sb.AppendLine("Options:");
				foreach (OptionHelpInfo option in metadata.Options)
				{
					string optionLabel = "";
					if (option.ShortKey.HasValue)
					{
						optionLabel += $"-{option.ShortKey} ";
					}
					optionLabel += $"--{option.FullKey}";

					sb.AppendLine($"{keyPadding}{optionLabel}");

					if (!string.IsNullOrWhiteSpace(option.Description))
					{
						string formattedDescription = option.Description.Replace(Environment.NewLine, Environment.NewLine + descriptionPadding);
						sb.AppendLine(descriptionPadding + formattedDescription);
					}
				}
			}

			return sb.ToString();
		}

		public HelpMetadata GetMetadata()
		{
			List<ArgumentHelpInfo> arguments = _argumentMaps
				.GetArguments()
				.Select(a => new ArgumentHelpInfo(a.Key, a.Description, a.PropertyInfo, a.ValidatorDescription))
				.ToList();

			List<CommandHelpInfo> commands = _argumentMaps
				.GetCommands()
				.Select(c => new CommandHelpInfo(c.Key, c.Description))
				.ToList();

			List<OptionHelpInfo> options = _argumentMaps
				.GetOptions()
				.Select(o => new OptionHelpInfo(o.FullKey, o.ShortKey, o.Description, o.PropertyInfo))
				.ToList();


			return new HelpMetadata(arguments, commands, options, _helpTriggers.ToList(), _programDescription);
		}

		public void InvokeCallback()
		{
			if (_callback == null)
			{
				throw new InvalidOperationException("Trigger callback has not been set.");
			}

			var callbackContext = new HelpCallbackContext<TRunInfo>(this.GetFormatted(), this.GetMetadata());
			_callback(callbackContext);
		}
	}
}
