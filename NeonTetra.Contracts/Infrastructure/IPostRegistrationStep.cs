using System;
using System.Threading;
using System.Threading.Tasks;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface IPostRegistrationStep
    {
        Task ExecutePostRegistrationStep(IDIContainer container,
            CancellationToken cancellationToken = default(CancellationToken));
    }

    public class RegistrationStepOrderAttribute : Attribute
    {
        public int Order { get; set; } = 0;
    }
}