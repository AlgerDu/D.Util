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

        /// <summary>
        /// 用户、内容文件的绝对路径
        /// </summary>
        string ContentRootPath { get; set; }

        /// <summary>
        /// 程序可执行文件的绝对路径
        /// </summary>
        string AppRootPath { get; set; }

        /// <summary>
        /// 程序名称
        /// </summary>
        string ApplicationName { get; set; }
    }
}
