using D.Util.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D.Util.Config
{
    /// <summary>
    /// IConfigProvider 接口简单实现
    /// </summary>
    public class ConfigProvider : IConfigProvider
    {
        IEnumerable<IConfigCollectItem> _configItems;
        IEnumerable<IConfigLoader> _configLoaders;

        public ConfigProvider(
            IEnumerable<IConfigCollectItem> configItems
            , IEnumerable<IConfigLoader> configLoaders
            )
        {
            _configItems = configItems;
            _configLoaders = configLoaders;
        }

        #region IConfigProvider 接口相关
        T IConfigProvider.GetConfig<T>(string instanceName)
        {
            var tmpT = new T();
            T config = null;

            var find = _configItems?.Where(item => item.Path == tmpT.Path && item.InstanceName == instanceName);

            if (find != null && find.Count() >= 1)
            {
                config = find.First().ConfigData as T;
            }
            else if (_configLoaders != null)
            {
                foreach (var loader in _configLoaders)
                {
                    config = loader.Load(tmpT.Path, instanceName) as T;

                    if (config != null) break;
                }
            }

            return config;
        }

        T IConfigProvider.GetConfigNullWithDefault<T>(string instanceName)
        {
            var config = (this as IConfigProvider).GetConfig<T>(instanceName);

            if (config == null)
            {
                config = new T();
            }

            return config;
        }
        #endregion
    }
}
