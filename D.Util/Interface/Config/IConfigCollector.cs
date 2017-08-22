using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D.Util.Interface
{
    /// <summary>
    /// 配置收集器
    /// </summary>
    public interface IConfigCollector
    {
        /// <summary>
        /// 创建一个配置提供者，同一个实例（TODO 暂定）
        /// </summary>
        /// <returns></returns>
        IConfigProvider CreateProvider();
    }
}
