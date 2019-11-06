using System.Collections.Generic;
using System.Globalization;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface IResourceCompiler
    {
        IList<CultureInfo> SupportedCultures { get; }

        void Compile();
    }
}