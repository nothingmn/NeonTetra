using System;
using System.Collections.Generic;
using System.Text;
using Akka.Configuration;

namespace NeonTetra.Services.Akka
{
    public class ActorSystemConfiguration
    {
        public Config LoadHoconConfigurationFromFile(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                var hocon = System.IO.File.ReadAllText(fileName);
                if (!string.IsNullOrEmpty(hocon))
                {
                    var config = ConfigurationFactory.ParseString(hocon);
                    return config;
                }
            }

            return null;
        }

        public Config LoadHoconConfigPriority(string clusterName)
        {
            //priority is first look for machinename, then cluster, then generic

            var machineNameHoconConfigFile = System.Environment.MachineName + ".hocon";
            if (System.IO.File.Exists(machineNameHoconConfigFile)) return LoadHoconConfigurationFromFile(machineNameHoconConfigFile);

            var clusterConfigFile = clusterName + ".hocon";
            if (System.IO.File.Exists(clusterConfigFile)) return LoadHoconConfigurationFromFile(clusterConfigFile);

            var lastResortConfigFile = "Configuration.hocon";
            if (System.IO.File.Exists(lastResortConfigFile)) return LoadHoconConfigurationFromFile(lastResortConfigFile);

            return new Config();
        }
    }
}