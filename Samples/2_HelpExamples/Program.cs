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

			builder.Commands
				.Add(new Command<TestRunInfo>
				{
					Key = "command",
					Description = "command description awoei fjoiawjef",
					Arguments =
						{
							new PropertyArgument<TestRunInfo, int>
							{
								Property = ri => ri.Int1
							}
						},
					Options =
						{
							new Option<TestRunInfo, bool>
							{
								Key = "bool | b",
								Property = ri => ri.Bool1
							}
						},
					SubCommands =
					{
						new Command<TestRunInfo>
						{
							Key = "sub1",
							Description = "sub command 1 description",
							Arguments =
							{
								new PropertyArgument<TestRunInfo, int>
								{
									Property = ri => ri.Int1
								}
							}
						},
						new Command<TestRunInfo>
						{
							Key = "sub2",
							Description = "sub command 2 description",
							Options =
							{
								new Option<TestRunInfo, bool>
								{
									Key = "bool | b",
									Property = ri => ri.Bool1
								}
							}
						}
					}
				})
				.AddDefault(new DefaultCommand<TestRunInfo>
				 {
					 Description = "Pass in date specifications to request time specific data.",
					 Arguments =
					{
						new PropertyArgument<TestRunInfo, int>
						{
							Property = ri => ri.Int1
						},
						new SequenceArgument<TestRunInfo, string>
						{
							ListProperty = ri => ri.StringList1
						},
						new CustomArgument<TestRunInfo>
						{
							Count = 3,
							Handler = context => ProcessResult.Continue,
							HelpToken = "<month> <day> <year>"
						}
					},
					 Options =
					{
						new Option<TestRunInfo, bool>
						{
							Key = "bool | b",
							Property = ri => ri.Bool1
						},
						new Option<TestRunInfo, int>
						{
							Key = "int | i",
							Property = ri => ri.Int1
						}
					}
				 });

			Console.ReadKey();
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
