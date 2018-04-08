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

            logger.LogError("LogError");
            logger.LogInformation("LogInformation");
            logger.LogDebug("LogDebug");

            Console.ReadKey();
        }
    }
}
