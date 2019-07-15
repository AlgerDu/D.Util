using D.Infrastructures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;

namespace D.Examples.App.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new ApplicationBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .UseStartup<Startup>()
                .Builde<SampleApp>();

            app.Run();

            System.Console.ReadKey();

            app.Stop();
        }
    }

    interface IRunnable
    {
        void Run();
    }

    class ServiceA : IRunnable
    {
        ILogger _logger;

        public ServiceA(
            ILogger<ServiceA> logger
            )
        {
            _logger = logger;
        }

        public void Run()
        {
            _logger.LogInformation("runable service A start running.");
        }
    }

    class SampleApp : IApplication
    {
        ILogger _logger;
        IEnumerable<IRunnable> _runners;

        public SampleApp(
            ILogger<SampleApp> logger
            , IEnumerable<IRunnable> runners
            )
        {
            _logger = logger;
            _runners = runners;
        }

        public IApplication Run()
        {
            _logger.LogInformation($"sample app start running.");

            foreach (var runner in _runners)
            {
                runner.Run();
            }

            return this;
        }

        public IApplication Stop()
        {
            _logger.LogInformation($"sample app stoped.");
            return this;
        }
    }
}
