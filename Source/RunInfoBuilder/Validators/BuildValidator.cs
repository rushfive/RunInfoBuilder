﻿using R5.RunInfoBuilder.ArgumentParser;
using R5.RunInfoBuilder.Configuration;
using R5.RunInfoBuilder.Store;
using R5.RunInfoBuilder.Validators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace R5.RunInfoBuilder.Validators
{
	internal interface IBuildValidator
	{
		void ValidateBuilderConfiguration();

		List<ProgramArgument> ValidateProgramArguments(string[] programArguments);
	}

	internal class BuildValidator<TRunInfo> : IBuildValidator
		where TRunInfo : class
	{
		//private IValidationRuleSetFactory _validationFactory { get; }
		private IArgumentMetadata<TRunInfo> _argumentMaps { get; }
		private IParser _parser { get; }
		private ProcessConfig _processConfig { get; }
		private IArgumentTypeResolver _argumentTypeResolver { get; }

		public BuildValidator(
			//IValidationRuleSetFactory validationFactory,
			IArgumentMetadata<TRunInfo> argumentMaps,
			IParser parser,
			ProcessConfig processConfig,
			IArgumentTypeResolver argumentTypeResolver)
		{
			//_validationFactory = validationFactory;
			_argumentMaps = argumentMaps;
			_parser = parser;
			_processConfig = processConfig;
			_argumentTypeResolver = argumentTypeResolver;
		}

		public void ValidateBuilderConfiguration()
		{
			ValidateParserHandlesConfiguredTypes();
		}

		private void ValidateParserHandlesConfiguredTypes()
		{
			IEnumerable<Type> argumentPropertyTypes = _argumentMaps.GetArguments()
				.Select(a => a.PropertyInfo.PropertyType)
				.Distinct();

			var unhandledTypes = new List<Type>();

			foreach (Type type in argumentPropertyTypes)
			{
				if (!_parser.HandlesType(type))
				{
					unhandledTypes.Add(type);
				}
			}

			if (unhandledTypes.Any())
			{
				string unhandledTypesList = string.Join(", ", unhandledTypes);
				string error = $"The following types are configured as arguments not handled by the parser: {unhandledTypesList}";

				throw new BuilderConfigurationValidationException(error);
			}
		}

		public List<ProgramArgument> ValidateProgramArguments(string[] programArguments)
		{
			ValidateRawProgramArguments(programArguments);

			ProgramArgumentValidationInfo[] argumentInfos = InitializeArgumentInfosFrom(programArguments);

			ValidateFromArgumentInfos(argumentInfos);
			
			return argumentInfos
				.Select(i => new ProgramArgument(i.Position, i.RawArgumentToken, i.Type))
				.ToList();
		}

		private void ValidateRawProgramArguments(string[] programArguments)
		{
			//_validationFactory
			//	.NoDuplicateProgramArguments()
			//	.Validate(programArguments);

			//_validationFactory
			//	.HelpArgumentMustBeFirst()
			//	.Validate(programArguments);

			//_validationFactory
			//	.VersionArgumentMustBeFirst()
			//	.Validate(programArguments);
		}

		private ProgramArgumentValidationInfo[] InitializeArgumentInfosFrom(string[] programArguments)
		{
			var validationInfos = new ProgramArgumentValidationInfo[programArguments.Length];

			for (int i = 0; i < programArguments.Length; i++)
			{
				validationInfos[i] = new ProgramArgumentValidationInfo(i, programArguments[i]);

				//if (_argumentTypeResolver.GetArgumentType(programArguments[i], out ProgramArgumentType type))
				//{
				//	validationInfos[i].SetType(type);
				//}
			}

			return validationInfos;
		}

		private void ValidateFromArgumentInfos(ProgramArgumentValidationInfo[] argumentInfos)
		{
			if (_processConfig.HandleUnresolved == HandleUnresolvedArgument.NotAllowed)
			{
				//_validationFactory
				//	.AllProgramArgumentsHaveValidTypes()
				//	.Validate(argumentInfos);
			}

			ValidationByConfigurationRules(argumentInfos);
		}

		private void ValidationByConfigurationRules(ProgramArgumentValidationInfo[] argumentInfos)
		{
			//_validationFactory
			//	.CommandConfigurationRules()
			//	.Validate(argumentInfos);

			//_validationFactory
			//	.ArgumentConfigurationRules()
			//	.Validate(argumentInfos);

			//_validationFactory
			//	.OptionConfigurationRules()
			//	.Validate(argumentInfos);
		}
	}
}
