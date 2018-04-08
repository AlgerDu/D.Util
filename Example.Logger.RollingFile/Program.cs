using D.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Logger.RollingFile
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();


            var factory = new LoggerFactory()
                   //.AddConsole(LogLevel.Trace)
                   .AddRollingFile(configuration.GetSection("Logging:RollingFile"));

            ILogger logger = factory.CreateLogger<Program>();

            var i = 0;

            while (i < 100)
            {
                logger.LogError($"LogError {i}");
                logger.LogInformation($"LogInformation {i}");
                logger.LogDebug($"LogDebug {i}");
            }

            Console.ReadKey();
        }
    }
}
