using D.Utils.Extensions.Logging.RollingFile;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.Utils
{
    public static class RollingFileLoggerExtensions
    {
        public static ILoggerFactory AddConsole(this ILoggerFactory factory)
        {
            factory.AddProvider(new RollingFileProvider());
            return factory;
        }
    }
}
