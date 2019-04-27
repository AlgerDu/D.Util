using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.Infrastructures
{
    /// <summary>
    /// IApplicationBuilder 上下文
    /// </summary>
    public class ApplicationBuilderContext
    {
        /// <summary>
        /// App 的配置
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// App 的运行环境
        /// </summary>
        public IHostingEnvironment Environment { get; set; }
    }
}
