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
using R5.RunInfoBuilder.Process;

namespace R5.RunInfoBuilder
{
	public class RunInfoBuilder<TRunInfo>
		where TRunInfo : class
	{
		public IParser Parser { get; }
		public IArgumentStore<TRunInfo> Store { get; }

		private IBuildValidator _buildValidator { get; }
		private IHelpManager<TRunInfo> _helpManager { get; }
		private IProcessInvoker _processInvoker { get; }
		private RunInfo<TRunInfo> _runInfo { get; }
		private IVersionManager _versionManager { get; }
		private BuilderConfig _config { get; }
		private HooksConfig<TRunInfo> _hooksConfig { get; }

		private bool _helpEnabled => _helpManager != null;
		private bool _versionEnabled => _versionManager != null;

		internal RunInfoBuilder(
			IProcessInvoker processInvoker,
			IParser parser,
			IArgumentStore<TRunInfo> store,
			IBuildValidator buildValidator,
			IHelpManager<TRunInfo> helpManager,
			RunInfo<TRunInfo> runInfo,
			IVersionManager versionManager,
			BuilderConfig config,
			HooksConfig<TRunInfo> hooksConfig)
		{
			Parser = parser;
			Store = store;

			_processInvoker = processInvoker;
			_buildValidator = buildValidator;
			_helpManager = helpManager;
			_runInfo = runInfo;
			_versionManager = versionManager;
			_config = config;
			_hooksConfig = hooksConfig;

			Console.WriteLine("INITIALIZED FROM IOC");
		}

		public RunInfoBuilder()
		{
			Console.WriteLine("INITIALIZED FROM PUBLIC");

			var setup = new BuilderSetup<TRunInfo>();
			var services = setup.ResolveServices();

			RunInfoBuilderDependencies<TRunInfo> dependencies = services
				.BuildServiceProvider()
				.GetRunInfoBuilderDependencies<TRunInfo>();

			Parser = dependencies.Parser;
			Store = dependencies.Store;
			_processInvoker = dependencies.ProcessInvoker;
			_buildValidator = dependencies.BuildValidator;
			_helpManager = dependencies.HelpManager;
			_runInfo = dependencies.RunInfo;
			_versionManager = dependencies.VersionManager;
			_config = dependencies.Config;
			_hooksConfig = dependencies.HooksConfig;
		}

		public BuildResult<TRunInfo> Build(string[] args)
		{
			try
			{
				// pre build
				if (_hooksConfig.PreBuildCallback != null)
				{

				}

				if (args == null || !args.Any())
				{
					return BuildResult<TRunInfo>.NotProcessed();
				}

				if (_versionEnabled && args.Length == 1 && _versionManager.IsTrigger(args[0]))
				{
					_versionManager.InvokeCallback();
					return BuildResult<TRunInfo>.Version();
				}

				if (_helpEnabled && args.Length == 1 && _helpManager.IsTrigger(args[0]))
				{
					_helpManager.InvokeCallback();
					return BuildResult<TRunInfo>.Help();
				}

				_buildValidator.ValidateBuilderConfiguration();

				List<ProgramArgument> programArguments = _buildValidator.ValidateProgramArguments(args);

				_processInvoker.Start(programArguments);

				// post build

				return BuildResult<TRunInfo>.Success(_runInfo.Value);
			}
			catch (BuilderConfigurationValidationException ex)
			{
				if (_config.AlwaysReturnBuildResult)
				{
					return BuildResult<TRunInfo>.ConfigurationValidationFail(ex.Message, ex);
				}
				throw;
			}
			catch (ProgramArgumentsValidationException ex)
			{
				if (_config.AlwaysReturnBuildResult)
				{
					return BuildResult<TRunInfo>.ProgramArgumentsValidationFail(ex.Message, ex, ex.Errors);
				}
				throw;
			}
			catch (Exception ex)
			{
				if (_config.AlwaysReturnBuildResult)
				{
					return BuildResult<TRunInfo>.ProcessFail(ex.Message, ex);
				}
				throw;
			}
		}
	}
}
