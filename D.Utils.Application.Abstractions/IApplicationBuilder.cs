using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.Utils
{
    /// <summary>
    /// app 构建器
    /// </summary>
    public interface IApplicationBuilder
    {
        /// <summary>
        /// 使用 .net core 自带的容器
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="configureDelegate"></param>
        /// <returns></returns>
        IApplicationBuilder ConfigureAppConfiguration(Action<IConfigurationBuilder> configureDelegate);

        /// <summary>
        /// 配置日志
        /// </summary>
        /// <param name="configureLogging"></param>
        /// <returns></returns>
        IApplicationBuilder ConfigureLogging(Action<IConfiguration, ILoggingBuilder> configureLogging);

        /// <summary>
        /// 使用 Startup
        /// </summary>
        /// <typeparam name="Startup"></typeparam>
        /// <returns></returns>
        IApplicationBuilder Use<Startup>() where Startup : IStartup;

        /// <summary>
        /// 构建 app
        /// </summary>
        /// <typeparam name="App"></typeparam>
        /// <returns></returns>
        App Builde<App>() where App : IApplication;
    }
}
