using D.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Logger.RollingFile
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new LoggerFactory()
                   //.AddConsole(LogLevel.Trace)
                   .AddRollingFile();

            ILogger logger = factory.CreateLogger<Program>();

            logger.LogError("LogError");
            logger.LogInformation("LogInformation");
            logger.LogDebug("LogDebug");

            Console.ReadKey();
        }
    }
}
