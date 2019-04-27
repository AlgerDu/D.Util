using System;
using System.Collections.Generic;
using System.Text;

namespace D.Infrastructures
{
    /// <summary>
    /// App 运行的一些环境变量
    /// </summary>
    public interface IHostingEnvironment
    {
        /// <summary>
        /// 环境名称
        /// </summary>
        string EnvironmentName { get; set; }
    }
}
