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
		private Func<string> _peekNextFunc { get; }

		protected ProcessTreeNode(
			//Func<string> peekNextFunc
			//Queue<string> programArguments
			)
		{
			//_programArguments = programArguments;
			//_peekNextFunc = peekNextFunc;
		}


		protected bool TryPeekNextArgument(out string next)
		{
			next = null;

			if (!_programArguments.Any())
			{
				return false;
			}

			next = _programArguments.Peek();
			return true;
		}

		protected bool TryGetNextArgument(out string next)
		{
			next = null;

			if (!_programArguments.Any())
			{
				return false;
			}

			next = _programArguments.Dequeue();
			return true;
		}

		protected abstract ProcessNodeResult ProcessNode(ProcessContext<TRunInfo> context); // todo: return type of some result?
	}

	internal class CallbackNode<TRunInfo> : ProcessTreeNode<TRunInfo>
		where TRunInfo : class
	{
		private Func<ProcessContext<TRunInfo>, ProcessNodeResult> _callback { get; }

		internal CallbackNode(Func<ProcessContext<TRunInfo>, ProcessNodeResult> callback)
		{
			_callback = callback;
		}

		protected override ProcessNodeResult ProcessNode(ProcessContext<TRunInfo> context)
		{
			return _callback(context);
		}
	}

	internal class ArgumentPropertyMappedNode<TRunInfo, TProperty> : ProcessTreeNode<TRunInfo>
		where TRunInfo : class
	{
		private Expression<Func<TRunInfo, TProperty>> _property { get; }

		internal ArgumentPropertyMappedNode(Expression<Func<TRunInfo, TProperty>> property)
		{
			_property = property;
		}

		protected override ProcessNodeResult ProcessNode(ProcessContext<TRunInfo> context)
		{
			if (!context.Parser.HandlesType<TProperty>())
			{
				throw new InvalidOperationException($"Failed to process program argument '{context.Token}' because the "
					+ $"parser cannot handle the property type of '{typeof(TProperty).Name}'.");
			}

			if (!ReflectionHelper<TRunInfo>.PropertyIsWritable(_property, out string propertyName))
			{
				throw new InvalidOperationException($"Failed to process program argument '{context.Token}' because the "
					+ $"property '{propertyName}' is not writable.");
			}

			if (!context.Parser.TryParseAs<TProperty>(context.Token, out TProperty parsed))
			{
				throw new InvalidOperationException($"Failed to process program argument '{context.Token}' because it "
					+ $"couldn't be parsed into a '{typeof(TProperty).Name}'.");
			}

			PropertyInfo propertyInfo = ReflectionHelper<TRunInfo>.GetPropertyInfoFromExpression(_property);
			propertyInfo.SetValue(context.RunInfo, parsed);

			return ProcessResult.Continue;
		}
	}

	internal class ArgumentSequenceNode<TRunInfo, TListProperty> : ProcessTreeNode<TRunInfo>
		where TRunInfo : class
	{
		private Expression<Func<TRunInfo, List<TListProperty>>> _listProperty { get; }
		private IArgumentParser _parser { get; }
		private IOptionTokenizer _optionTokenizer { get; }
		private (List<string>, List<char>)? _availableOptions { get; }
		private List<string> _availableSubCommands { get; }

		internal ArgumentSequenceNode(
			Expression<Func<TRunInfo, List<TListProperty>>> listProperty,
			IArgumentParser parser,
			IOptionTokenizer optionTokenizer,
			List<string> availableOptions,
			List<string> availableSubCommands)
		{
			_listProperty = listProperty;
			_parser = parser;
			_optionTokenizer = optionTokenizer;
			_availableOptions = TokenizeAvailableOptions(availableOptions);
			_availableSubCommands = availableSubCommands;
		}
		
		private (List<string>, List<char>)? TokenizeAvailableOptions(List<string> availableOptions)
		{
			if (!availableOptions.Any())
			{
				return null;
			}

			var fullKeys = new List<string>();
			var shortKeys = new List<char>();

			foreach(string option in availableOptions)
			{
				if (!_optionTokenizer.TryTokenize(option, out (string, char?)? result))
				{
					throw new ArgumentException($"Failed to tokenize option '{option}'.", nameof(availableOptions));
				}

				var (fullKey, shortKey) = result.Value;

				fullKeys.Add(fullKey);

				if (shortKey.HasValue)
				{
					shortKeys.Add(shortKey.Value);
				}
			}

			return (fullKeys, shortKeys);
		}

		protected override ProcessNodeResult ProcessNode(ProcessContext<TRunInfo> context)
		{
			PropertyInfo propertyInfo = ReflectionHelper<TRunInfo>.GetPropertyInfoFromExpression(_listProperty);

			// initialize list if null
			var list = (IList<TListProperty>)propertyInfo.GetValue(context.RunInfo, null);
			if (list == null)
			{
				propertyInfo.SetValue(context.RunInfo, Activator.CreateInstance(propertyInfo.PropertyType));
			}

			// keep getting next program args until:
			// hit a valid option
			// hit end of program args list
			// hit a subcommand key
			// cannot parse token into type TListProperty

			if (!TryPeekNextArgument(out string nextProgramArgument))
			{
				return ProcessResult.End;
			}


			

			throw new NotImplementedException();
		}

		private void AddItem(IList<TListProperty> list)
		{

		}
	}
}
