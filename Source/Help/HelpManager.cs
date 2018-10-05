﻿using R5.RunInfoBuilder.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder
{
	public class HelpManager
	{
		private static readonly string[] _defaultTriggers = new string[] 
		{
			"--help", "-h", "-?", "/help", "/h", "/?"
		};

		public bool InvokeOnFail { get; private set; }

		private List<string> _triggers { get; }
		private Action _customCallback { get; set; }
		private List<string> _commandInfo { get; }
		private string _defaultCommandInfo { get; set; }

		internal HelpManager()
		{
			InvokeOnFail = false;
			_triggers = new List<string>(_defaultTriggers);
			_customCallback = null;
			_commandInfo = new List<string>();
			_defaultCommandInfo = null;
		}

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

		public HelpManager DisplayHelpOnBuildFail()
		{
			InvokeOnFail = true;
			return this;
		}

		public HelpManager OnTrigger(Action customCallback)
		{
			if (customCallback == null)
			{
				throw new ArgumentNullException(nameof(customCallback), "A valid custom help callback must be provided.");
			}

			_customCallback = customCallback;
			return this;
		}

		internal void InvokeHelp()
		{
			if (_customCallback != null)
			{
				_customCallback();
				return;
			}

			// default help logic..
		}

		internal void ConfigureForCommand<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class
		{
			string helpText = HelpBuilder.BuildFor(command);

			// TEST
			Console.WriteLine(helpText);
		}


		internal void ConfigureForDefaultCommand<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand)
			where TRunInfo : class
		{
			string helpText = HelpBuilder.BuildFor(defaultCommand);

			_defaultCommandInfo = helpText;

			// TEMP
			Console.WriteLine(helpText);
		}
	}
}
