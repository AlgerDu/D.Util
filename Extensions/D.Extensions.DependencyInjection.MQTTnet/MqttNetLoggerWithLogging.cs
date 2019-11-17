using Microsoft.Extensions.Logging;
using MQTTnet.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.Extensions.DependencyInjection
{
    internal class MqttNetLoggerWithLogging : IMqttNetLogger
    {
        public event EventHandler<MqttNetLogMessagePublishedEventArgs> LogMessagePublished;

        ILoggerFactory _loggerFactory;

        public MqttNetLoggerWithLogging(
            ILoggerFactory loggerFactory
            )
        {
            _loggerFactory = loggerFactory;
        }

        public IMqttNetChildLogger CreateChildLogger(string source = null)
        {
            return new MqttNetChildLoggerWithLogging(_loggerFactory, source);
        }

        public void Publish(MqttNetLogLevel logLevel, string source, string message, object[] parameters, Exception exception)
        {
            var hasLocalListeners = LogMessagePublished != null;

            if (!hasLocalListeners)
            {
                return;
            }

            if (parameters?.Length > 0)
            {
                try
                {
                    message = string.Format(message, parameters);
                }
                catch
                {
                    message = "MESSAGE FORMAT INVALID: " + message;
                }
            }

            var traceMessage = new MqttNetLogMessage(string.Empty, DateTime.UtcNow, Environment.CurrentManagedThreadId, source, logLevel, message, exception);

            if (hasLocalListeners)
            {
                LogMessagePublished?.Invoke(this, new MqttNetLogMessagePublishedEventArgs(traceMessage));
            }
        }
    }

    internal class MqttNetChildLoggerWithLogging : IMqttNetChildLogger
    {
        ILoggerFactory _loggerFactory;
        ILogger _logger;

        public MqttNetChildLoggerWithLogging(
            ILoggerFactory loggerFactory
            , string source = null
            )
        {
            _loggerFactory = loggerFactory;

            source = source == null ? "MQTTnetDefaultLogger" : source;

            _logger = loggerFactory.CreateLogger(source);
        }

        public IMqttNetChildLogger CreateChildLogger(string source = null)
        {
            return new MqttNetChildLoggerWithLogging(_loggerFactory, source);
        }

        public void Error(Exception exception, string message, params object[] parameters)
        {
            _logger.LogError($"{string.Format(message, parameters)}{exception}");
        }

        public void Info(string message, params object[] parameters)
        {
            _logger.LogInformation(string.Format(message, parameters));
        }

        public void Verbose(string message, params object[] parameters)
        {
            _logger.LogDebug(string.Format(message, parameters));
        }

        public void Warning(Exception exception, string message, params object[] parameters)
        {
            _logger.LogWarning($"{string.Format(message, parameters)}{exception}");
        }
    }
}
