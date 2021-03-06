﻿using Microsoft.Extensions.Logging;
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

        //public RollingFileProvider()
        //{
        //    _processor = new RollingFileLoggerProcessor(@"log/{Date}/test.log", 100);
        //}

        public RollingFileProvider(ConfigurationRollingFileSettings settings)
        {
            _settings = settings;

            if (_settings == null) throw new Exception("D.RollingFileProvider null settings");

            if (string.IsNullOrEmpty(_settings.Path)) throw new Exception("D.RollingFileProvider null settings with 'Path'");

            _processor = new RollingFileLoggerProcessor(_settings.Path, _settings.GetMaxFileSize());
        }

        public RollingFileProvider(IOptionsMonitor<ConfigurationRollingFileSettings> options)
            : this(options.CurrentValue)
        {
        }

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
            LogLevel level;
            if (!_settings.TryGetSwitch(name, out level))
            {
                level = LogLevel.Information;
            }

            return new RollingFileLogger(_processor, level, name);
        }
    }
}
