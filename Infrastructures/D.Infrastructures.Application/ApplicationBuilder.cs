using System;
using System.Collections.Generic;
using System.Text;
using D.Infrastructures.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace D.Infrastructures
{
    public class ApplicationBuilder : IApplicationBuilder
    {
        private readonly HostingEnvironment _hostingEnvironment;
        private Action<ApplicationBuilderContext, IServiceCollection> _configureServices;

        private IConfiguration _config;
        private ApplicationBuilderContext _context;
        private Action<ApplicationBuilderContext, IConfigurationBuilder> _configureAppConfigurationBuilder;
        private Func<IServiceCollection, IServiceProvider> _createProvider;

        public ApplicationBuilder()
        {
            _hostingEnvironment = new HostingEnvironment()
            {
                ContentRootPath = AppContext.BaseDirectory,
                AppRootPath = AppContext.BaseDirectory
            };

            _config = new ConfigurationBuilder()
                .Build();

            _context = new ApplicationBuilderContext
            {
                Configuration = _config,
                Environment = _hostingEnvironment
            };
        }

        public IApplicationBuilder ConfigureAppConfiguration(Action<ApplicationBuilderContext, IConfigurationBuilder> action)
        {
            _configureAppConfigurationBuilder += action;
            return this;
        }

        public IApplicationBuilder ConfigureServices(Action<ApplicationBuilderContext, IServiceCollection> configureServices)
        {
            _configureServices += configureServices;
            return this;
        }

        public IApplicationBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            return ConfigureServices((_, services) => configureServices(services));
        }

        App IApplicationBuilder.Builde<App>()
        {
            var hostingServices = BuildCommonServices(out var hostingStartupErrors);

            hostingServices.AddTransient<App>();

            //var provider = hostingServices.BuildServiceProvider();

            var provider = GetProviderFromFactory(hostingServices);

            return provider.GetService<App>();
        }

        private IServiceCollection BuildCommonServices(out AggregateException hostingStartupErrors)
        {
            hostingStartupErrors = null;

            var services = new ServiceCollection();

            services.AddSingleton<IHostingEnvironment>(_hostingEnvironment);
            services.AddSingleton(_context);

            var builder = new ConfigurationBuilder()
                .SetBasePath(_hostingEnvironment.ContentRootPath)
                .AddConfiguration(_config);

            _configureAppConfigurationBuilder?.Invoke(_context, builder);

            var configuration = builder.Build();
            services.AddSingleton<IConfiguration>(_ => configuration);
            _context.Configuration = configuration;

            services.AddOptions();
            services.AddLogging();

            _configureServices?.Invoke(_context, services);

            return services;
        }

        public IApplicationBuilder ConfigureProviderFactory(Func<IServiceCollection, IServiceProvider> createProvider)
        {
            _createProvider = createProvider;
            return this;
        }

        //private string ResolveContentRootPath(string contentRootPath, string basePath)
        //{
        //    if (string.IsNullOrEmpty(contentRootPath))
        //    {
        //        return basePath;
        //    }
        //    if (Path.IsPathRooted(contentRootPath))
        //    {
        //        return contentRootPath;
        //    }
        //    return Path.Combine(Path.GetFullPath(basePath), contentRootPath);
        //}

        IServiceProvider GetProviderFromFactory(IServiceCollection collection)
        {
            var provider = collection.BuildServiceProvider();

            if (_createProvider != null)
            {
                return _createProvider(collection);
            }

            return provider;
        }
    }
}
