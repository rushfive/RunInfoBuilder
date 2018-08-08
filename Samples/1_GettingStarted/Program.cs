using OLD;
using OLD.ArgumentParser;
using OLD.Configuration;
using OLD.Help;
using R5.RunInfoBuilder.Command;
using R5.RunInfoBuilder.Command.Models;
using System;
using System.Collections.Generic;
using static System.Console;

namespace R5.RunInfoBuilder.Samples.GettingStarted
{
	public class RunInfo
	{
		public Command Command { get; set; }
		public bool RunAsRoot { get; set; }
		public bool OverwriteExisting { get; set; }
		public DateTime BeginDate { get; set; }
		public DateTime EndDate { get; set; }
		public OnFail OnFail { get; set; }
		public List<string> RandomTokens { get; set; }
	}

	public enum Command
	{
		Read,
		Write,
		Execute,
		Delete
	}

	public enum OnFail
	{
		Log,
		Crash
	}

	class Program
	{
		static void Main(string[] args)
		{
			var test = new CommandPropertyMapped<RunInfo, bool>
			{
				Key = "status",
				Description = "Lists all new or modified files to be committed",
				Mapping = //new PropertyMapping<RunInfo, bool>
				{
					Property = r => r.OverwriteExisting,
					Value = true
				},
				Callback = //new Callback<RunInfo>
				{
					Timing = CallbackTiming.AfterProcessing,
					Order = CallbackOrder.Parallel,
					Func = context => CallbackResult.Continue
				},
				SubCommands =
				{
					new Command<RunInfo>(),
					new CommandPropertyMapped<RunInfo, bool>()
				},
				Arguments =
				{
					new Argument<RunInfo, bool> { Property = r => r.RunAsRoot },
					new ArgumentExclusiveSet<RunInfo>
					{
						new Argument<RunInfo, bool> { Property = r => r.RunAsRoot },
						new ArgumentSequence<RunInfo, string>{ List = r => r.RandomTokens }
					}
				},
				Options =
				{
					new OptionWithArguments<RunInfo>
					{
						Key = "overwrite | o",
						Description = "overwrite existing files. defaults to false if not specified.",
						Usage = "git status --overwrite | git status --overwrite=true/false"
					}
				}
			};






			Console.ReadKey();
			return;

			var setup = new BuilderSetup<RunInfo>();

			setup.AlwaysReturnBuildResult();

			setup.Process
				.AllowUnresolvedArgumentsButThrowOnProcess();

			setup.Parser
				.HandleBuiltInPrimitives()
				.AutoParseEnumCaseSensitive();

			setup.ConfigureHelp(config =>
			{
				config
					.SetTriggers("--help", "?")
					.SetProgramDescription("This program lets you do computer related things.")
					.SetCallback(context =>
					{
						HelpMetadata metadata = context.Metadata;



						WriteLine(context.FormattedText);
					});
			});

			RunInfoBuilder<RunInfo> builder = setup.Create();
			builder.Store
				.AddCommand<Command>("Read", ri => ri.Command, Command.Read, "Program will read.")
				.AddCommand<Command>("Write", ri => ri.Command, Command.Write, "Program will write.")
				.AddCommand<Command>("Execute", ri => ri.Command, Command.Execute, "Program will execute.")
				.AddCommand<Command>("Delete", ri => ri.Command, Command.Delete, "Program will delete.")
				.AddOption("root", ri => ri.RunAsRoot, 'r', "Program will run with elevated privileges.")
				.AddOption("overwrite", ri => ri.OverwriteExisting, 'o', "Program will overwrite any existing data.")
				.AddCommand("Range",
					context =>
					{
						int length = context.ProgramArguments.Count;
						if (length - context.Position + 1 < 2)
						{
							throw new Exception("Range command must be followed by two date time values.");
						}

						var beginRangeArgument = context.ProgramArguments[context.Position + 1];
						var endRangeArgument = context.ProgramArguments[context.Position + 2];

						if (beginRangeArgument.Type != ProgramArgumentType.Unresolved
							|| endRangeArgument.Type != ProgramArgumentType.Unresolved)
						{
							throw new Exception("Range command must be followed by two values that are not configured.");
						}

						IParser parser = context.Parser;

						if (!parser.TryParseAs<DateTime>(beginRangeArgument.ArgumentToken, out DateTime beginRange))
						{
							throw new Exception($"'{beginRangeArgument.ArgumentToken}' is an invalid datetime value.");
						}
						if (!parser.TryParseAs<DateTime>(endRangeArgument.ArgumentToken, out DateTime endRange))
						{
							throw new Exception($"'{endRangeArgument.ArgumentToken}' is an invalid datetime value.");
						}

						context.RunInfo.BeginDate = beginRange;
						context.RunInfo.EndDate = endRange;

						return new ProcessStageResult().SkipNext(2);
					},
					"Specifies that the next two arguments will define a range.")
					.AddArgument<OnFail>("onfail", ri => ri.OnFail, description: "Specify how to handle any program run failures");

			var programArguments = new string[] { "--help" };

			BuildResult<RunInfo> result = builder.Build(programArguments);

			ReadKey();
		}
	}
}
