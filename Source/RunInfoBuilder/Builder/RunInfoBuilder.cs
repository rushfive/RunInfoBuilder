using R5.RunInfoBuilder.ArgumentParser;
using R5.RunInfoBuilder.Store;
using R5.RunInfoBuilder.Help;
using R5.RunInfoBuilder.Pipeline;
using R5.RunInfoBuilder.Version;
using System;
using System.Linq;
using R5.RunInfoBuilder.Validators;
using Microsoft.Extensions.DependencyInjection;
using R5.RunInfoBuilder.Configuration;
using System.Collections.Generic;

namespace R5.RunInfoBuilder
{
	public class RunInfoBuilder<TRunInfo>
		where TRunInfo : class
	{
		public IParser Parser { get; }
		public IArgumentStore<TRunInfo> Store { get; }

		private IBuildValidator _buildValidator { get; }
		private IHelpManager<TRunInfo> _helpManager { get; }
		private IPipelineProcessor<TRunInfo> _pipelineProcessor { get; }
		private RunInfo<TRunInfo> _runInfo { get; }
		private IVersionManager _versionManager { get; }

		private bool _helpEnabled => _helpManager != null;
		private bool _versionEnabled => _versionManager != null;

		internal RunInfoBuilder(
			IParser parser,
			IPipelineProcessor<TRunInfo> pipeline,
			IArgumentStore<TRunInfo> store,
			IBuildValidator buildValidator,
			IHelpManager<TRunInfo> helpManager,
			RunInfo<TRunInfo> runInfo,
			IVersionManager versionManager)
		{
			Parser = parser;
			Store = store;

			_pipelineProcessor = pipeline;
			_buildValidator = buildValidator;
			_helpManager = helpManager;
			_runInfo = runInfo;
			_versionManager = versionManager;

			Console.WriteLine("INITIALIZED FROM IOC");
		}

		public RunInfoBuilder()
		{
			Console.WriteLine("INITIALIZED FROM PUBLIC");

			var setup = new BuilderSetup<TRunInfo>();
			var services = setup.ResolveServices();

			RunInfoBuilderDependencies<TRunInfo> dependencies = services
				.BuildServiceProvider()
				.GetBuilderDependencies<TRunInfo>();

			Parser = dependencies.Parser;
			Store = dependencies.Store;
			_pipelineProcessor = dependencies.Pipeline;
			_buildValidator = dependencies.BuildValidator;
			_helpManager = dependencies.HelpManager;
			_runInfo = dependencies.RunInfo;
			_versionManager = dependencies.VersionManager;
		}

		public BuildResult<TRunInfo> Build(string[] programArguments)
		{
			try
			{
				if (programArguments == null || !programArguments.Any())
				{
					return BuildResult<TRunInfo>.NotProcessed();
				}

				if (_versionEnabled && programArguments.Length == 1 && _versionManager.IsTrigger(programArguments[0]))
				{
					_versionManager.InvokeCallback();
					return BuildResult<TRunInfo>.Version();
				}

				if (_helpEnabled && programArguments.Length == 1 && _helpManager.IsTrigger(programArguments[0]))
				{
					_helpManager.InvokeCallback();
					return BuildResult<TRunInfo>.Help();
				}

				_buildValidator.ValidateBuilderConfiguration();

				List<ProgramArgumentInfo> programArgumentInfos = _buildValidator.ValidateProgramArguments(programArguments);

				_pipelineProcessor.ProcessArgs(programArgumentInfos);

				return BuildResult<TRunInfo>.Success(_runInfo.Value);
			}
			catch (BuilderConfigurationValidationException ex)
			{
				return BuildResult<TRunInfo>.ConfigurationValidationFail(ex.Message, ex);
			}
			catch (ProgramArgumentsValidationException ex)
			{
				return BuildResult<TRunInfo>.ProgramArgumentsValidationFail(ex.Message, ex, ex.Errors);
			}
			catch (Exception ex)
			{
				return BuildResult<TRunInfo>.ProcessFail(ex.Message, ex);
			}
		}
	}
}
