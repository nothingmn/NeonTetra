using NeonTetra.Contracts;
using System;
using System.Reflection;
using IContractResolver = NeonTetra.Contracts.Serialization.IContractResolver;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NeonTetra.Contracts.Logging;

namespace NeonTetra.Core.Serialization
{
    public class ContractResolver : CamelCasePropertyNamesContractResolver, IContractResolver
    {
        private readonly IDIContainer _container;
        private readonly ILog _log;

        public ContractResolver(IDIContainer container)
        {
            _container = container;
            _log = _container.Resolve<ILogFactory>().CreateLog(GetType());
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            return property;
        }

        public override JsonContract ResolveContract(Type type)
        {
            if (type.GetTypeInfo().IsInterface)
            {
                if (_container.IsRegistered(type))
                {
                    var t = _container.Resolve(type);
                    if (t != null)
                    {
                        var i = t.GetType();
                        return base.ResolveContract(i);
                    }
                }
                else
                {
                    _log.Error("JSON contract resolver failed to resolve type {0}.", type.FullName);
                }
            }

            return base.ResolveContract(type);
        }
    }
}