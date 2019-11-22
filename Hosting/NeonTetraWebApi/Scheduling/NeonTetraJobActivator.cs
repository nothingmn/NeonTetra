using System;
using Hangfire;
using NeonTetra.Contracts;

namespace NeonTetraWebApi.Scheduling
{
    public class NeonTetraJobActivator : JobActivator
    {
        public IResolve Resolver { get; set; }

        /// <inheritdoc />
        public override object ActivateJob(Type jobType)
        {
            return Resolver.Resolve(jobType);
        }
    }
}