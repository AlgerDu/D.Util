using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.Examples.App.Console
{
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

        public void Stop()
        {
            _logger.LogInformation("runable service A start stoped.");
        }
    }

    class ServiceB : IRunnable
    {
        ILogger _logger;

        public ServiceB(
            ILogger<ServiceA> logger
            )
        {
            _logger = logger;
        }

        public void Run()
        {
            _logger.LogInformation("runable service B start running.");
        }

        public void Stop()
        {
            _logger.LogInformation("runable service B start stoped.");
        }
    }
}
