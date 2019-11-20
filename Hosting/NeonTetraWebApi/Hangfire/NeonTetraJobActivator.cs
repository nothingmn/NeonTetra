using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using NeonTetra.Contracts;

namespace NeonTetraWebApi.Hangfire
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