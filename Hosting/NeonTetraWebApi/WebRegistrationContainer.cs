using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Contracts.Services;
using NeonTetraWebApi.Hangfire;

namespace NeonTetraWebApi
{
    public class WebRegistrationContainer : IRegistrationContainer
    {
        public void Register(IDIContainer container)
        {
            container.Register<IScheduler, BasicHangfireScheduler>();
        }

        public Task ExecutePostRegistrationStep(IDIContainer container,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return null;
        }
    }
}