using Autofac;
using D.Util.Config;
using D.Util.Interface;
using D.Util.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D.Utils.AutofacExt
{
    /// <summary>
    /// 一些扩展函数
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 添加一些基本的注入
        /// </summary>
        /// <param name="builder"></param>
        public static void AddUtils(this ContainerBuilder builder)
        {
            builder.RegisterType<LoggerFactory>()
                .As<ILoggerFactory>();
        }

        /// <summary>
        /// 添加使用 ConsoleLogWriter
        /// </summary>
        /// <param name="builder"></param>
        public static void AddConsoleLogWriter(this ContainerBuilder builder)
        {
            builder.RegisterType<ConsoleLogWriter>()
                .As<ILogWriter>();
        }

        /// <summary>
        /// 添加使用 ConfigProvider
        /// </summary>
        /// <param name="builder"></param>
        public static void AddConfigProvider(this ContainerBuilder builder, IConfigProvider provider)
        {
            if (provider == null)
                builder.RegisterType<NullConfigProvider>()
                    .As<IConfigProvider>()
                    .SingleInstance();
            else
                builder.RegisterInstance(provider)
                    .As<IConfigProvider>()
                    .SingleInstance();
        }
    }
}
