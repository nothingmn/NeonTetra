using System.Collections.Generic;
using System.Threading.Tasks;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface IDomainDataLoader
    {
        Task<IDictionary<string, byte[]>> LoadManifestStreams(string assemblyFullFilePath, string assemblyName, bool isolate = true);
    }
}