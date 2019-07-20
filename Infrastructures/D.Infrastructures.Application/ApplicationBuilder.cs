using System;
using System.Collections.Generic;
using System.Text;
using D.Infrastructures.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace D.Infrastructures
{
    /// <summary>
    /// IApplicationBuilder 接口的基本实现
    /// </summary>
    public class ApplicationBuilder : IApplicationBuilder
    {
        private readonly HostingEnvironment _hostingEnvironment;
        private Action<ApplicationBuilderContext, IServiceCollection> _configureServices;

        private IConfiguration _config;
        private ApplicationBuilderContext _context;
        private Action<ApplicationBuilderContext, IConfigurationBuilder> _configureAppConfigurationBuilder;
        private Func<IServiceCollection, IServiceProvider> _createProvider;

        /// <summary>
        /// 无参构造方法
        /// </summary>
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

        /// <summary>
        /// 设置 app 配置读取 action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IApplicationBuilder ConfigureAppConfiguration(Action<ApplicationBuilderContext, IConfigurationBuilder> action)
        {
            _configureAppConfigurationBuilder += action;
            return this;
        }

        /// <summary>
        /// 设置依赖注入的 action
        /// </summary>
        /// <param name="configureServices"></param>
        /// <returns></returns>
        public IApplicationBuilder ConfigureServices(Action<ApplicationBuilderContext, IServiceCollection> configureServices)
        {
            _configureServices += configureServices;
            return this;
        }

        /// <summary>
        /// 设置依赖注入的 action
        /// </summary>
        /// <param name="configureServices"></param>
        /// <returns></returns>
        public IApplicationBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            return ConfigureServices((_, services) => configureServices(services));
        }

        /// <summary>
        /// 构造 app
        /// </summary>
        /// <typeparam name="App"></typeparam>
        /// <returns></returns>
        App IApplicationBuilder.Builde<App>()
        {
            var hostingServices = BuildCommonServices(out var hostingStartupErrors);

            hostingServices.AddTransient<App>();

            //var provider = hostingServices.BuildServiceProvider();

            var provider = GetProviderFromFactory(hostingServices);

            return provider.GetService<App>();
        }

        /// <summary>
        /// 设置构造 provider 的函数
        /// </summary>
        /// <param name="createProvider"></param>
        /// <returns></returns>
        public IApplicationBuilder ConfigureProviderFactory(Func<IServiceCollection, IServiceProvider> createProvider)
        {
            _createProvider = createProvider;
            return this;
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

        private IServiceProvider GetProviderFromFactory(IServiceCollection collection)
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
