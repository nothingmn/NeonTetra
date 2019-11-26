using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Dashboard;
using NeonTetra.Contracts;

namespace NeonTetraWebApi.Scheduling
{
    public class CustomHangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly IDIContainer _container;

        public CustomHangfireAuthorizationFilter(IDIContainer container)
        {
            _container = container;
        }

        public bool Authorize(DashboardContext context)
        {
            //need to incorporate the user manager here
            return false;
        }
    }
}