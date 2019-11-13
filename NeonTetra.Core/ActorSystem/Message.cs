using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts.ActorSystem.Messages;

namespace NeonTetra.Core.ActorSystem
{
    public class Message : IMessage
    {
        public string MessageId { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
    }
}