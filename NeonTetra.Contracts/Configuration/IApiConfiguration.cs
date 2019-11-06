using System;
using System.Collections.Generic;
using System.Text;

namespace NeonTetra.Contracts.Configuration
{
    public interface IApiConfiguration
    {
        string Url { get; set; }
        string SharedSecret { get; set; }
    }
}