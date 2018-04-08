using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.Utils.Extensions.Logging.RollingFile
{
    internal class RollingFileLogger : ILogger
    {
        private readonly RollingFileLoggerProcessor _processor;
        private readonly string _category;
        private readonly LogLevel _miniLevel;

        public RollingFileLogger(
            RollingFileLoggerProcessor processor
            , LogLevel miniLevel
            , string category
            )
        {
            _processor = processor;
            _miniLevel = miniLevel;
            _category = category;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _miniLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
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

                _processor.AddLogContent(content);
            }
        }
    }
}
