using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using NeonTetra.Contracts.Cache;

namespace NeonTetra.Core.Caching
{
    public class NonStaticImmutableInMemoryCache<TKey, TValue> : ICache<TKey, TValue>, ICacheKeys<TKey>
    {
        public IImmutableDictionary<TKey, TValue> Cache = ImmutableDictionary.Create<TKey, TValue>();

        public Task<bool> ContainsKeyAsync(TKey key)
        {
            return Task.FromResult(Cache.ContainsKey(key));
        }

        public Task SetItemAsync(TKey key, TValue item)
        {
            var oldCache = Cache;

            // Add the new value to the cache
            if (Cache.ContainsKey(key))
            {
                var newCache = oldCache.Remove(key);
                Interlocked.CompareExchange(ref Cache, newCache, oldCache);
            }

            if (item != null)
                ApplyChangeOptimistically(
                    ref Cache,
                    d => d.ContainsKey(key) ? d : d.Add(key, item));

            return Task.CompletedTask;
        }

        public Task<TValue> GetItemAsync(TKey key)
        {
            if (Cache.ContainsKey(key)) return Task.FromResult(Cache[key]);
            return Task.FromResult(default(TValue));
        }

        public IEnumerable<TKey> Keys => Cache.Keys;

        /// <summary>
        ///     Optimistically performs some value transformation based on some field and tries to apply it back to the field,
        ///     retrying as many times as necessary until no other thread is manipulating the same field.
        /// </summary>
        /// <typeparam name="T">The type of data.</typeparam>
        /// <param name="hotLocation">The field that may be manipulated by multiple threads.</param>
        /// <param name="applyChange">A function that receives the unchanged value and returns the changed value.</param>
        private bool ApplyChangeOptimistically<T>(ref T hotLocation, Func<T, T> applyChange) where T : class
        {
            if (applyChange == null) throw new NullReferenceException("applyChange");

            bool successful;
            do
            {
                Thread.MemoryBarrier();
                var oldValue = hotLocation;
                var newValue = applyChange(oldValue);
                if (ReferenceEquals(oldValue, newValue)) return false;

                var actualOldValue = Interlocked.CompareExchange(ref hotLocation, newValue, oldValue);
                successful = ReferenceEquals(oldValue, actualOldValue);
            } while (!successful);

            Thread.MemoryBarrier();
            return true;
        }
    }
}