using D.Utils.Extensions.Logging.RollingFile;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.Utils
{
    /// <summary>
    /// 对 ILoggerFactory 有关 RollingFileLogger 的扩展
    /// </summary>
    public static class RollingFileLoggerExtensions
    {
        /// <summary>
        /// 测试用
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        [Obsolete]
        public static ILoggerFactory AddRollingFile(this ILoggerFactory factory)
        {
            //factory.AddProvider(new RollingFileProvider());
            return factory;
        }

        /// <summary>
        /// 通过配置文件配置 rolling file provider
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ILoggerFactory AddRollingFile(this ILoggerFactory factory, IConfiguration configuration)
        {
            //ConfigurationRollingFileSettings settings = new ConfigurationRollingFileSettings();

            var settings = configuration.Get<ConfigurationRollingFileSettings>();

            factory.AddProvider(new RollingFileProvider(settings));
            return factory;
        }

        /// <summary>
        /// 添加 rolling file provider
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="path">日志输出路径</param>
        /// <param name="maxFileSize">日志文件的最大大小</param>
        /// <returns></returns>
        public static ILoggerFactory AddRollingFile(this ILoggerFactory factory, string path, string maxFileSize = "4MB")
        {
            var settings = new ConfigurationRollingFileSettings
            {
                Path = path,
                MaxFileSize = maxFileSize
            };

            factory.AddProvider(new RollingFileProvider(settings));
            return factory;
        }

        /// <summary>
        /// 添加 rolling file provider
        /// 读取配置，暂时有些疑问
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddRollingFile(this ILoggingBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddSingleton<ILoggerProvider, RollingFileProvider>();
            builder.Services.Configure<ConfigurationRollingFileSettings>(configuration);

            return builder;
        }

        /// <summary>
        /// 添加 rolling file provider
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddRollingFile(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, RollingFileProvider>();

            return builder;
        }
    }
}
