using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace D.Utils.Extensions.Logging.RollingFile
{
    /// <summary>
    /// 基类 loggerProvider，封装一些批量的操作
    /// </summary>
    [ProviderAlias("RollingFile")]
    public class RollingFileProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, RollingFileLogger> _loggers = new ConcurrentDictionary<string, RollingFileLogger>();

        private ConfigurationRollingFileSettings _settings;

        private readonly RollingFileLoggerProcessor _processor;

        #region ILoggerProvider
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        public void Dispose()
        {
            _processor?.Dispose();
        }
        #endregion

        private RollingFileLogger CreateLoggerImplementation(string name)
        {
            return new RollingFileLogger();
        }
    }
}
