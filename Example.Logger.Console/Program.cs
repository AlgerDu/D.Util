using Autofac;
using D.Util.Interface;
using D.Util.Logger;
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
            var builder = new ContainerBuilder();

            builder.RegisterType<ConsoleLogWriter>()
                .As<ILogWriter>();

            builder.RegisterType<LoggerFactory>()
                .As<ILoggerFactory>();

            var container = builder.Build();

            var loggerFactory = container.Resolve<ILoggerFactory>();

            var logger = loggerFactory.CreateLogger<Program>();

            var name = "日志";

            logger.LogDebug($"helle debug {name}");

            System.Console.ReadKey();
        }
    }
}
