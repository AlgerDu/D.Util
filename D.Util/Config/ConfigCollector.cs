using D.Util.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D.Util.Config
{
    /// <summary>
    /// IConfigCollector 实现，基本不需要重新实现
    /// </summary>
    public class ConfigCollector : IConfigCollector
    {
        List<ConfigCollectItem> _collectItems;
        List<IConfigLoader> _configLoaders;

        public ConfigCollector()
        {
            _collectItems = new List<ConfigCollectItem>();
            _configLoaders = new List<IConfigLoader>();
        }

        #region IConfigCollector 接口实现
        public void AddLoader(IConfigLoader loader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 手动向收集器中加入配置对象
        /// </summary>
        /// <param name="config"></param>
        /// <param name="instanceName"></param>
        public void CollectConfig(IConfig config, string instanceName = null)
        {
            var newItem = new ConfigCollectItem
            {
                ConfigData = config,
                FromeLoader = null,
                InstanceName = instanceName
            };

            var findItems = _collectItems.Where(o => o == newItem);

            if (findItems.Count() > 1)
            {
                throw new Exception($"ConfigCollector CollectConfig {findItems.Count()}");
            }
            else if (findItems.Count() == 1)
            {
                _collectItems.Remove(findItems.FirstOrDefault());

                _collectItems.Add(newItem);
            }
            else
            {
                _collectItems.Add(newItem);
            }
        }

        public IConfigProvider CreateProvider()
        {
            return new ConfigProvider(_collectItems, _configLoaders);
        }
        #endregion
    }
}
