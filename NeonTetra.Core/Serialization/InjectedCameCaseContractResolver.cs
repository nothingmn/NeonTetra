using NeonTetra.Contracts;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;
using IContractResolver = NeonTetra.Contracts.Serialization.IContractResolver;

namespace NeonTetra.Core.Serialization
{
    public class InjectedCameCaseContractResolver : CamelCasePropertyNamesContractResolver, IContractResolver
    {
        private readonly IDIContainer _container;

        public InjectedCameCaseContractResolver(IDIContainer container)
        {
            _container = container;
        }

        public override JsonContract ResolveContract(Type type)
        {
            if (type.GetTypeInfo().IsInterface)
                try
                {
                    var t = _container.Resolve(type);
                    if (t == null) throw new NotImplementedException(type.FullName);
                    var i = t.GetType();
                    return base.ResolveContract(i);
                }
                catch (Exception)
                {
                    //somethig went wrong, lets let the bsae resolver give it a try...
                }

            return base.ResolveContract(type);
        }
    }
}