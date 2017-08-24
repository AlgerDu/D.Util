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
            throw new NotImplementedException();
        }

        public IConfigProvider CreateProvider()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
