using D.Utils.Extensions.Logging.RollingFile;
using Microsoft.Extensions.Configuration;
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
            factory.AddProvider(new RollingFileProvider());
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
    }
}
