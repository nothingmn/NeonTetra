using System.Collections.Generic;
using System.Threading.Tasks;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface IDeployment
    {
        IDIContainer Container { get; }
        Task Start(IDIContainer container, IDictionary<string, object> environment = null);

        Task Stop(bool throwOnFirstException = false);
    }
}