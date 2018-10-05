using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.RunInfoBuilder.Help
{
	internal static class HelpBuilder
	{
		private const string Padding = "  ";
		private static string PaddingRepeated(int repeatCount) => new StringBuilder().Insert(0, Padding, repeatCount).ToString();

		internal static string BuildFor<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class
		{
			var sb = new StringBuilder();

			sb.Append(command.Key + " - ");

			if (!string.IsNullOrWhiteSpace(command.Description))
			{
				sb.AppendLine(command.Description);
			}

			sb.AppendLine(Padding + "Usage: ");

			AppendCommandInfo(sb, command.Key, isRoot: true, command, commandDepth: 0);

			return sb.ToString();
		}

		private static void AppendCommandInfo<TRunInfo>(StringBuilder sb,
			string rootCommandKey, bool isRoot, Command<TRunInfo> command, int commandDepth)
			where TRunInfo : class
		{
			var helpTokens = new List<string> { rootCommandKey };
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
			sb.AppendLine(PaddingRepeated(2) + result);

			// recursively add subcommands with more padding
			foreach(var subCommand in command.SubCommands)
			{
				AppendCommandInfo(sb, rootCommandKey, isRoot: false, subCommand, commandDepth + 1);
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

		//private static string BuildForCommandInternal<TRunInfo>(StringBuilder sb, Command<TRunInfo> command, int paddingDepth)
		//	where TRunInfo : class
		//{
			
		//	var tokens = string.Join(" ", argumentHelpTokens.Concat(optionHelpTokens));
		//	sb.Append(tokens);

		//	if (command.SubCommands.Any())
		//	{
		//		string subcommands = " (";
		//		subcommands += string.Join("|", command.SubCommands.Select(c => c.Key));
		//		sb.AppendLine(subcommands + ")");
		//	}
		//	else
		//	{
		//		sb.AppendLine();
		//	}

		//	foreach (var sc in command.SubCommands)
		//	{
		//		BuildForCommandInternal<TRunInfo>(sb, sc, paddingDepth + 2);
		//	}

		//	return sb.ToString();
		//}

		//private static string BuildForCommandInternal<TRunInfo>(StringBuilder sb, Command<TRunInfo> command, int paddingDepth)
		//	where TRunInfo : class
		//{
		//	string padding_1 = PaddingRepeated(paddingDepth);
		//	string padding_2 = PaddingRepeated(paddingDepth + 1);

		//	sb.Append(padding_1 + command.Key + " - ");

		//	if (!string.IsNullOrWhiteSpace(command.Description))
		//	{
		//		sb.AppendLine(command.Description);
		//	}

		//	sb.Append(padding_2 + $"Usage: {command.Key} ");

		//	IEnumerable<string> argumentHelpTokens = command.Arguments.Select(a => a.GetHelpToken());
		//	IEnumerable<string> optionHelpTokens = command.Options.Select(o => o.GetHelpToken());

		//	var tokens = string.Join(" ", argumentHelpTokens.Concat(optionHelpTokens));
		//	sb.Append(tokens);

		//	if (command.SubCommands.Any())
		//	{
		//		string subcommands = " (";
		//		subcommands += string.Join("|", command.SubCommands.Select(c => c.Key));
		//		sb.AppendLine(subcommands + ")");
		//	}
		//	else
		//	{
		//		sb.AppendLine();
		//	}

		//	foreach (var sc in command.SubCommands)
		//	{
		//		BuildForCommandInternal<TRunInfo>(sb, sc, paddingDepth + 2);
		//	}

		//	return sb.ToString();
		//}

		internal static string BuildFor<TRunInfo>(DefaultCommand<TRunInfo> command)
			where TRunInfo : class
		{
			var sb = new StringBuilder();

			sb.AppendLine("Default Command");

			if (!string.IsNullOrWhiteSpace(command.Description))
			{
				sb.AppendLine(Padding + $"{command.Description}");
			}

			sb.Append(Padding + "Usage: ");

			IEnumerable<string> argumentHelpTokens = command.Arguments.Select(a => a.GetHelpToken());
			IEnumerable<string> optionHelpTokens = command.Options.Select(o => o.GetHelpToken());

			var tokens = string.Join(" ", argumentHelpTokens.Concat(optionHelpTokens));
			sb.Append(tokens);

			return sb.ToString();
		}
	}
}
