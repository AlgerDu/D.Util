using D.Util.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D.Util.Logger
{
    /// <summary>
    /// console 日志配置文件
    /// </summary>
    public class ConsoleLogWriterConfig : IConfig
    {
        public string Path
        {
            get
            {
                return "logging.writer.console";
            }
        }

        public LogLevel LogLevel { get; set; }

        public ConsoleLogWriterConfig()
        {
            LogLevel = LogLevel.trce;
        }
    }

    /// <summary>
    /// 控制台日志 writer
    /// </summary>
    public class ConsoleLogWriter : ILogWriter
    {
        ConsoleLogWriterConfig _config;

        public ConsoleLogWriter(
            IConfigProvider configProvider
            )
        {
            _config = configProvider.GetConfigNullWithDefault<ConsoleLogWriterConfig>();
        }

        public void Write(ILogContext context)
        {
            if (context.Level < _config.LogLevel)
            {
                return;
            }

            lock (this)
            {
                Print("[");
                Print(context.Level.ToString(), LogContextTypeColor(context.Level));
                Print("]");

                Print("[");
                Print(context.CreateTime.ToString("HH:mm:ss ffff"));
                Print("]");

                Print("[");
                Print("thread " + context.ThreadID);
                Print("]");

                Print("\r\n      ");

                Print(context.ClassFullName, ConsoleColor.White);

                Print("\r\n      ");

                Print(context.Text.Replace("\r\n", "\r\n      "), LogContextTypeColor(context.Level));

                Print("\r\n");
            }
        }

        void Print(string txt, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.Write(txt);
        }

        /// <summary>
        /// 根据不同的日志类型输出不同的颜色
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ConsoleColor LogContextTypeColor(LogLevel type)
        {
            switch (type)
            {
                case LogLevel.trce: return ConsoleColor.Gray;
                case LogLevel.dbug: return ConsoleColor.Blue;
                case LogLevel.info: return ConsoleColor.Green;
                case LogLevel.warn: return ConsoleColor.Yellow;
                case LogLevel.fail: return ConsoleColor.Red;
                case LogLevel.crit: return ConsoleColor.Red;
                default: return ConsoleColor.Black;
            }
        }
    }
}
