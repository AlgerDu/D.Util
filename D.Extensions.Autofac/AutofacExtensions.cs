using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.Extensions.Autofac
{
    /// <summary>
    /// 类 微软 DI，写法，感觉方便很多
    /// </summary>
    public static class AutofacExtensions
    {
        /// <summary>
        /// 单例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="builder"></param>
        public static void AddSingleton<TService, TImplementation>(this ContainerBuilder builder)
            where TImplementation : TService
        {
            builder.RegisterType<TImplementation>()
                .As<TService>()
                .AsSelf()
                .SingleInstance();
        }

        /// <summary>
        /// 单例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="builder"></param>
        public static void AddSingleton<TImplementation>(this ContainerBuilder builder)
        {
            builder.RegisterType<TImplementation>()
                .AsSelf()
                .SingleInstance();
        }

        /// <summary>
        /// 单例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="builder"></param>
        public static void AddSingleton<TService, TImplementation>(
            this ContainerBuilder builder
            , string name
            )
            where TImplementation : TService
        {
            builder.RegisterType<TImplementation>()
                .Named<TService>(name)
                .AsSelf()
                .SingleInstance();
        }

        /// <summary>
        /// 单例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="builder"></param>
        public static void AddSingleton<TImplementation>(
            this ContainerBuilder builder
            , string name
            )
        {
            builder.RegisterType<TImplementation>()
                .Named<TImplementation>(name)
                .AsSelf()
                .SingleInstance();
        }

        /// <summary>
        /// 瞬时
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="builder"></param>
        public static void AddTransient<TService, TImplementation>(this ContainerBuilder builder)
            where TImplementation : TService
        {
            builder.RegisterType<TImplementation>()
                .As<TService>()
                .AsSelf();
        }

        /// <summary>
        /// 瞬时
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="builder"></param>
        public static void AddTransient<TImplementation>(this ContainerBuilder builder)
        {
            builder.RegisterType<TImplementation>()
                .AsSelf();
        }

        /// <summary>
        /// 瞬时
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="builder"></param>
        public static void AddTransient<TService, TImplementation>(
            this ContainerBuilder builder
            , string name
            )
            where TImplementation : TService
        {
            builder.RegisterType<TImplementation>()
                .Named<TService>(name)
                .AsSelf();
        }

        /// <summary>
        /// 瞬时
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="builder"></param>
        public static void AddTransient<TImplementation>(
            this ContainerBuilder builder
            , string name
            )
        {
            builder.RegisterType<TImplementation>()
                .Named<TImplementation>(name)
                .AsSelf();
        }
    }
}
