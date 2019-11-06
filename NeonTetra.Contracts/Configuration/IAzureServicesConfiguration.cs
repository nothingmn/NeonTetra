using System;
using System.Collections.Generic;
using System.Text;

namespace NeonTetra.Contracts.Configuration
{
    public interface IAzureServicesConfiguration
    {
        IApplicationInsights ApplicationInsights { get; set; }
        ILUISConfiguration LUIS { get; set; }
    }

    public interface ILUISConfiguration
    {
        string AppId { get; set; }
        string Key { get; set; }
        string Region { get; set; }
    }
}