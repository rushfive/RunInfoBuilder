using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace R5.RunInfoBuilder.Builder
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
			//_store
			//	.AddCommand(new Command<GitRunInfo, bool>
			//	{
			//		Key = "status",
			//		Description = "Lists all new or modified files to be committed",
			//		Usage = "git status",
			//		Mapping =
			//		{

			//		}
			//	});
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

		IArgumentStore AddCommand<TRunInfo, TProperty>(Command<TRunInfo, TProperty> command)
			where TRunInfo : class;

		IArgumentStore AddDefault();
	}

	[Obsolete]
	public class Commands<TRunInfo> 
		where TRunInfo : class
	{
		private Dictionary<string, CommandBase<TRunInfo>> _commandMap
			= new Dictionary<string, CommandBase<TRunInfo>>();

		public CommandBase<TRunInfo> this[string key]
		{
			get
			{
				if (!_commandMap.TryGetValue(key, out CommandBase<TRunInfo> command))
				{
					//throw new KeyNotFoundException($"'{key}' is not a valid command.");
					return null;
				}
				return command;
			}
			
			set
			{
				if (string.IsNullOrWhiteSpace(key))
				{
					throw new ArgumentNullException(nameof(key), "Valid key must be provided.");
				}
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value), "A valid command object must be provided.");
				}
				if (_commandMap.ContainsKey(key))
				{
					throw new InvalidOperationException($"Command with key '{key}' already exists.");
				}
				_commandMap.Add(key, value);
			}
		}
	}

	public abstract class CommandBase<TRunInfo> where TRunInfo : class
	{
		public string Key { get; set; }
		public string Description { get; set; }
		public string Usage { get; set; }
		public Callback<TRunInfo> Callback { get; set; } = new Callback<TRunInfo>();

		public List<CommandBase<TRunInfo>> SubCommands { get; set; } = new List<CommandBase<TRunInfo>>();
		public List<ArgumentBase> Arguments { get; set; } = new List<ArgumentBase>();
		public List<Option<TRunInfo>> Options { get; set; } = new List<Option<TRunInfo>>();
	}

	public class Command<TRunInfo, TProperty> : CommandBase<TRunInfo>
		where TRunInfo : class
	{
		public PropertyMapping<TRunInfo, TProperty> Mapping { get; set; } = new PropertyMapping<TRunInfo, TProperty>();
	}

	public class Command<TRunInfo> : CommandBase<TRunInfo>
		where TRunInfo : class
	{
		
	}


	// add callbacks to these??
	public abstract class ArgumentBase//<TRunInfo> where TRunInfo : class
	{ }

	public class Argument<TRunInfo, TProperty> : ArgumentBase
		where TRunInfo : class
	{
		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }
	}

	public class ExclusiveArgumentSet<TRunInfo> : ArgumentBase, IEnumerable<ArgumentBase>
		where TRunInfo : class
	{
		private readonly List<ArgumentBase> _arguments = new List<ArgumentBase>();

		public void Add(ArgumentBase argument) => _arguments.Add(argument);

		public IEnumerator<ArgumentBase> GetEnumerator() => _arguments.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	public class ArgumentList<TRunInfo, TListProperty> : ArgumentBase
		where TRunInfo : class
	{
		public Expression<Func<TRunInfo, List<TListProperty>>> List { get; set; }
	}


	public class PropertyMapping<TRunInfo, TProperty>
		where TRunInfo : class
	{
		// use for "SET BY USER" check
		public Expression<Func<TRunInfo, TProperty>> Property { get; set; }
		public TProperty Value { get; set; }
	}

	public class Option<TRunInfo>
		where TRunInfo : class
	{
		public string Key { get; set; }
		public string Description { get; set; }
		public string Usage { get; set; }
		public List<ArgumentBase> Arguments { get; set; }
		// if only a single argument, will assume the NEXT token (if delimited by space) is its value
		//    - even if the next value is technically a valid configuerd option
	}

	// callback
	public class Callback<TRunInfo> where TRunInfo : class
	{
		// use for "SET BY USER" check
		public Func<ProcessContext<TRunInfo>, Result> Func { get; set; }
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

	public static class CallbackResult
	{
		public static readonly Result Continue = new Continue();
		public static readonly Result KillProcess = new KillProcess();
		public static Result Skip(int count) => new Skip(count);
	}

	public abstract class Result
	{

	}

	public class Continue : Result
	{

	}

	public class Skip : Result
	{
		internal Skip(int count) { }
	}

	public class KillProcess : Result
	{

	}
}
