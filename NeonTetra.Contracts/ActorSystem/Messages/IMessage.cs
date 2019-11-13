using System;

namespace NeonTetra.Contracts.ActorSystem.Messages
{
    public interface IMessage
    {
        string MessageId { get; set; }
        DateTimeOffset Timestamp { get; set; }
    }
}