using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace D.Examples.App.Console
{
    public class Startup
    {


        public void ConfigServices(IServiceCollection services)
        {
            services.AddTransient<IRunable, ServiceA>();
        }

        public void ConfigServices(ContainerBuilder builder)
        {

        }
    }
}
