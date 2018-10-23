using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.Utils
{
    /// <summary>
    /// 开始，主要是配置
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        /// 日志工厂
        /// </summary>
        ILoggerFactory LoggerFactory { get; set; }

        /// <summary>
        /// 配置
        /// </summary>
        IConfiguration Configuration { get; set; }

        /// <summary>
        /// 配置依赖注入
        /// </summary>
        /// <param name="service"></param>
        IServiceProvider ConfigService(IServiceCollection service);
    }
}
