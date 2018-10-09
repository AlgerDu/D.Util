using System;
using System.Collections.Generic;
using System.Text;

namespace D.Utils
{
    /// <summary>
    /// App
    /// </summary>
    public interface IApplication
    {
        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        IApplication Run();

        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        IApplication Stop();
    }
}
