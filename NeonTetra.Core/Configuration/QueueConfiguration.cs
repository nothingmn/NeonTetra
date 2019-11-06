using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts.Configuration;

namespace NeonTetra.Core.Configuration
{
    public class QueuesConfiguration : IQueuesConfiguration
    {
        public IList<IQueueConfiguration> Queues { get; set; }
    }

    public class QueueConfiguration : IQueueConfiguration
    {
        public string Name { get; set; }
        public QueueType Type { get; set; }
    }
}