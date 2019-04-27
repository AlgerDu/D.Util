using System;
using System.Collections.Generic;
using System.Text;

namespace D.Infrastructures
{
    /// <summary>
    /// App
    /// </summary>
    public interface IApplication
    {
        /// <summary>
        /// 启动应用（保证此方法在主线程中被调用）
        /// </summary>
        /// <returns></returns>
        IApplication Run();

        /// <summary>
        /// 停止应用（保证此方法在主线程中被调用）
        /// </summary>
        /// <returns></returns>
        IApplication Stop();
    }
}
