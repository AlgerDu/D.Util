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

        public ConfigCollector()
        {
            _collectItems = new List<ConfigCollectItem>();
        }

        #region IConfigCollector 接口实现
        public void AddLoader(IConfigLoader loader)
        {
            throw new NotImplementedException();
        }

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
            throw new NotImplementedException();
        }
        #endregion
    }
}
