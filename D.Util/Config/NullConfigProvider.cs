using D.Util.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D.Util.Config
{
    /// <summary>
    /// 使用 NullObject 代替 null
    /// </summary>
    public class NullConfigProvider : IConfigProvider
    {
        T IConfigProvider.GetConfig<T>(string instanceName)
        {
            //永远都返回一个 null 值的配置对象
            return null;
        }

        T IConfigProvider.GetConfigNullWithDefault<T>(string instanceName)
        {
            //永远都返回一个 null 值的配置对象
            return null;
        }
    }
}
