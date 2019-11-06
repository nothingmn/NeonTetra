using System;
using System.Collections.Generic;
using System.Text;

namespace NeonTetra.Contracts.Configuration
{
    public enum KnownQueues
    {
        Conversations
    }

    public enum QueueType
    {
        InMemory
    }

    public interface IQueuesConfiguration
    {
        IList<IQueueConfiguration> Queues { get; set; }
    }

    public interface IQueueConfiguration
    {
        string Name { get; set; }
        QueueType Type { get; set; }
    }
}