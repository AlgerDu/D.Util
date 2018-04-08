using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.Utils.Extensions.Logging.RollingFile
{
    /// <summary>
    /// RollingFile 配置
    /// </summary>
    public class ConfigurationRollingFileSettings
    {
        public Dictionary<string, string> LogLevel { get; set; }

        public string Path { get; set; }

        public string MaxFileSize { get; set; }

        public ConfigurationRollingFileSettings()
        {
        }

        public bool TryGetSwitch(string name, out LogLevel level)
        {
            if (LogLevel == null || LogLevel.Count == 0)
            {
                level = Microsoft.Extensions.Logging.LogLevel.None;
                return false;
            }

            string levelStr;
            if (!LogLevel.TryGetValue(name, out levelStr) || string.IsNullOrEmpty(levelStr))
            {
                if (!LogLevel.TryGetValue("Default", out levelStr) || string.IsNullOrEmpty(levelStr))
                {
                    level = Microsoft.Extensions.Logging.LogLevel.None;
                    return false;
                }
            }
            
            if (Enum.TryParse<LogLevel>(levelStr, true, out level))
            {
                return true;
            }

            throw new InvalidOperationException($"Configuration value '{levelStr}' for category '{name}' is not supported.");
        }

        public long GetMaxFileSize()
        {
            return 100000;
        }
    }
}
