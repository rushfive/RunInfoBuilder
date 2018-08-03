using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace R5.RunInfoBuilder2.Builder
{
	public class Test
	{
		public enum Command
		{
			Status,
			Diff
		}

		public class GitRunInfo
		{
			public bool Bool { get; set; }
		}

		IArgumentStore _store { get; }

		void Main()
		{
			_store
				.AddCommand(new Command<GitRunInfo>
				{
					Key = "status",
					Description = "Lists all new or modified files to be committed",
					Usage = "git status",

				});
		}
	}
    public class RunInfoBuilder2
    {
		public IArgumentStore Store { get; }
    }

	public interface IArgumentStore
	{
		IArgumentStore AddCommand<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class;

		IArgumentStore AddDefault();
	}

	public class Command<TRunInfo, TProperty>
		where TRunInfo : class
	{
		public string Key { get; set; }
		public string Description { get; set; }
		public string Usage { get; set; }

		public PropertyMapping<TRunInfo, TProperty> Mapping { get; set; } // look into collection initializer
		public Callback<TRunInfo> Callback { get; set; } // same
		public List<Command<TRunInfo>> SubCommands { get; set; }
		public List<Argument> Arguments { get; set; }
		public List<Option<TRunInfo>> Options { get; set; }
	}

	public class Command<TRunInfo>
		where TRunInfo : class
	{
		public string Key { get; set; }
		public string Description { get; set; }
		public string Usage { get; set; }
		public Callback<TRunInfo> Callback { get; set; }

		public List<Command<TRunInfo>> SubCommands { get; set; }
		public List<Argument> Arguments { get; set; }
		public List<Option<TRunInfo>> Options { get; set; }
	}

	public abstract class Argument//<TRunInfo> where TRunInfo : class
	{ }

	public class Argument<TRunInfo, TProperty> : Argument
		where TRunInfo : class
	{
		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }
	}

	public class ExclusiveArgumentSet<TRunInfo, TProperty> : Argument
		where TRunInfo : class
	{
		public List<Argument> Arguments { get; set; }
	}

	public class ArgumentList<TRunInfo, TListProperty> : Argument
		where TRunInfo : class
	{
		public Expression<Func<TRunInfo, List<TListProperty>>> List { get; set; }
	}


	public class PropertyMapping<TRunInfo, TProperty>
		where TRunInfo : class
	{
		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }
		public TProperty Value { get; set; }
	}

	public class Option<TRunInfo>
		where TRunInfo : class
	{
		public string Key { get; set; }
		public string Description { get; set; }
		public string Usage { get; set; }
		public List<Argument> Arguments { get; set; }
		// if only a single argument, will assume the NEXT token (if delimited by space) is its value
		//    - even if the next value is technically a valid configuerd option
	}

	// callback
	public class Callback<TRunInfo> where TRunInfo : class
	{
		public Func<ProcessContext<TRunInfo>, CallbackResult> Func { get; set; }
		public CallbackTiming Timing { get; set; }
		public CallbackOrder Order { get; set; }
	}

	public enum CallbackTiming
	{
		Immediately,
		AfterProcessing
	}

	public enum CallbackOrder
	{
		InOrder,
		Parallel
	}

	public class ProcessContext<TRunInfo> where TRunInfo : class
	{

	}

	public abstract class CallbackResult
	{

	}

	public class Continue : CallbackResult
	{

	}

	public class Skip : CallbackResult
	{

	}

	public class KillProcess : CallbackResult
	{

	}
}
