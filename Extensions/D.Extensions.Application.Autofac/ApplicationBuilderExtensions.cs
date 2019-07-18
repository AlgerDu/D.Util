using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using D.Infrastructures.Application;

namespace D.Infrastructures
{
    /// <summary>
    /// IApplicationBuilder 的一些扩展方法
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 使用 Startup 类，可以配合使用 autofac
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStartupWithAutofac<T>(
            this IApplicationBuilder applicationBuilder
            )
            where T : class
        {
            applicationBuilder.ConfigureServices(collection =>
            {
                collection.AddTransient<T>();

                var provider = collection.BuildServiceProvider();

                var startupInstance = provider.GetService<T>();



                typeof(T).InvokeMember(
                    "ConfigServices"
                    , System.Reflection.BindingFlags.InvokeMethod
                    , null
                    , startupInstance
                    , new object[] { collection }
                    );
            });

            return applicationBuilder;
        }
    }
}
