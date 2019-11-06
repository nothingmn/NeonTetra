using System;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface IObjectPool<TClass> where TClass : IDisposable
    {
        IPoolableObject<TClass> GetOrAdd(Func<TClass> createFactory);
    }

    public interface IPoolableObject<TClass> : IDisposable where TClass : IDisposable
    {
        TClass Payload { get; set; }
    }
}