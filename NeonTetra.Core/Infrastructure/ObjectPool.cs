using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Infrastructure
{
    public class ObjectPool<TClass> : IDisposable, IObjectPool<TClass> where TClass : IDisposable
    {
        private readonly ConcurrentQueue<PoolableObject<TClass>> _concurrentQueue;
        private int _nummberOfPooledObjects;

        public ObjectPool(int maxCapacity)
        {
            MaxCapacity = maxCapacity;

            _nummberOfPooledObjects = 0;

            _concurrentQueue = new ConcurrentQueue<PoolableObject<TClass>>();
        }

        public int MaxCapacity { get; }

        public void Dispose()
        {
            foreach (var payload in _concurrentQueue.Select(x => x.Payload).ToArray()) payload.Dispose();
        }

        public IPoolableObject<TClass> GetOrAdd(Func<TClass> createFactory)
        {
            if (!_concurrentQueue.TryDequeue(out var pooledObject))
            {
                if (_nummberOfPooledObjects >= MaxCapacity)
                    throw new ApplicationException(
                        "No objects available in the pool. Can't register new object because object pool size reached maximum capacity: " +
                        MaxCapacity);

                pooledObject = new PoolableObject<TClass>(this) {Payload = createFactory()};
                Interlocked.Increment(ref _nummberOfPooledObjects);
            }

            return pooledObject;
        }

        internal void Add(PoolableObject<TClass> pooledObject)
        {
            Interlocked.Increment(ref _nummberOfPooledObjects);
            _concurrentQueue.Enqueue(pooledObject);
        }
    }
}