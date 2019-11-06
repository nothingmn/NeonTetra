using System;
using System.Collections.Generic;
using System.Linq;
using NeonTetra.Contracts.Configuration;

namespace NeonTetra.Core.Configuration
{
    public class AzureServicesConfiguration : IAzureServicesConfiguration
    {
        public IApplicationInsights ApplicationInsights { get; set; }
        public ILUISConfiguration LUIS { get; set; }
    }

    public class ApplicationInsights : IApplicationInsights
    {
        public string Key { get; set; }
        public bool Enabled { get; set; }
    }

    public class LUISConfiguration : ILUISConfiguration
    {
        public string AppId { get; set; }
        public string Key { get; set; }
        public string Region { get; set; }
    }
}