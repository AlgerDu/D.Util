using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.Utils
{
    /// <summary>
    /// 一个尝试
    /// </summary>
    public class DLoggingBuilder : ILoggingBuilder
    {
        /// <summary>
        /// 服务
        /// </summary>
        public IServiceCollection Services { get; set; }
    }
}
