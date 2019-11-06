using System;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Infrastructure
{
    public class PoolableObject<TClass> : IPoolableObject<TClass> where TClass : IDisposable
    {
        private readonly ObjectPool<TClass> _objectPool;

        public PoolableObject(ObjectPool<TClass> objectPool)
        {
            _objectPool = objectPool;
        }

        public TClass Payload { get; set; }

        public void Dispose()
        {
            _objectPool.Add(this);
        }
    }
}