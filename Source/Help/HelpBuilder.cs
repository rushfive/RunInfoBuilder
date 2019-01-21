﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Help
{
	internal static class HelpBuilder
	{
		private const string Padding = "  ";
		private static string PaddingRepeated(int repeatCount) => new StringBuilder().Insert(0, Padding, repeatCount).ToString();

		internal static string BuildFor<TRunInfo>(Command<TRunInfo> command, string programName)
			where TRunInfo : class
		{
			var sb = new StringBuilder();

			sb.Append(command.Key);

			if (!string.IsNullOrWhiteSpace(command.Description))
			{
				sb.AppendLine(" - " + command.Description);
			}
			else
			{
				sb.AppendLine();
			}

			if (command.SubCommands.Any())
			{
				sb.AppendLine(Padding + "Usage: ");
			}
			else
			{
				sb.Append(Padding + "Usage: ");
			}
			

			AppendCommandInfo(sb, command.Key, isRoot: true, command, commandDepth: 0, programName);

			return sb.ToString();
		}

		private static void AppendCommandInfo<TRunInfo>(StringBuilder sb,
			string rootCommandKey, bool isRoot, StackableCommand<TRunInfo> command, int commandDepth,
			string programName)
			where TRunInfo : class
		{
			var helpTokens = new List<string>();
			
			if (!string.IsNullOrWhiteSpace(programName))
			{
				helpTokens.Add(programName);
			}
			helpTokens.Add(rootCommandKey);

			helpTokens.AddRange(GetRepeatedList("...", commandDepth));

			if (!isRoot)
			{
				helpTokens.Add(command.Key);
			}

			IEnumerable<string> argumentTokens = command.Arguments.Select(a => a.GetHelpToken());
			IEnumerable<string> optionTokens = command.Options.Select(o => o.GetHelpToken());

			helpTokens.AddRange(argumentTokens);
			helpTokens.AddRange(optionTokens);

			if (command.SubCommands.Any())
			{
				var keys = string.Join("|", command.SubCommands.Select(c => c.Key));
				helpTokens.Add($"({keys})");
				helpTokens.Add("...");
			}

			string result = string.Join(" ", helpTokens.Select(t => t.Trim()));

			if (command.SubCommands.Any())
			{
				sb.AppendLine(PaddingRepeated(2) + result);
			}
			else if (!command.SubCommands.Any() && isRoot)
			{
				sb.AppendLine(result);
			}
			else
			{
				sb.AppendLine(PaddingRepeated(2) + result);
			}

			// recursively add subcommands with more padding
			foreach(SubCommand<TRunInfo> subCommand in command.SubCommands)
			{
				AppendCommandInfo(sb, rootCommandKey, isRoot: false, subCommand, commandDepth + 1, programName);
			}
		}

		private static List<string> GetRepeatedList(string token, int count)
		{
			var result = new List<string>();
			while (count-- > 0)
			{
				result.Add(token);
			}
			return result;
		}

		internal static string BuildFor<TRunInfo>(DefaultCommand<TRunInfo> command, string programName)
			where TRunInfo : class
		{
			var sb = new StringBuilder();

			sb.AppendLine("Default Command");

			if (!string.IsNullOrWhiteSpace(command.Description))
			{
				sb.AppendLine(Padding + $"{command.Description}");
			}

			sb.Append(Padding + "Usage: ");

			var helpTokens = new List<string>();

			if (!string.IsNullOrWhiteSpace(programName))
			{
				helpTokens.Add(programName);
			}

			helpTokens.AddRange(command.Arguments.Select(a => a.GetHelpToken()));
			helpTokens.AddRange(command.Options.Select(o => o.GetHelpToken()));

			var tokens = string.Join(" ", helpTokens);
			sb.Append(tokens);

			return sb.ToString();
		}
	}
}
