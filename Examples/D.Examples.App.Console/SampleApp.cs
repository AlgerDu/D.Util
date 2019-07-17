using D.Infrastructures;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.Examples.App.Console
{
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
            foreach (var runner in _runners)
            {
                runner.Stop();
            }

            _logger.LogInformation($"sample app stoped.");
            return this;
        }
    }
}
