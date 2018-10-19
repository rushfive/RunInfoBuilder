using R5.RunInfoBuilder.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder
{
	/// <summary>
	/// Provides methods to configure how the help menu works.
	/// </summary>
	public class HelpManager
	{
		private static readonly string[] _defaultTriggers = new string[] 
		{
			"--help", "-h", "/help"
		};

		internal bool InvokeOnFail { get; private set; }

		private string _programName { get; set; }
		private List<string> _triggers { get; }
		private Action _customCallback { get; set; }
		private List<string> _commandInfos { get; }
		private string _defaultCommandInfo { get; set; }

		internal HelpManager()
		{
			InvokeOnFail = false;
			_triggers = new List<string>(_defaultTriggers);
			_customCallback = null;
			_commandInfos = new List<string>();
			_defaultCommandInfo = null;
		}

		/// <summary>
		/// Sets a value for your program's name for use in the help menu.
		/// </summary>
		/// <param name="name">The name value.</param>
		/// <returns>The HelpManager instance.</returns>
		public HelpManager SetProgramName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException(nameof(name), "Program name must be provided.");
			}

			_programName = name;
			return this;
		}

		/// <summary>
		/// Sets the list of keywords that will trigger the help menu as a command.
		/// </summary>
		/// <param name="triggers">List of triggers as params.</param>
		/// <returns>The HelpManager instance.</returns>
		public HelpManager SetTriggers(params string[] triggers)
		{
			if (triggers == null || !triggers.Any())
			{
				throw new ArgumentNullException(nameof(triggers), "Triggers must be provided.");
			}

			_triggers.Clear();
			_triggers.AddRange(triggers);

			return this;
		}

		/// <summary>
		/// Configures the builder to automatically display the help menu (or invoke callback) on fail.
		/// </summary>
		/// <returns>The HelpManager instance.</returns>
		public HelpManager DisplayOnBuildFail()
		{
			InvokeOnFail = true;
			return this;
		}

		/// <summary>
		/// Sets a custom callback that will be invoked when the help is triggered.
		/// Having this set prevents the default help menu from displaying.
		/// </summary>
		/// <param name="customCallback">The custom callback action.</param>
		/// <returns>The HelpManager instance.</returns>
		public HelpManager OnTrigger(Action customCallback)
		{
			if (customCallback == null)
			{
				throw new ArgumentNullException(nameof(customCallback), "A valid custom help callback must be provided.");
			}

			_customCallback = customCallback;
			return this;
		}

		internal void Invoke()
		{
			if (_customCallback != null)
			{
				_customCallback();
				return;
			}

			if (!string.IsNullOrWhiteSpace(_defaultCommandInfo))
			{
				Console.WriteLine(_defaultCommandInfo);
				Console.WriteLine();
			}

			_commandInfos.ForEach(Console.WriteLine);
		}

		internal bool IsTrigger(string token)
		{
			return _triggers.Contains(token);
		}

		internal void ConfigureForCommand<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class
		{
			string helpText = HelpBuilder.BuildFor(command, _programName);
			_commandInfos.Add(helpText);
		}


		internal void ConfigureForDefaultCommand<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand)
			where TRunInfo : class
		{
			string helpText = HelpBuilder.BuildFor(defaultCommand, _programName);
			_defaultCommandInfo = helpText;
		}
	}
}
