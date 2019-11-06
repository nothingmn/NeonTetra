using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using NeonTetra.Contracts.ActorSystem.Actors;
using NeonTetra.Contracts.Logging;

namespace NeonTetra.Services.Akka.Actors
{
    public class SimpleLoggingActor : ReceiveActor, ISimpleLoggingActor
    {
        private readonly ILog _logger;

        public SimpleLoggingActor(ILog logger)
        {
            _logger = logger;
            Receive<object>(msg =>
            {
                _logger.Debug(Newtonsoft.Json.JsonConvert.SerializeObject(msg));
            });
        }

        protected override void PreStart() => _logger.Information($"Device actorSimpleLoggingActor started");

        protected override void PostStop() => _logger.Information($"Device actor SimpleLoggingActor stopped");
    }
}