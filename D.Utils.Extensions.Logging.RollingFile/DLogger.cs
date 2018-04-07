using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.Utils.Extensions.Logging.RollingFile
{
    public class DLogger : ILogger
    {
        private readonly DBaseLoggerProvider _provider;
        private readonly string _category;

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _provider.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (_provider.IsEnabled(logLevel, _category))
            {
                var content = new LogContent
                {
                    LogLevel = logLevel,
                    EventId = eventId,
                    Timestamp = DateTimeOffset.Now,
                    Msg = formatter(state, exception),
                    Category = _category,
                    Ex = exception
                };

                _provider.AddLogContent(content);
            }
        }
    }
}
