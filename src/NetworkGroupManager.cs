using System.Collections.Generic;
using System.IO;
using System.Reflection;
using YamlDotNet.Serialization;

namespace NetworkSwitcher
{
    public class NetworkGroupManager
    {
        private readonly string configPath;

        public List<NetworkGroup> NetworkGroups { get; set; }

        public NetworkGroupManager()
        {
            configPath = Path.Combine(new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent.FullName, "config.yml");
        }

        public void LoadConfiguration()
        {
            var deserializer = new Deserializer();

            TextReader textReader = File.OpenText(configPath);

            this.NetworkGroups = deserializer.Deserialize<List<NetworkGroup>>(textReader);
        }
    }
}