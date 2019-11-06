using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using NeonTetra.Contracts.ActorSystem.Actors;
using NeonTetra.Contracts.Logging;

namespace NeonTetra.Services.Akka.Actors
{
    public class EchoActor : ReceiveActor, IEchoActor
    {
        private readonly ILog _logger;

        private string _lastMessage = "";

        public EchoActor(ILog logger)
        {
            _logger = logger;
            Receive<string>(msg =>
            {
                _lastMessage = msg;
                Sender.Tell($"Hello from Echo:{msg}");
            });
        }

        protected override void PreStart() => _logger.Information($"EchoActor started");

        protected override void PostStop() => _logger.Information($"EchoActor stopped");
    }
}