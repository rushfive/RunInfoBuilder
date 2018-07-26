using Microsoft.Extensions.DependencyInjection;
using R5.RunInfoBuilder.ArgumentParser;
using R5.RunInfoBuilder.Store;
using R5.RunInfoBuilder.Help;
using R5.RunInfoBuilder.Pipeline;
using R5.RunInfoBuilder.Validators;
using R5.RunInfoBuilder.Version;
using System;

namespace R5.RunInfoBuilder.Configuration
{
	public class BuilderSetup<TRunInfo>
		where TRunInfo : class
	{
		public CommandConfigBuilder Commands { get; }
		public OptionConfigBuilder Options { get; }
		public ArgumentConfigBuilder Arguments { get; }
		public ParserConfigBuilder Parser { get; }
		public ProcessConfigBuilder<TRunInfo> Process { get; }

		private TRunInfo _implementation { get; set; }
		private bool _alwaysReturnBuildResult { get; set; }

		private HelpConfigBuilder<TRunInfo> _helpBuilder { get; set; }
		private VersionConfigBuilder _versionBuilder { get; set; }

		public BuilderSetup()
		{
			Commands = new CommandConfigBuilder();
			Options = new OptionConfigBuilder();
			Arguments = new ArgumentConfigBuilder();
			Parser = new ParserConfigBuilder();
			Process = new ProcessConfigBuilder<TRunInfo>();

			_implementation = null;
			_alwaysReturnBuildResult = false;

			// these two are configuration through method calls because they're optional
			_helpBuilder = null;
			_versionBuilder = null;
		}

		public BuilderSetup<TRunInfo> UseImplementation(TRunInfo implementation)
		{
			if (_implementation != null)
			{
				throw new InvalidOperationException("Run info implementation has already been set.");
			}

			_implementation = implementation ?? throw new ArgumentNullException(nameof(implementation), "Run info implementation must be provided.");
			return this;
		}

		public BuilderSetup<TRunInfo> AlwaysReturnBuildResult()
		{
			_alwaysReturnBuildResult = true;
			return this;
		}

		public BuilderSetup<TRunInfo> ConfigureHelp(Action<HelpConfigBuilder<TRunInfo>> config)
		{
			if (_helpBuilder != null)
			{
				throw new InvalidOperationException("Help configuration can only be setup once.");
			}
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config), "Help manager configuration must be provided.");
			}

			_helpBuilder = new HelpConfigBuilder<TRunInfo>();

			config(_helpBuilder);

			return this;
		}

		public BuilderSetup<TRunInfo> ConfigureVersion(Action<VersionConfigBuilder> config)
		{
			if (_versionBuilder != null)
			{
				throw new InvalidOperationException("Version configuration can only be setup once.");
			}
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config), "Version manager configuration must be provided.");
			}

			_versionBuilder = new VersionConfigBuilder();

			config(_versionBuilder);

			return this;
		}

		public RunInfoBuilder<TRunInfo> Create()
		{
			return ResolveServices()
				.BuildServiceProvider()
				.GetRequiredService<RunInfoBuilder<TRunInfo>>();
		}

		// maybe make this return serviceProvider?
		// maybe ONLY add IRunInfoBuilder if via ioc/DI
		// TODO: refactor this to make more readable (split service adds into better groups?)
		internal IServiceCollection ResolveServices()
		{
			IServiceCollection services = new ServiceCollection();

			AddConfigs(services);

			services
				.AddScoped<RunInfo<TRunInfo>>(sp =>
				{
					TRunInfo implementation = _implementation ?? (TRunInfo)Activator.CreateInstance(typeof(TRunInfo));
					return new RunInfo<TRunInfo>(implementation);
				})
				.AddScoped<IRestrictedKeyValidator, RestrictedKeyValidator>()
				.AddScoped<IReflectionHelper<TRunInfo>, ReflectionHelper<TRunInfo>>()
				.AddScoped<IArgumentMetadataMaps<TRunInfo>, ArgumentMetadataMaps<TRunInfo>>()
				.AddScoped<IArgumentStore<TRunInfo>, ArgumentStore<TRunInfo>>()
				.AddScoped<IArgumentTypeResolver, ArgumentTypeResolver<TRunInfo>>()
				.AddScoped<IValidationRuleSetFactory, ValidationRuleSetFactory<TRunInfo>>()
				.AddScoped<IValidationHelper, ValidationHelper<TRunInfo>>()
				.AddScoped<IArgumentStoreValidator<TRunInfo>, ArgumentStoreValidator<TRunInfo>>()
				.AddScoped<IBuildValidator, BuildValidator<TRunInfo>>()
				.AddScoped<IArgumentTokenizer, ArgumentTokenizer>()
				.AddConfigurableServices<TRunInfo>(_helpBuilder, _versionBuilder)
				.AddScoped<RunInfoBuilder<TRunInfo>>(sp =>
				{
					RunInfoBuilderDependencies<TRunInfo> dependencies = sp.GetRunInfoBuilderDependencies<TRunInfo>();

					return new RunInfoBuilder<TRunInfo>(
						dependencies.Parser,
						dependencies.Pipeline,
						dependencies.Store,
						dependencies.BuildValidator,
						dependencies.HelpManager,
						dependencies.RunInfo,
						dependencies.VersionManager,
						dependencies.Config);
				});

			return services;
		}

		private IServiceCollection AddConfigs(IServiceCollection services)
		{
			(ProcessConfig processConfig, ProcessHooksConfig<TRunInfo> processHooks) = Process.Build();

			return services
				.AddScoped<CommandConfig>(sp => Commands.Build())
				.AddScoped<OptionConfig>(sp => Options.Build())
				.AddScoped<ArgumentConfig>(sp => Arguments.Build())
				.AddScoped<ProcessConfig>(sp => processConfig)
				.AddScoped<ProcessHooksConfig<TRunInfo>>(sp => processHooks)
				.AddScoped<BuilderConfig>(sp => BuildConfig())
				.AddScoped<ParserConfig>(sp => Parser.Build());
		}

		private BuilderConfig BuildConfig()
		{
			return new BuilderConfig(_alwaysReturnBuildResult);
		}
	}
	
	internal static class BuilderSetupExtensions
	{
		public static IServiceCollection AddConfigurableServices<TRunInfo>(this IServiceCollection services,
			HelpConfigBuilder<TRunInfo> helpBuilder, VersionConfigBuilder versionBuilder)
			where TRunInfo : class
		{
			return services
				.AddHelpManager(helpBuilder)
				.AddVersionManager(versionBuilder)
				.AddScoped<IPipelineProcessor<TRunInfo>, PipelineProcessor<TRunInfo>>()
				.AddScoped<IParser, Parser>();
		}

		private static IServiceCollection AddHelpManager<TRunInfo>(this IServiceCollection services,
			HelpConfigBuilder<TRunInfo> helpBuilder)
			where TRunInfo : class
		{
			if (helpBuilder == null)
			{
				return services.AddScoped<IHelpManager<TRunInfo>>(sp => null);
			}

			return services
				.AddScoped<HelpConfig<TRunInfo>>(sp => helpBuilder.Build())
				.AddScoped<IHelpManager<TRunInfo>, HelpManager<TRunInfo>>();
		}

		private static IServiceCollection AddVersionManager(this IServiceCollection services,
			VersionConfigBuilder versionBuilder)
		{
			if (versionBuilder == null)
			{
				return services.AddScoped<IVersionManager>(sp => null);
			}

			return services
				.AddScoped<VersionConfig>(sp => versionBuilder.Build())
				.AddScoped<IVersionManager, VersionManager>();
		}

		public static RunInfoBuilderDependencies<TRunInfo> GetRunInfoBuilderDependencies<TRunInfo>(this IServiceProvider provider)
			where TRunInfo : class
		{
			var parser = provider.GetRequiredService<IParser>();
			var pipeline = provider.GetRequiredService<IPipelineProcessor<TRunInfo>>();
			var store = provider.GetRequiredService<IArgumentStore<TRunInfo>>();
			var buildValidator = provider.GetRequiredService<IBuildValidator>();
			var helpManager = provider.GetService<IHelpManager<TRunInfo>>();
			var runInfoValue = provider.GetRequiredService<RunInfo<TRunInfo>>();
			var versionManager = provider.GetService<IVersionManager>();
			var config = provider.GetService<BuilderConfig>();

			return new RunInfoBuilderDependencies<TRunInfo>(
				parser,
				pipeline,
				store,
				buildValidator,
				helpManager,
				runInfoValue,
				versionManager,
				config);
		}
	}
}
