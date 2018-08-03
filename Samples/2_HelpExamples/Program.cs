using R5.RunInfoBuilder.Help;
using System;
using static System.Console;

namespace R5.RunInfoBuilder.Samples.HelpExamples
{
	public class RunInfo
	{
		public Command Command { get; set; }
		public bool RunAsRoot { get; set; }
		public bool OverwriteExisting { get; set; }
		public DateTime BeginDate { get; set; }
		public DateTime EndDate { get; set; }
		public OnFail OnFail { get; set; }
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
			

			Console.ReadKey();
        }
    }
}
