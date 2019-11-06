using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts.Configuration;

namespace NeonTetra.Core.Configuration
{
    public class ApiConfiguration : IApiConfiguration
    {
        public string Url { get; set; }
        public string SharedSecret { get; set; }
    }
}