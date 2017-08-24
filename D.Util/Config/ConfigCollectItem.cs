using D.Util.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D.Util.Config
{
    internal class ConfigCollectItem : IConfigCollectItem
    {
        public string Path
        {
            get
            {
                return ConfigData.Path;
            }
        }

        public string InstanceName { get; set; }

        public IConfigLoader FromeLoader { get; set; }

        public IConfig ConfigData { get; set; }

        public static bool operator ==(ConfigCollectItem l, ConfigCollectItem r)
        {
            return l.Path == r.Path && l.InstanceName == l.InstanceName;
        }

        public static bool operator !=(ConfigCollectItem l, ConfigCollectItem r)
        {
            return l.Path != r.Path || l.InstanceName != r.InstanceName;
        }
    }
}
