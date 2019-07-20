using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.Infrastructures
{
    /// <summary>
    /// App 构造器；
    /// 统一使用 DI，Microsoft.Extensions 等等一些基础组件
    /// </summary>
    public interface IApplicationBuilder
    {
        /// <summary>
        /// 配置 App 的配置相关的一些东西
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IApplicationBuilder ConfigureAppConfiguration(Action<ApplicationBuilderContext, IConfigurationBuilder> action);

        /// <summary>
        /// 给应用容器注入服务（可以被多次调用）
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IApplicationBuilder ConfigureServices(Action<ApplicationBuilderContext, IServiceCollection> configureServices);

        /// <summary>
        /// 给应用容器注入服务（可以被多次调用）
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IApplicationBuilder ConfigureServices(Action<IServiceCollection> configureServices);

        /// <summary>
        /// 配置生成 provider 的 func ，用来达到可以使用其它依赖注入容器的使用，暂时只能想到这样实现了
        /// </summary>
        /// <param name="createProvider"></param>
        /// <returns></returns>
        IApplicationBuilder ConfigureProviderFactory(Func<IServiceCollection, IServiceProvider> createProvider);

        /// <summary>
        /// 构造一个 App
        /// </summary>
        /// <typeparam name="App">想要构建的 App 的类型</typeparam>
        /// <returns></returns>
        App Builde<App>() where App : class, IApplication;
    }
}
