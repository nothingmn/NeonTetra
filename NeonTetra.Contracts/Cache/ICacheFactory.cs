namespace NeonTetra.Contracts.Cache
{
    public enum KnownCaches
    {
        TcpServerInstances,
        EnglishResources,
        FrenchResources,
        BotCache,
        vAutoData,
        Conversations
    }

    public interface ICacheFactory
    {
        ICache<TKey, TValue> CreateCache<TKey, TValue>(string name);

        object CreateCache<TInterface>(string name, string instanceName = null);

        ICache<TKey, TValue> CreateCache<TKey, TValue>(KnownCaches knownCache);
    }
}