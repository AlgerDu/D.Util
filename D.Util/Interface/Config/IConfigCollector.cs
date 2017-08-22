using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D.Util.Interface
{
    /// <summary>
    /// 配置收集器（设计思路学习 autofac）
    /// </summary>
    public interface IConfigCollector
    {
        /// <summary>
        /// 创建一个配置提供者，同一个实例（TODO 暂定）
        /// </summary>
        /// <returns></returns>
        IConfigProvider CreateProvider();

        /// <summary>
        /// 像 collector 中手动添加一个配置对象
        /// </summary>
        /// <param name="config"></param>
        /// <param name="instanceName"></param>
        void CollectConfig(IConfig config, string instanceName = null);

        /// <summary>
        /// 像 collector 中添加一个 loader，用于获取配置
        /// </summary>
        /// <param name="loader"></param>
        void AddLoader(IConfigLoader loader);
    }
}
