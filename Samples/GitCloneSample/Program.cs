using R5.RunInfoBuilder.Samples.HelpExamples.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace R5.RunInfoBuilder.Samples.HelpExamples
{
	class Program
	{
		static void Main(string[] args)
		{
			var builder = new RunInfoBuilder();

			builder.Commands.Add<TestRunInfo>(new Command<TestRunInfo>
			{
				Key = "command",
				Arguments =
				{
					new CustomArgument<TestRunInfo>
					{
						Count = 1,
						HelpToken = "<test>",
						Handler = context =>
						{
							context.RunInfo.String1 = context.ProgramArguments.Single();
							return ProcessResult.Continue;
						}
					},
					new SetArgument<TestRunInfo, bool>
					{
						Property = ri => ri.Bool1,
						Values = new List<(string Label, bool Value)>
						{
							("true", true),
							("false", false)
						}
					}
				},
				Options =
				{
					new Option<TestRunInfo, bool>
					{
						Key = "bool | b",
						Property = ri => ri.Bool1
					}
				}
			},
			ri =>
			{
				Console.WriteLine("Processing RUNINFO after finished building.");
				Console.WriteLine("String1 was set to = " + ri.String1);
			});

			builder.Help.DisplayOnBuildFail();



			//var runInfo = builder.Build(new string[] { "command", "hello there" });
			var runInfo = builder.Build(new string[] { "--help" });


			Console.ReadKey();
		}

		// uncomment below, real code
		//static void Main(string[] args)
		//{
		//	var builder = new RunInfoBuilder();

		//	builder.Version.Set("2.19.1");

		//	builder.Help
		//		.SetProgramName("git")
		//		.DisplayHelpOnBuildFail();

		//	ConfigureCommands(builder);

		//	builder.Build(new string[] { });
		//	Console.ReadKey();
		//}

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
