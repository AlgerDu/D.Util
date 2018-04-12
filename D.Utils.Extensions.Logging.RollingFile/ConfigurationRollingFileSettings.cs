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
                foreach (var category in LogLevel.Keys)
                {
                    if (name.StartsWith(category))
                    {
                        levelStr = LogLevel[category];
                        break;
                    }
                }

                if (string.IsNullOrEmpty(levelStr))
                {
                    if (!LogLevel.TryGetValue("Default", out levelStr) || string.IsNullOrEmpty(levelStr))
                    {
                        level = Microsoft.Extensions.Logging.LogLevel.None;
                        return false;
                    }
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
            var maxSizeStr = MaxFileSize.ToUpper();
            long size = 0;

            try
            {
                if (maxSizeStr.EndsWith("KB"))
                {
                    size = Convert.ToInt64(maxSizeStr.Replace("KB", "")) * 1024;
                }
                else if (maxSizeStr.EndsWith("MB"))
                {
                    size = Convert.ToInt64(maxSizeStr.Replace("MB", "")) * 1024 * 1024;
                }
                else if (maxSizeStr.EndsWith("GB"))
                {
                    size = Convert.ToInt64(maxSizeStr.Replace("GB", "")) * 1024 * 1024 * 1024;
                }
                else if (maxSizeStr.EndsWith("B"))
                {
                    size = Convert.ToInt64(maxSizeStr.Replace("B", ""));
                }
                else
                {
                    size = 8 * 1024 * 1024;
                }
            }
            catch
            {
                throw new Exception("rolling file provider sttings MaxFileSize is a wrong number");
            }

            return size;
        }
    }
}
