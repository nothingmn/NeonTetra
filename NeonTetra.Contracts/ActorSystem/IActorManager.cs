using System;
using System.Collections.Generic;
using System.Text;

namespace NeonTetra.Contracts.ActorSystem
{
    public interface IActorManager
    {
        INeonActor GetByPath(string path);

        INeonActor Create<T>(string id);
    }

    public interface INeonActor
    {
        object RootActor { get; }

        void Tell(object message, INeonActor sender = null);
    }
}