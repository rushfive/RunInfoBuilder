using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace R5.RunInfoBuilder.Processor
{
	internal interface ICommandProcessor
	{

	}

	internal class CommandProcessor : ICommandProcessor
	{

		public CommandProcessor()
		{

		}
	}



	// for each of these commands, we should lex and parse into a DAG
	// such that processing a list of arguments starts at the root and we simply
	// follow a path?
	internal class ProcessCommand<TRunInfo>
		where TRunInfo : class
	{
		private TRunInfo _runInfo { get; set; }
		private TRunInfo _defaultRunInfo { get; set; }

		internal ProcessCommand(Command<TRunInfo> command, TRunInfo implementation = null)
		{
			if (implementation != null)
			{
				_defaultRunInfo = implementation.Copy();
			}
		}

		internal ProcessCommand(DefaultCommand<TRunInfo> defaultCommand, TRunInfo implementation = null)
		{

		}

		internal TRunInfo Process()
		{
			TRunInfo runInfo = GetStartingRunInfo();


			// do processing


			return runInfo;
		}

		private TRunInfo GetStartingRunInfo()
		{
			return _defaultRunInfo == null
				? (TRunInfo)Activator.CreateInstance(typeof(TRunInfo))
				: _defaultRunInfo.Copy();
		}
	}




	// (inspired by) behavior tree implementation

	// trees should be built when commands are added (use a builder for this probably)

	internal abstract class ProcessTreeNode<TRunInfo>
		where TRunInfo : class
	{
		private Queue<string> _programArguments { get; }
		private Func<ProcessContext<TRunInfo>, ProcessStageResult> _callback { get; }

		protected ProcessTreeNode(
			Queue<string> programArguments,
			Func<ProcessContext<TRunInfo>, ProcessStageResult> callback)
		{
			_programArguments = programArguments;
			_callback = callback;
		}

		protected bool HasNext() => _programArguments.Any();

		protected string PeekNext() => _programArguments.Peek();

		protected string GetNext() => _programArguments.Dequeue();

		protected ProcessStageResult InvokeCallback(ProcessContext<TRunInfo> context)
		{
			if (_callback == null)
			{
				return ProcessResult.Continue;
			}

			return _callback(context);
		}

		protected abstract ProcessStageResult ProcessNode(ProcessContext<TRunInfo> context); // todo: return type of some result?
	}

	//internal class CallbackNode<TRunInfo> : ProcessTreeNode<TRunInfo>
	//	where TRunInfo : class
	//{
	//	private Func<ProcessContext<TRunInfo>, ProcessNodeResult> _callback { get; }

	//	internal CallbackNode(Func<ProcessContext<TRunInfo>, ProcessNodeResult> callback)
	//	{
	//		_callback = callback;
	//	}

	//	protected override ProcessNodeResult ProcessNode(ProcessContext<TRunInfo> context)
	//	{
	//		return _callback(context);
	//	}
	//}

	//internal class ArgumentPropertyMappedNode<TRunInfo, TProperty> : ProcessTreeNode<TRunInfo>
	//	where TRunInfo : class
	//{
	//	private Expression<Func<TRunInfo, TProperty>> _property { get; }

	//	internal ArgumentPropertyMappedNode(
	//		Expression<Func<TRunInfo, TProperty>> property,
	//		Queue<string> programArguments,
	//		Func<ProcessContext<TRunInfo>, ProcessStageResult> callback)
	//		: base(programArguments, callback)
	//	{
	//		_property = property;
	//	}

	//	protected override ProcessStageResult ProcessNode(ProcessContext<TRunInfo> context)
	//	{
	//		ProcessStageResult result = InvokeCallback(context);
	//		if (result != ProcessResult.Continue)
	//		{
	//			return result;
	//		}

	//		if (!context.Parser.HandlesType<TProperty>())
	//		{
	//			throw new InvalidOperationException($"Failed to process program argument '{context.Token}' because the "
	//				+ $"parser cannot handle the property type of '{typeof(TProperty).Name}'.");
	//		}

	//		if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(_property, out string propertyName))
	//		{
	//			throw new InvalidOperationException($"Failed to process program argument '{context.Token}' because the "
	//				+ $"property '{propertyName}' is not writable.");
	//		}

	//		if (!context.Parser.TryParseAs<TProperty>(context.Token, out TProperty parsed))
	//		{
	//			throw new InvalidOperationException($"Failed to process program argument '{context.Token}' because it "
	//				+ $"couldn't be parsed into a '{typeof(TProperty).Name}'.");
	//		}

	//		PropertyInfo propertyInfo = ReflectionHelper<TRunInfo>.GetPropertyInfoFromExpression(_property);
	//		propertyInfo.SetValue(context.RunInfo, parsed);

	//		return ProcessResult.Continue;
	//	}
	//}

	//internal class ArgumentSequenceNode<TRunInfo, TListProperty> : ProcessTreeNode<TRunInfo>
	//	where TRunInfo : class
	//{
	//	private Expression<Func<TRunInfo, List<TListProperty>>> _listProperty { get; }
	//	private IArgumentParser _parser { get; }
	//	private (List<string>, List<char>)? _availableOptions { get; }
	//	private List<string> _availableSubCommands { get; }

	//	internal ArgumentSequenceNode(
	//		Expression<Func<TRunInfo, List<TListProperty>>> listProperty,
	//		IArgumentParser parser,
	//		List<string> availableOptions,
	//		List<string> availableSubCommands,
	//		Queue<string> programArguments,
	//		Func<ProcessContext<TRunInfo>, ProcessStageResult> callback)
	//		: base(programArguments, callback)
	//	{
	//		_listProperty = listProperty;
	//		_parser = parser;
	//		_availableOptions = TokenizeAvailableOptions(availableOptions);
	//		_availableSubCommands = availableSubCommands;
	//	}

	//	private (List<string>, List<char>)? TokenizeAvailableOptions(List<string> availableOptions)
	//	{
	//		if (!availableOptions.Any())
	//		{
	//			return null;
	//		}

	//		var fullKeys = new List<string>();
	//		var shortKeys = new List<char>();

	//		foreach (string option in availableOptions)
	//		{
	//			if (!OptionHelper.TryTokenize(option, out (string, char?)? result))
	//			{
	//				throw new ArgumentException($"Failed to tokenize option '{option}'.", nameof(availableOptions));
	//			}

	//			var (fullKey, shortKey) = result.Value;

	//			fullKeys.Add(fullKey);

	//			if (shortKey.HasValue)
	//			{
	//				shortKeys.Add(shortKey.Value);
	//			}
	//		}

	//		return (fullKeys, shortKeys);
	//	}

	//	protected override ProcessStageResult ProcessNode(ProcessContext<TRunInfo> context)
	//	{
	//		ProcessStageResult result = InvokeCallback(context);
	//		if (result != ProcessResult.Continue)
	//		{
	//			return result;
	//		}

	//		PropertyInfo propertyInfo = ReflectionHelper<TRunInfo>.GetPropertyInfoFromExpression(_listProperty);

	//		// initialize list if null
	//		var list = (IList<TListProperty>)propertyInfo.GetValue(context.RunInfo, null);
	//		if (list == null)
	//		{
	//			propertyInfo.SetValue(context.RunInfo, Activator.CreateInstance(propertyInfo.PropertyType));
	//		}

	//		// Iterate over proceeding program args, adding parseable items to list.
	//		// Ends when all program args are exhausted or when an option or subcommand 
	//		// has been identified.
	//		while (HasNext())
	//		{
	//			string nextProgramArgument = PeekNext();

	//			bool nextIsOption = _availableOptions.HasValue && OptionHelper.IsValidOption(nextProgramArgument, _availableOptions.Value);
	//			if (nextIsOption)
	//			{
	//				return ProcessResult.Continue;
	//			}

	//			bool nextIsSubCommand = _availableSubCommands.Contains(nextProgramArgument);
	//			if (nextIsSubCommand)
	//			{
	//				return ProcessResult.Continue;
	//			}

	//			nextProgramArgument = GetNext();

	//			if (!_parser.TryParseAs(nextProgramArgument, out TListProperty parsed))
	//			{
	//				throw new ArgumentException($"Failed to parse '{nextProgramArgument}' as type '{typeof(TListProperty).Name}'.");
	//			}

	//			list.Add(parsed);
	//		}

	//		return ProcessResult.End;
	//	}
	//}

	// Processing Options:
	// For each option configured, create the relevant node instance.
	// After all are created, create a map (dictionary) of key to the node instance
	// to process, just iterate throug the program args as follows:
	// - if valid option and matching, invoke/process that node
	// - if end of list, return RESULT.END
	// - if next is subcommand (via peek), return RESULT.CONTINUE

	internal class OptionAsFlagNode<TRunInfo> : ProcessTreeNode<TRunInfo>
		where TRunInfo : class
	{
		public Expression<Func<TRunInfo, bool>> _property { get; }

		internal OptionAsFlagNode(
			Expression<Func<TRunInfo, bool>> property,
			Queue<string> programArguments,
			Func<ProcessContext<TRunInfo>, ProcessStageResult> callback)
			: base(programArguments, callback)
		{
			_property = property;
		}

		protected override ProcessStageResult ProcessNode(ProcessContext<TRunInfo> context)
		{
			ProcessStageResult result = InvokeCallback(context);
			if (result != ProcessResult.Continue)
			{
				return result;
			}

			PropertyInfo propertyInfo = ReflectionHelper<TRunInfo>.GetPropertyInfoFromExpression(_property);
			propertyInfo.SetValue(context.RunInfo, true);

			return ProcessResult.Continue;
		}
	}
}
