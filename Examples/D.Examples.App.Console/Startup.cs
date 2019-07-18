using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using D.Infrastructures;

namespace D.Examples.App.Console
{
    public class Startup
    {
        IHostingEnvironment _env;
        IConfiguration _config;
        ILoggerFactory _loggerFactory;

        public Startup(
            IHostingEnvironment env
            , IConfiguration config
            , ILoggerFactory loggerFactory
            )
        {
            _env = env;
            _config = config;
            _loggerFactory = loggerFactory;
        }

        public void ConfigOptions(IServiceCollection services)
        {
            services.Configure<AppOptions>(_config);
        }

        public void ConfigServices(IServiceCollection services)
        {
            services.AddSingleton<IRunnable, ServiceA>();
        }

        public void ConfigServices(ContainerBuilder builder)
        {
            builder.RegisterType<ServiceB>()
                .As<IRunnable>()
                .AsSelf();
        }
    }
}
