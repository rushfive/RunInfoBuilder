using OLD.Help;
using OLD.Process;
using OLD.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace OLD.Store
{
	public interface IArgumentStore<TRunInfo>
		where TRunInfo : class
	{
		IArgumentStore<TRunInfo> AddArgument<TPropertyType>(string key, Expression<Func<TRunInfo, TPropertyType>> propertyExpression,
			Func<object, bool> validateFunction = null, string validatorDescription = null, string description = null);
		
		IArgumentStore<TRunInfo> AddCommand<TPropertyType>(string key, Expression<Func<TRunInfo, TPropertyType>> propertyExpression,
					TPropertyType mappedValue, string description = null);

		IArgumentStore<TRunInfo> AddCommand(string key, Func<ProcessContext<TRunInfo>, ProcessStageResult> callback,
			string description = null);

		IArgumentStore<TRunInfo> AddCommand<TPropertyType>(string key, Expression<Func<TRunInfo, TPropertyType>> propertyExpression,
			TPropertyType mappedValue, Func<ProcessContext<TRunInfo>, ProcessStageResult> callback, string description = null);

		IArgumentStore<TRunInfo> AddOption(string fullKey, Expression<Func<TRunInfo, bool>> propertyExpression,
			char? shortKey = null, string description = null);
	}

	internal class ArgumentStore<TRunInfo> : IArgumentStore<TRunInfo>
		where TRunInfo : class
	{
		private IArgumentStoreValidator<TRunInfo> _storeValidator { get; }
		private IRestrictedKeyValidator _keyValidator { get; }
		private IArgumentMetadata<TRunInfo> _argumentMaps { get; }
		private IReflectionHelper<TRunInfo> _reflectionHelper { get; }

		public ArgumentStore(
			IArgumentStoreValidator<TRunInfo> storeValidator,
			IRestrictedKeyValidator keyValidator,
			IArgumentMetadata<TRunInfo> argumentMaps,
			IReflectionHelper<TRunInfo> reflectionHelper)
		{
			_storeValidator = storeValidator;
			_keyValidator = keyValidator;
			_argumentMaps = argumentMaps;
			_reflectionHelper = reflectionHelper;
		}

		public IArgumentStore<TRunInfo> AddArgument<TPropertyType>(string key, Expression<Func<TRunInfo, TPropertyType>> propertyExpression,
			Func<object, bool> validateFunction = null, string validatorDescription = null, string description = null)
		{
			_storeValidator.ValidateArgument(key, propertyExpression, validateFunction, validatorDescription);

			PropertyInfo propertyInfo = _reflectionHelper.GetPropertyInfoFromExpression(propertyExpression);
			var metadata = new ArgumentMetadata(key, description, validatorDescription, validateFunction, propertyInfo);

			_argumentMaps.AddArgument(key, metadata);

			_keyValidator.AddRestrictedKey(key);

			return this;
		}
		
		public IArgumentStore<TRunInfo> AddCommand<TPropertyType>(string key, Expression<Func<TRunInfo, TPropertyType>> propertyExpression,
			TPropertyType mappedValue, string description = null)
		{
			return AddCommandInternal(CommandType.PropertyMapped, key, propertyExpression, mappedValue, null, description);
		}

		public IArgumentStore<TRunInfo> AddCommand(string key, Func<ProcessContext<TRunInfo>, ProcessStageResult> callback,
			string description = null)
		{
			return AddCommandInternal<object>(CommandType.CustomCallback, key, null, null, callback, description);
		}

		public IArgumentStore<TRunInfo> AddCommand<TPropertyType>(string key, Expression<Func<TRunInfo, TPropertyType>> propertyExpression,
			TPropertyType mappedValue, Func<ProcessContext<TRunInfo>, ProcessStageResult> callback, string description = null)
		{
			return AddCommandInternal(CommandType.MappedAndCallback, key, propertyExpression, mappedValue, callback, description);
		}

		private IArgumentStore<TRunInfo> AddCommandInternal<TPropertyType>(CommandType type, string key, Expression<Func<TRunInfo, TPropertyType>> propertyExpression,
			TPropertyType mappedValue, Func<ProcessContext<TRunInfo>, ProcessStageResult> callback, string description)
		{
			_storeValidator.ValidateCommand(type, key, propertyExpression, mappedValue, callback);

			PropertyInfo propertyInfo = propertyExpression != null
				? _reflectionHelper.GetPropertyInfoFromExpression(propertyExpression)
				: null;

			var metadata = new CommandMetadata<TRunInfo>(key, type, description, propertyInfo, mappedValue, callback);

			_argumentMaps.AddCommand(key, metadata);

			_keyValidator.AddRestrictedKey(key);

			return this;
		}

		public IArgumentStore<TRunInfo> AddOption(string fullKey, Expression<Func<TRunInfo, bool>> propertyExpression,
			char? shortKey = null, string description = null)
		{
			_storeValidator.ValidateOption(fullKey, propertyExpression, shortKey);

			PropertyInfo propertyInfo = _reflectionHelper.GetPropertyInfoFromExpression(propertyExpression);

			_argumentMaps.AddOption(fullKey, shortKey, new OptionMetadata(fullKey, shortKey, description, propertyInfo));

			_keyValidator.AddRestrictedKey(fullKey);

			if (shortKey.HasValue)
			{
				_keyValidator.AddRestrictedKey(shortKey.Value);
			}

			return this;
		}
	}
}
