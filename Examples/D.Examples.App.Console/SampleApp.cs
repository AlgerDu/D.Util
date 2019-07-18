using D.Infrastructures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.Examples.App.Console
{
    class AppOptions
    {
        public AppOptions()
        {
            Name = "unknown";
        }

        public string Name { get; set; }
    }

    class SampleApp : IApplication
    {
        ILogger _logger;
        AppOptions _options;

        IEnumerable<IRunnable> _runners;

        public SampleApp(
            ILogger<SampleApp> logger
            , IEnumerable<IRunnable> runners
            , IOptionsMonitor<AppOptions> optionsAccessor
            )
        {
            _logger = logger;
            _options = optionsAccessor.CurrentValue;

            _runners = runners;
        }

        public IApplication Run()
        {
            _logger.LogInformation($"{_options.Name} app start running.");

            foreach (var runner in _runners)
            {
                runner.Run();
            }

            return this;
        }

        public IApplication Stop()
        {
            foreach (var runner in _runners)
            {
                runner.Stop();
            }

            _logger.LogInformation($"{_options.Name} app stoped.");
            return this;
        }
    }
}
