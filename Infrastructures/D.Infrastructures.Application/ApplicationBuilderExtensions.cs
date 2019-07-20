using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace D.Infrastructures
{
    /// <summary>
    /// IApplicationBuilder 的一些扩展方法
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 配置使用的日志组件
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="configureLogging"></param>
        /// <returns></returns>
        public static IApplicationBuilder ConfigureLogging(
            this IApplicationBuilder applicationBuilder
            , Action<ILoggingBuilder> configureLogging)
        {
            return applicationBuilder.ConfigureServices(collection => collection.AddLogging(configureLogging));
        }

        /// <summary>
        /// 配置使用的日志组件
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="configureLogging"></param>
        /// <returns></returns>
        public static IApplicationBuilder ConfigureLogging(
            this IApplicationBuilder applicationBuilder
            , Action<ApplicationBuilderContext, ILoggingBuilder> configureLogging)
        {
            applicationBuilder.ConfigureServices((context, collection) =>
            {
                collection.AddLogging(builder =>
                {
                    configureLogging(context, builder);
                });
            });

            return applicationBuilder;
        }

        //public static IApplicationBuilder SetTestEnvironment(this IApplicationBuilder )

        /// <summary>
        /// 想来想去还是有点问题，先这样写着吧
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStartup<T>(
            this IApplicationBuilder applicationBuilder
            )
            where T : class
        {
            applicationBuilder.ConfigureServices(collection =>
            {
                collection.AddTransient<T>();

                var provider = collection.BuildServiceProvider();

                var startupInstance = provider.GetService<T>();

                var startupType = typeof(T);

                var members = from m in startupType.GetMethods()
                              where m.IsPublic && !m.IsStatic
                                 && m.GetParameters().Length == 1
                                 && m.GetParameters()[0].ParameterType.IsAssignableFrom(collection.GetType())
                              select m;

                foreach (var m in members)
                {
                    m.Invoke(startupInstance, new object[] { collection });
                }
            });

            return applicationBuilder;
        }
    }
}
