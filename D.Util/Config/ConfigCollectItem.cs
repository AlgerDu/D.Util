using D.Util.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D.Util.Config
{
    internal class ConfigCollectItem : IConfigCollectItem, IEquatable<ConfigCollectItem>
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

        public override bool Equals(object obj)
        {
            return Equals(obj as ConfigCollectItem);
        }

        public bool Equals(ConfigCollectItem other)
        {
            return other != null &&
                   Path == other.Path &&
                   InstanceName == other.InstanceName;
        }

        public override int GetHashCode()
        {
            var hashCode = 1285529631;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Path);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InstanceName);
            return hashCode;
        }

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
