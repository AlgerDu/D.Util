using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D.Util.Interface
{
    /// <summary>
    /// 配置提供者
    /// </summary>
    public interface IConfigProvider
    {
        /// <summary>
        /// 获取配置，当没有从配置中找到对应的项的时候，返回 null
        /// </summary>
        /// <typeparam name="T">具体的配置类</typeparam>
        /// <param name="instanceName">实例名</param>
        /// <returns></returns>
        T GetConfig<T>(string instanceName = null) where T : class, IConfig, new();
    }
}
