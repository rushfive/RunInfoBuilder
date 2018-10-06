using R5.RunInfoBuilder.Samples.HelpExamples.Commands;
using System;
using System.Collections.Generic;
using static System.Console;

namespace R5.RunInfoBuilder.Samples.HelpExamples
{
	class Program
	{
		static void Main(string[] args)
		{
			var builder = new RunInfoBuilder();

			builder.Version.Set("2.19.1");

			builder.Help
				.SetProgramName("git")
				.DisplayHelpOnBuildFail();

			ConfigureCommands(builder);

			builder.Build(new string[] { });
			Console.ReadKey();
		}

		private static void ConfigureCommands(RunInfoBuilder builder)
		{
			builder.Commands
				.Add(Commit.GetCommandConfiguration())
				.Add(Commit.GetCommandConfiguration2());
		}
	}

	public class TestRunInfo
	{
		public List<string> StringList1 { get; set; }
		public List<int> IntList1 { get; set; }
		public List<int> IntList2 { get; set; }
		public List<string> UnwritableStringList { get; }
		public string String1 { get; set; }
		public bool Bool1 { get; set; }
		public bool Bool2 { get; set; }
		public bool Bool3 { get; set; }
		public int Int1 { get; set; }
		public int Int2 { get; set; }
		public int Int3 { get; set; }
		public bool UnwritableBool { get; }
	}
}
