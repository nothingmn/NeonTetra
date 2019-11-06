using NeonTetra.Contracts;
using NeonTetra.Contracts.Serialization;

namespace NeonTetra.Core.Serialization
{
    public class SerializerFactory : ISerializerFactory
    {
        private readonly IDIContainer _resolver;

        public SerializerFactory(IDIContainer container)
        {
            _resolver = container;
        }

        public ISerializer SerializerForContentType(KnownSerializerFormats format)
        {
            return _resolver.Resolve<ISerializer>(format.ToString());
        }

        public ISerialize SerializeForContentType(KnownSerializerFormats format)
        {
            return _resolver.Resolve<ISerialize>(format.ToString());
        }

        public IDeserialize DeserializeForContentType(KnownSerializerFormats format)
        {
            return _resolver.Resolve<IDeserialize>(format.ToString());
        }

        public ISerializer SerializerForIntent(KnownSerializerIntents intent)
        {
            return _resolver.Resolve<ISerializer>(intent.ToString());
        }

        public ISerialize SerializeForIntent(KnownSerializerIntents intent)
        {
            return _resolver.Resolve<ISerialize>(intent.ToString());
        }

        public IDeserialize DeserializeForIntent(KnownSerializerIntents intent)
        {
            return _resolver.Resolve<IDeserialize>(intent.ToString());
        }

        public void MutateSerializerSettings(KnownSerializerFormats format, object settings)
        {
            SerializerForContentType(format).MutateSerializerSettings(settings);
        }
    }
}