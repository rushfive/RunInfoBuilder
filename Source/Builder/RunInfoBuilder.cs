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

		private IProcessInvoker _processInvoker { get; }
		private RunInfo<TRunInfo> _runInfo { get; }
		private BuilderConfig _config { get; }
		private HooksConfig<TRunInfo> _hooksConfig { get; }
		
		internal RunInfoBuilder(
			IProcessInvoker processInvoker,
			IParser parser,
			IArgumentStore<TRunInfo> store,
			RunInfo<TRunInfo> runInfo,
			BuilderConfig config,
			HooksConfig<TRunInfo> hooksConfig)
		{
			Parser = parser;
			Store = store;

			_processInvoker = processInvoker;
			_runInfo = runInfo;
			_config = config;
			_hooksConfig = hooksConfig;
		}

		public RunInfoBuilder()
		{
			var setup = new BuilderSetup<TRunInfo>();
			var services = setup.ResolveServices();

			RunInfoBuilderDependencies<TRunInfo> dependencies = services
				.BuildServiceProvider()
				.GetRunInfoBuilderDependencies<TRunInfo>();

			Parser = dependencies.Parser;
			Store = dependencies.Store;
			_processInvoker = dependencies.ProcessInvoker;
			_runInfo = dependencies.RunInfo;
			_config = dependencies.Config;
			_hooksConfig = dependencies.HooksConfig;
		}

		public BuildResult<TRunInfo> Build(string[] args)
		{
			try
			{
				TryInvokePreBuildInvoke(args);

				if (args == null || !args.Any())
				{
					return BuildResult<TRunInfo>.NotProcessed();
				}
				
				ProcessResult processResult = _processInvoker.Start(args);

				TryInvokePostBuildCallback(args);

				return CompleteFromResult(processResult);
			}
			catch (Exception ex)
			{
				if (_config.AlwaysReturnBuildResult)
				{
					return BuildResult<TRunInfo>.Fail(ex.Message, ex);
				}
				throw;
			}
		}

		private BuildContext<TRunInfo> GetBuildContext(string[] args)
			=> new BuildContext<TRunInfo>((string[]) args.Clone(), _runInfo.Value);

		private void TryInvokePreBuildInvoke(string[] args)
		{
			if (_hooksConfig.PreBuildCallback != null)
			{
				BuildContext<TRunInfo> buildContext = GetBuildContext(args);
				_hooksConfig.PreBuildCallback(buildContext);
			}
		}

		private void TryInvokePostBuildCallback(string[] args)
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
