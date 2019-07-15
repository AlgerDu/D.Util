using System;
using System.Collections.Generic;
using System.Text;

namespace D.Infrastructures.Application
{
    internal class HostingEnvironment : IHostingEnvironment
    {
        public string EnvironmentName { get; set; }

        public string ContentRootPath { get; set; }

        public string AppRootPath { get; set; }

        public string ApplicationName { get; set; }
    }
}
