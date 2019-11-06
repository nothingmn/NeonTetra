using System.Collections.Generic;
using System.Threading.Tasks;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface IDeploymentManager
    {
        bool Started { get; }

        Task<IDeployment> Start(IDIContainer rootContainer = null, IDictionary<string, object> environment = null);
    }
}