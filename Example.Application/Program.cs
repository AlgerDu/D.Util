using D.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Example.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new ApplicationBuilder()
                .ConfigureAppConfiguration((config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json");
                })
                .ConfigureLogging((config, logging) =>
                {
                    logging.AddConfiguration(config.GetSection("logging"));
                    logging.AddConsole();
                })
                .Use<Startup>()
                .Builde<TestApp>();

            app.Run();

            app.Stop();

            Console.ReadKey();
        }
    }

    class Startup : IStartup
    {
        public ILoggerFactory LoggerFactory { get; set; }
        public IConfiguration Configuration { get; set; }

        public void ConfigService(IServiceCollection service)
        {
        }
    }

    class TestApp : IApplication
    {
        ILogger _logger;

        public TestApp(
            ILogger<TestApp> logger
            )
        {
            _logger = logger;
        }

        public IApplication Run()
        {
            _logger.LogInformation($"TestApp run");

            return this;
        }

        public IApplication Stop()
        {
            _logger.LogTrace($"TestApp stop");

            return this;
        }
    }
}
