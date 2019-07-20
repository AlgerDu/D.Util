using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using D.Infrastructures.Application;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;

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
            applicationBuilder
                .ConfigureProviderFactory((collection) =>
                {
                    var provider = collection.BuildServiceProvider();
                    var factory = provider.GetService<IServiceProviderFactory<ContainerBuilder>>();

                    if (factory != null && !(factory is DefaultServiceProviderFactory))
                    {
                        using (provider)
                        {
                            return factory.CreateServiceProvider(factory.CreateBuilder(collection));
                        }
                    }

                    return provider;
                })
                .ConfigureServices(collection =>
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

                    collection.AddSingleton<IServiceProviderFactory<ContainerBuilder>>(new AutofacServiceProviderFactory(builder =>
                    {
                        members = from m in startupType.GetMethods()
                                  where m.IsPublic && !m.IsStatic
                                     && m.GetParameters().Length == 1
                                     && m.GetParameters()[0].ParameterType.IsAssignableFrom(builder.GetType())
                                  select m;

                        foreach (var m in members)
                        {
                            m.Invoke(startupInstance, new object[] { builder });
                        }
                    }));
                });

            return applicationBuilder;
        }
    }
}
