using R5.RunInfoBuilder.Pipeline;
using R5.RunInfoBuilder.Process;
using R5.RunInfoBuilder.Store;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace R5.RunInfoBuilder.Validators
{
	internal interface IArgumentStoreValidator<TRunInfo>
		where TRunInfo : class
	{
		void ValidateArgument<TPropertyType>(string key, Expression<Func<TRunInfo, TPropertyType>> propertyExpression,
			Func<object, bool> validateFunction = null, string validatorDescription = null);
		
		void ValidateCommand<TPropertyType>(CommandType type, string key, Expression<Func<TRunInfo, TPropertyType>> propertyExpression,
			TPropertyType mappedValue, Func<ProcessContext<TRunInfo>, ProcessStageResult> callback);

		void ValidateOption(string fullOptionKey, Expression<Func<TRunInfo, bool>> propertyExpression,
			char? shortOptionKey = null);
	}

    internal class ArgumentStoreValidator<TRunInfo> : IArgumentStoreValidator<TRunInfo>
		where TRunInfo : class
	{
		private IValidationHelper _validationHelper { get; }
		private IRestrictedKeyValidator _keyValidator { get; }
		private IReflectionHelper<TRunInfo> _reflectionHelper { get; }
		private IArgumentMetadata<TRunInfo> _argumentMaps { get; }

		public ArgumentStoreValidator(
			IValidationHelper validationHelper,
			IRestrictedKeyValidator keyValidator,
			IReflectionHelper<TRunInfo> reflectionHelper,
			IArgumentMetadata<TRunInfo> argumentMaps)
		{
			_validationHelper = validationHelper;
			_keyValidator = keyValidator;
			_reflectionHelper = reflectionHelper;
			_argumentMaps = argumentMaps;
		}

		public void ValidateArgument<TPropertyType>(string key, Expression<Func<TRunInfo, TPropertyType>> propertyExpression,
			Func<object, bool> validateFunction = null, string validatorDescription = null)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentNullException(nameof(key), "Argument key must be provided.");
			}

			if (_argumentMaps.ArgumentExists(key))
			{
				throw new ArgumentException($"Argument with key '{key}' has already been configured.", nameof(key));
			}

			if (_keyValidator.IsRestrictedKey(key))
			{
				throw new ArgumentException($"Argument key '{key}' is restricted.", nameof(key));
			}

			if (propertyExpression == null)
			{
				throw new ArgumentNullException(nameof(propertyExpression),
					"A property expression associated to the argument key must be provided.");
			}

			if (validateFunction == null && !string.IsNullOrWhiteSpace(validatorDescription))
			{
				throw new ArgumentException("Can't add a validator description without also providing the validator function.");
			}

			if (!_reflectionHelper.PropertyIsWritable(propertyExpression, out string propertyName))
			{
				throw new ArgumentException($"Property '{propertyName}' is not writable.", nameof(propertyExpression));
			}
		}
		
		public void ValidateCommand<TPropertyType>(CommandType type, string key, Expression<Func<TRunInfo, TPropertyType>> propertyExpression,
			TPropertyType mappedValue, Func<ProcessContext<TRunInfo>, ProcessStageResult> callback)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentNullException(nameof(key), "Command key must be provided.");
			}

			if (_argumentMaps.CommandExists(key))
			{
				throw new ArgumentException($"Command with '{key}' has already been configured.", nameof(key));
			}

			if (_keyValidator.IsRestrictedKey(key))
			{
				throw new ArgumentException($"Command '{key}' is restricted.", nameof(key));
			}

			switch (type)
			{
				case CommandType.PropertyMapped:
					{
						if (propertyExpression == null)
						{
							throw new ArgumentException("Property expression must be provided.");
						}
						if (!_reflectionHelper.PropertyIsWritable(propertyExpression, out string propertyName))
						{
							throw new ArgumentException($"Property '{propertyName}' is not writable.", nameof(propertyExpression));
						}
					}
					break;
				case CommandType.CustomCallback:
					if (callback == null)
					{
						throw new ArgumentException("Custom callback must be provided.");
					}
					break;
				case CommandType.MappedAndCallback:
					{
						if (propertyExpression == null || callback == null)
						{
							throw new ArgumentException("Property expression and custom callback must be provided.");
						}
						if (!_reflectionHelper.PropertyIsWritable(propertyExpression, out string propertyName))
						{
							throw new ArgumentException($"Property '{propertyName}' is not writable.", nameof(propertyExpression));
						}
					}
					break;
				default:
					throw new ArgumentOutOfRangeException($"'{type}' is not a valid command type.");
			}
		}

		public void ValidateOption(string fullOptionKey, Expression<Func<TRunInfo, bool>> propertyExpression,
			char? shortOptionKey = null)
		{
			if (string.IsNullOrWhiteSpace(fullOptionKey))
			{
				throw new ArgumentNullException(nameof(fullOptionKey), "Full option key must be provided.");
			}

			if (_argumentMaps.FullOptionExists(fullOptionKey))
			{
				throw new ArgumentException($"Full option with key '{fullOptionKey}' has already been configured.", 
					nameof(fullOptionKey));
			}

			if (_keyValidator.IsRestrictedKey(fullOptionKey))
			{
				throw new ArgumentException($"Full option key '{fullOptionKey}' is restricted.", 
					nameof(fullOptionKey));
			}

			if (propertyExpression == null)
			{
				throw new ArgumentNullException(nameof(propertyExpression),
					"A property expression associated to the option must be provided.");
			}

			if (shortOptionKey.HasValue)
			{
				if (_argumentMaps.ShortOptionExists(shortOptionKey.Value))
				{
					throw new ArgumentException($"Short option '{shortOptionKey.Value}' has already been configured.",
						nameof(shortOptionKey));
				}
				if (_keyValidator.IsRestrictedKey(shortOptionKey.Value))
				{
					throw new ArgumentException($"Short option key '{shortOptionKey.Value}' is restricted.",
						nameof(shortOptionKey));
				}
			}

			if (!_reflectionHelper.PropertyIsWritable(propertyExpression, out string propertyName))
			{
				throw new ArgumentException($"Property '{propertyName}' is not writable.", nameof(propertyExpression));
			}
		}
	}
}
