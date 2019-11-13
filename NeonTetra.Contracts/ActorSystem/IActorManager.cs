using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NeonTetra.Contracts.ActorSystem
{
    public interface IActorManager
    {
        Task<INeonActor> GetByPath<T>(string path);

        INeonActor Create<T>(string id);
    }

    public interface INeonActor
    {
        object RootActor { get; }

        void Tell(object message, INeonActor sender = null);

        Task<T> Ask<T>(object message, TimeSpan? timeout = null);
    }
}