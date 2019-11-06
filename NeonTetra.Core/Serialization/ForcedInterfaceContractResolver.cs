using NeonTetra.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NeonTetra.Core.Serialization
{
    public class ForcedInterfaceContractResolver<T> : ContractResolver
    {
        private readonly IEnumerable<PropertyInfo> _properties;
        private readonly TypeInfo _type;

        public ForcedInterfaceContractResolver(IDIContainer container) : base(container)
        {
            _type = typeof(T).GetTypeInfo();
            _properties = _type.DeclaredProperties;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            property.ShouldSerialize = i =>
            {
                var existingProperty = from p in _properties where p.Name == member.Name select p;

                if (existingProperty.Any()) return true;

                return false;
            };

            return property;
        }
    }
}