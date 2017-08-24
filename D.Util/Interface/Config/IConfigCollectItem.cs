using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D.Util.Interface
{
    /// <summary>
    /// 配置
    /// </summary>
    public interface IConfigCollectItem
    {
        /// <summary>
        /// 类似 xml 的 path
        /// </summary>
        string Path { get; }

        /// <summary>
        /// 所属的实例名
        /// </summary>
        string InstanceName { get; }

        /// <summary>
        /// 从那个 loader 加载来的（直接注入的没有来源）
        /// </summary>
        IConfigLoader FromeLoader { get; }

        /// <summary>
        /// 配置数据
        /// </summary>
        IConfig ConfigData { get; }
    }
}
