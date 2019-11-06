using System.Reflection;
using NeonTetra.Contracts;

namespace NeonTetra.Core.Configuration
{
    public class AssemblyInfoVersionProvider : IVersionProvider
    {
        private static readonly string _version = "1.0.0";

        static AssemblyInfoVersionProvider()
        {
            _version = typeof(AssemblyInfoVersionProvider).GetTypeInfo().Assembly.GetName().Version.ToString();
        }

        public string GetVersion()
        {
            return _version;
        }
    }
}