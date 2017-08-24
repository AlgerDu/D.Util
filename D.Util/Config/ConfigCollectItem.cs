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
    }
}
