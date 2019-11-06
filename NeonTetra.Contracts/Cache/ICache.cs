using System.Collections.Generic;
using System.Threading.Tasks;

namespace NeonTetra.Contracts.Cache
{
    public interface ICache<TKey, TValue>
    {
        Task SetItemAsync(TKey key, TValue item);

        Task<TValue> GetItemAsync(TKey key);

        Task<bool> ContainsKeyAsync(TKey key);
    }

    public interface ICacheKeys<TKey>
    {
        IEnumerable<TKey> Keys { get; }
    }
}