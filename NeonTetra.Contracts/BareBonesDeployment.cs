using System.Collections.Generic;
using System.Threading.Tasks;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Contracts
{
    public class BareBonesDeployment : IDeployment
    {
        public Task Start(IDIContainer container, IDictionary<string, object> environment = null)
        {
            Container = container;
            return Task.CompletedTask;
        }

        public Task Stop(bool throwOnFirstException = false)
        {
            return Task.CompletedTask;
        }

        public IDIContainer Container { get; private set; }
    }
}