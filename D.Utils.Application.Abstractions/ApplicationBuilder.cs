using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace D.Utils
{
    /// <summary>
    /// 通用 builder
    /// </summary>
    public class ApplicationBuilder : IApplicationBuilder
    {
        ServiceCollection _services;

        Action<IConfigurationBuilder> _configureDelegate;
        Action<IConfiguration, ILoggingBuilder> _configureLogging;

        IStartup _startup;

        /// <summary>
        /// .net core 依赖注入容器
        /// </summary>
        public IServiceCollection Services => _services;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ApplicationBuilder()
        {
            _services = new ServiceCollection();
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="configureDelegate"></param>
        /// <returns></returns>
        public IApplicationBuilder ConfigureAppConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            _configureDelegate = configureDelegate;

            return this;
        }

        /// <summary>
        /// 配置日志
        /// </summary>
        /// <param name="configureLogging"></param>
        /// <returns></returns>
        public IApplicationBuilder ConfigureLogging(Action<IConfiguration, ILoggingBuilder> configureLogging)
        {
            _configureLogging = configureLogging;

            return this;
        }

        /// <summary>
        /// 使用 Startup
        /// </summary>
        /// <typeparam name="Startup"></typeparam>
        /// <returns></returns>
        public IApplicationBuilder Use<Startup>() where Startup : IStartup, new()
        {
            _startup = new Startup();

            return this;
        }

        /// <summary>
        /// 创建 App
        /// </summary>
        /// <typeparam name="App"></typeparam>
        /// <returns></returns>
        public App Builde<App>() where App : class, IApplication
        {
            var configurationBuilder = new ConfigurationBuilder();

            _configureDelegate(configurationBuilder);

            _startup.Configuration = configurationBuilder.Build();

            _configureLogging(
                _startup.Configuration
                , new DLoggingBuilder { Services = _services }
                );

            _services.AddLogging();

            _services.AddSingleton<IApplication, App>();

            _startup.ConfigService(_services);

            var provider = _services.BuildServiceProvider();

            return provider.GetService<IApplication>() as App;
        }
    }
}
