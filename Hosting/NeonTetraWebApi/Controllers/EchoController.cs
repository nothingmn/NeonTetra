using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeonTetra.Contracts.ActorSystem;
using NeonTetra.Contracts.ActorSystem.Actors;

namespace NeonTetraWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EchoController : ControllerBase
    {
        private readonly IActorManager _actorManager;

        public EchoController(IActorManager actorManager)
        {
            _actorManager = actorManager;
        }

        [HttpGet]
        public async Task<string> Get(string input)
        {
            var echoActor = _actorManager.Create<IEchoActor>(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString());
            return await echoActor.Ask<string>(input);
        }
    }
}