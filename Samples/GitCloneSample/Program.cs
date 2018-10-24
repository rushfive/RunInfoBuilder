using R5.RunInfoBuilder.Samples.HelpExamples.Commands;
using System;

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
				.DisplayOnBuildFail();

			ConfigureCommands(builder);

			builder.Build(new string[] { "--help" });
			Console.ReadKey();
		}

		private static void ConfigureCommands(RunInfoBuilder builder)
		{
			builder.Commands
				.Add(Add.Configuration)
				.Add(Branch.Configuration)
				.Add(Checkout.Configuration)
				.Add(Commit.Configuration)
				.Add(Diff.Configuration)
				.Add(Init.Configuration);
		}
	}
}
