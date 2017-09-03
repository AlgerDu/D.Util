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
    public interface IConfig
    {
        /// <summary>
        /// 类似 xml 中的 path
        /// </summary>
        string Path { get; }
    }
}
