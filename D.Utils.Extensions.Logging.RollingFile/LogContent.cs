using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.Utils.Extensions.Logging.RollingFile
{
    /// <summary>
    /// 日志内容
    /// </summary>
    public class LogContent
    {
        public LogLevel LogLevel { get; set; }
        public EventId EventId { get; set; }
        public string Msg { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Category { get; set; }
        public Exception Ex { get; set; }
        public int ThreadID { get; set; }
    }
}
