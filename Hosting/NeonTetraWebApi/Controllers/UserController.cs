using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NeonTetra.Contracts.Jobs;
using NeonTetra.Contracts.Logging;
using NeonTetra.Contracts.Membership;
using NeonTetra.Contracts.Services;
using NeonTetra.Core.Jobs;

namespace NeonTetraWebApi.Controllers
{
    [Route("api/[controller]/{id?}")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IScheduler _scheduler;
        private readonly ILoggingJob _loggingJob;
        private readonly ILog _log;

        public UserController(IUserManager userManager, ILogFactory logFactory, IScheduler scheduler, ILoggingJob loggingJob)
        {
            _log = logFactory.CreateLog(this.GetType());
            _userManager = userManager;
            _scheduler = scheduler;
            _loggingJob = loggingJob;
        }

        [HttpGet]
        public async Task<IUser> Get(string id)
        {
            _scheduler.Enqueue(() =>
               _loggingJob.Information(this.GetType(), $"User Controller get by id {id}")
            );

            return await _userManager.Get(id);
        }
    }
}