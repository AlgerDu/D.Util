using D.Util.Interface;

namespace D.Util.Logger
{
    internal class LogFilter
    {
        public string Namesapce { get; set; }

        public LogLevel LogLevel { get; set; }
    }

    /// <summary>
    /// 通用的日志配置
    /// </summary>
    internal class LoggerConfig : IConfig
    {
        public string Path
        {
            get
            {
                return "logging";
            }
        }

        public LogFilter[] Filters { get; set; }

        public LoggerConfig()
        {
            Filters = new LogFilter[0];
        }
    }
}
