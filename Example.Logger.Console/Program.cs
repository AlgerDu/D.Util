using Autofac;
using D.Util.Config;
using D.Util.Interface;
using D.Util.Logger;
using D.Utils.AutofacExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Logger.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var useDefault = true;

            System.Console.WriteLine($"是否使用默认的配置文件：{useDefault}");
            System.Console.WriteLine("下面是日志输出：\r\n");

            var configCollector = new ConfigCollector();

            if (!useDefault)
            {
                var config = new ConsoleLogWriterConfig
                {
                    LogLevel = LogLevel.info
                };

                configCollector.CollectConfig(config);
            }

            var builder = new ContainerBuilder();

            builder.AddUtils();
            builder.AddConsoleLogWriter();
            builder.AddConfigProvider(configCollector.CreateProvider());

            var container = builder.Build();

            var logger = container
                .Resolve<ILoggerFactory>()
                .CreateLogger<Program>();

            var name = "日志";

            logger.LogTrace($"helle trace {name}");
            logger.LogDebug($"helle debug {name}");
            logger.LogInformation($"helle info {name}");
            logger.LogWarning($"helle warn {name}");
            logger.LogError($"helle error {name}");
            logger.LogCritical($"helle crit {name}");

            System.Console.ReadKey();
        }
    }
}
