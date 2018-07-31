using R5.RunInfoBuilder.ArgumentParser;
using R5.RunInfoBuilder.Store;
using R5.RunInfoBuilder.Help;
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
		private IProcessInvoker _processInvoker { get; }
		private RunInfo<TRunInfo> _runInfo { get; }
		private BuilderConfig _config { get; }
		private HooksConfig<TRunInfo> _hooksConfig { get; }
		
		internal RunInfoBuilder(
			IProcessInvoker processInvoker,
			IParser parser,
			IArgumentStore<TRunInfo> store,
			IBuildValidator buildValidator,
			RunInfo<TRunInfo> runInfo,
			BuilderConfig config,
			HooksConfig<TRunInfo> hooksConfig)
		{
			Parser = parser;
			Store = store;

			_processInvoker = processInvoker;
			_buildValidator = buildValidator;
			_runInfo = runInfo;
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
			_runInfo = dependencies.RunInfo;
			_config = dependencies.Config;
			_hooksConfig = dependencies.HooksConfig;
		}

		public BuildResult<TRunInfo> Build(string[] args)
		{
			try
			{
				InvokePreBuild(args);

				if (args == null || !args.Any())
				{
					return BuildResult<TRunInfo>.NotProcessed();
				}

				//_buildValidator.ValidateBuilderConfiguration();

				//List<ProgramArgument> programArguments = _buildValidator.ValidateProgramArguments(args);

				ProcessResult processResult = _processInvoker.Start(args);

				InvokePostBuild(args);

				return CompleteFromResult(processResult);
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

		private BuildContext<TRunInfo> GetBuildContext(string[] args)
			=> new BuildContext<TRunInfo>((string[]) args.Clone(), _runInfo.Value);

		private void InvokePreBuild(string[] args)
		{
			if (_hooksConfig.PreBuildCallback != null)
			{
				BuildContext<TRunInfo> buildContext = GetBuildContext(args);
				_hooksConfig.PreBuildCallback(buildContext);
			}
		}

		private void InvokePostBuild(string[] args)
		{
			if (_hooksConfig.PostBuildCallback != null)
			{
				BuildContext<TRunInfo> buildContext = GetBuildContext(args);
				_hooksConfig.PostBuildCallback(buildContext);
			}
		}

		private BuildResult<TRunInfo> CompleteFromResult(ProcessResult processResult)
		{
			if (processResult == ProcessResult.Help)
			{
				return BuildResult<TRunInfo>.Help();
			}
			if (processResult == ProcessResult.Version)
			{
				return BuildResult<TRunInfo>.Version();
			}

			return BuildResult<TRunInfo>.Success(_runInfo.Value);
		}
	}
}
