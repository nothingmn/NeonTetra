namespace NeonTetra.Contracts.Infrastructure
{
    public enum StatusTypes
    {
        Queue,
        InMemoryQueue,
        MsMqQueue,
        Host,
        TableStoreQueue,
        ServiceBusPublisher,
        ServiceBusSubscriber,
        Connector,
        SearchIndex
    }

    public interface IStatus
    {
        IStatusReport GetStatus();
    }
}