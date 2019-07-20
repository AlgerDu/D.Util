using D.Infrastructures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace D.Examples.App.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new ApplicationBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());

                    config.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .UseStartupWithAutofac<Startup>()
                //.UseStartup<Startup>()
                .Builde<SampleApp>();

            app.Run();

            System.Console.ReadKey();

            app.Stop();

            System.Console.ReadKey();
        }
    }
}
