using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D.Util.Interface
{
    /// <summary>
    /// 配置加载器
    /// </summary>
    public interface IConfigLoader
    {
        /// <summary>
        /// 从加载器中获取配置
        /// </summary>
        /// <param name="path">类似与 XML 中的 path</param>
        /// <param name="instanceName">实例名（为多实例做准备）</param>
        /// <returns></returns>
        T Load<T>(string path, string instanceName = null) where T : class, IConfig, new();

        /// <summary>
        /// 保存所有从此 loader 加载的配置项，思考半天，觉得还是需要个保存的接口
        /// </summary>
        void Save();
    }
}
