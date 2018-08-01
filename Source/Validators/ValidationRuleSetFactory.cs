//using R5.RunInfoBuilder.Configuration;
//using R5.RunInfoBuilder.Help;
//using R5.RunInfoBuilder.Store;
//using R5.RunInfoBuilder.Version;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace R5.RunInfoBuilder.Validators
//{
//	internal interface IValidationRuleSetFactory
//	{
//		ValidationRuleSet<string[]> NoDuplicateProgramArguments();

//		ValidationRuleSet<string[]> HelpArgumentMustBeFirst();

//		ValidationRuleSet<string[]> VersionArgumentMustBeFirst();

//		ValidationRuleSet<ProgramArgumentValidationInfo[]> AllProgramArgumentsHaveValidTypes();

//		ValidationRuleSet<ProgramArgumentValidationInfo[]> CommandConfigurationRules();

//		ValidationRuleSet<ProgramArgumentValidationInfo[]> ArgumentConfigurationRules();

//		ValidationRuleSet<ProgramArgumentValidationInfo[]> OptionConfigurationRules();
//	}

//	internal class ValidationRuleSetFactory<TRunInfo> : IValidationRuleSetFactory
//		where TRunInfo : class
//	{
//		private IHelpManager<TRunInfo> _helpManager { get; }
//		private IVersionManager _versionManager { get; }
//		private IArgumentTypeResolver _argumentTypeResolver { get; }
//		private CommandConfig _commandConfig { get; }
//		private OptionConfig _optionConfig { get; }
//		private ArgumentConfig _argumentConfig { get; }

//		public ValidationRuleSetFactory(
//			IHelpManager<TRunInfo> helpManager,
//			IVersionManager versionManager,
//			IArgumentTypeResolver argumentTypeResolver,
//			CommandConfig commandConfig,
//			OptionConfig optionConfig,
//			ArgumentConfig argumentConfig)
//		{
//			_helpManager = helpManager;
//			_versionManager = versionManager;
//			_argumentTypeResolver = argumentTypeResolver;
//			_commandConfig = commandConfig;
//			_optionConfig = optionConfig;
//			_argumentConfig = argumentConfig;
//		}

//		public ValidationRuleSet<string[]> NoDuplicateProgramArguments()
//		{
//			Action<string[]> onAnyInvalidCallback = programArguments =>
//			{
//				throw new ProgramArgumentsValidationException(null, "Duplicate program argument tokens were found.");
//			};

//			return new ValidationRuleSet<string[]>(new NoDuplicateArgumentsRule(), onAnyInvalidCallback);
//		}

//		public ValidationRuleSet<string[]> HelpArgumentMustBeFirst()
//		{
//			Action<string[]> onAnyInvalidCallback = programArguments =>
//			{
//				throw new ProgramArgumentsValidationException(null, "Help command must be the only program argument.");
//			};

//			return new ValidationRuleSet<string[]>(new HelpOnlyArgumentRule<TRunInfo>(_helpManager), onAnyInvalidCallback);
//		}

//		public ValidationRuleSet<string[]> VersionArgumentMustBeFirst()
//		{
//			Action<string[]> onAnyInvalidCallback = programArguments =>
//			{
//				throw new ProgramArgumentsValidationException(null, "Version command must be the only program argument.");
//			};

//			return new ValidationRuleSet<string[]>(new VersionOnlyArgumentRule(_versionManager), onAnyInvalidCallback);
//		}

//		public ValidationRuleSet<ProgramArgumentValidationInfo[]> AllProgramArgumentsHaveValidTypes()
//		{
//			Action<ProgramArgumentValidationInfo[]> onAnyInvalidCallback = argumentValidationInfos =>
//			{
//				List<ProgramArgumentError> errors = argumentValidationInfos
//					.Where(i => i.HasError)
//					.Select(i => i.ToError())
//					.ToList();

//				throw new ProgramArgumentsValidationException(errors, "One or more program arguments have errors.");
//			};

//			return new ValidationRuleSet<ProgramArgumentValidationInfo[]>(new ValidArgumentTypeRule(_argumentTypeResolver), onAnyInvalidCallback);
//		}

//		public ValidationRuleSet<ProgramArgumentValidationInfo[]> CommandConfigurationRules()
//		{
//			Action<ProgramArgumentValidationInfo[]> onAnyInvalidCallback = argumentValidationInfos =>
//			{
//				List<ProgramArgumentError> errors = argumentValidationInfos
//					.Where(i => i.HasError)
//					.Select(i => i.ToError())
//					.ToList();

//				throw new ProgramArgumentsValidationException(errors, "One or more commands have errors.");
//			};

//			var rules = new List<ValidationRule<ProgramArgumentValidationInfo[]>>
//			{
//				new SingleCommandRule(_commandConfig),
//				new CommandsPositionedFrontRule(_commandConfig)
//			};

//			return new ValidationRuleSet<ProgramArgumentValidationInfo[]>(rules, onAnyInvalidCallback);
//		}

//		public ValidationRuleSet<ProgramArgumentValidationInfo[]> ArgumentConfigurationRules()
//		{
//			Action<ProgramArgumentValidationInfo[]> onAnyInvalidCallback = argumentValidationInfos =>
//			{
//				List<ProgramArgumentError> errors = argumentValidationInfos
//					.Where(i => i.HasError)
//					.Select(i => i.ToError())
//					.ToList();

//				throw new ProgramArgumentsValidationException(errors, "One or more arguments have errors.");
//			};
			
//			return new ValidationRuleSet<ProgramArgumentValidationInfo[]>(new ArgumentsAfterCommandsRule(_argumentConfig), onAnyInvalidCallback);
//		}

//		public ValidationRuleSet<ProgramArgumentValidationInfo[]> OptionConfigurationRules()
//		{
//			Action<ProgramArgumentValidationInfo[]> onAnyInvalidCallback = argumentValidationInfos =>
//			{
//				List<ProgramArgumentError> errors = argumentValidationInfos
//					.Where(i => i.HasError)
//					.Select(i => i.ToError())
//					.ToList();

//				throw new ProgramArgumentsValidationException(errors, "One or more options have errors.");
//			};
			
//			return new ValidationRuleSet<ProgramArgumentValidationInfo[]>(new OptionsAfterCommandRule(_optionConfig), onAnyInvalidCallback);
//		}
//	}
//}
