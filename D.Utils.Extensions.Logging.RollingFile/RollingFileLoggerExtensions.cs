using D.Utils.Extensions.Logging.RollingFile;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.Utils
{
    public static class RollingFileLoggerExtensions
    {
        public static ILoggerFactory AddRollingFile(this ILoggerFactory factory)
        {
            factory.AddProvider(new RollingFileProvider());
            return factory;
        }

        public static ILoggerFactory AddRollingFile(this ILoggerFactory factory, IConfiguration configuration)
        {
            //ConfigurationRollingFileSettings settings = new ConfigurationRollingFileSettings();

            var settings = configuration.Get<ConfigurationRollingFileSettings>();

            factory.AddProvider(new RollingFileProvider(settings));
            return factory;
        }
    }
}
