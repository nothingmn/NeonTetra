using NeonTetra.Contracts;
using NeonTetra.Contracts.Encoding;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Contracts.Infrastructure.Compression;
using NeonTetra.Contracts.Infrastructure.Encryption;
using NeonTetra.Contracts.Serialization;
using NeonTetra.Contracts.Services;
using NeonTetra.Core.Configuration;
using NeonTetra.Core.Encoding;
using NeonTetra.Core.Infrastructure;
using NeonTetra.Core.Infrastructure.Encryption;
using NeonTetra.Core.Serialization;
using NeonTetra.Core.Services;
using Newtonsoft.Json;
using System;
using JsonSerializer = NeonTetra.Core.Serialization.JsonSerializer;

namespace NeonTetra.DI.Containers
{
    public class CoreServicesRegistrationContainer : IRegistrationContainer
    {
        public void Register(IDIContainer container)
        {
            container.RegisterInstance(container, typeof(IDIContainer));
            container.RegisterInstance(container, typeof(IRegister));
            container.RegisterInstance(container, typeof(IResolve));

            container.Register<ISerializerFactory, SerializerFactory>();

            container.Register<ISerialize, JsonSerializer>();
            container.Register<ISerializer, JsonSerializer>();
            container.Register<IDeserialize, JsonSerializer>();

            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.ContractResolver = new ContractResolver(container);
                // do something with settings
                return settings;
            };

            container.Register<ISerialize, JsonSerializer>(KnownSerializerFormats.JSON.ToString());
            container.Register<ISerializer, JsonSerializer>(KnownSerializerFormats.JSON.ToString());
            container.Register<IDeserialize, JsonSerializer>(KnownSerializerFormats.JSON.ToString());

            container.Register<ISerialize, JsonSerializer>(KnownSerializerIntents.Logging.ToString());
            container.Register<ISerializer, JsonSerializer>(KnownSerializerIntents.Logging.ToString());
            container.Register<IDeserialize, JsonSerializer>(KnownSerializerIntents.Logging.ToString());

            container.Register<IEncryptionFactory, BasicEncryptionFactory>();
            container.Register<IEncryption, BasicEncryption>();
            container.Register<IEncrypter, BasicEncryption>();
            container.RegisterInstance(EncryptionAlgorithm.Rijndael, typeof(EncryptionAlgorithm));
            container.Register<IDecrypter, BasicEncryption>();
            container.Register<IEncryptionKeyReadProvider, ConfigurationBasedEncryptionKeyProvider>();
            container.Register<IEncryptionKey, EncryptionKey>();
            container.Register<IValidateEncryptionKeys, ValidateEncryptionKeys>();

            container.RegisterSingleton<IVersionProvider, AssemblyInfoVersionProvider>();
            container.Register<IContractResolver, ContractResolver>();

            container.Register<ICompressionFactory, CompressionFactory>();
            container.Register<ICompression, Compression>();

            container.Register<ICharacterEncodingConverter, CharacterEncodingConverter>();

            container.Register(typeof(IProgress<>), typeof(Progress<>));

            container.Register(typeof(ITimedBufferBatch<>), typeof(TimedBufferBatch<>));
            container.Register(typeof(ITimedBufferBatchWithErrorRetries<>), typeof(TimedBufferBatchWithErrorRetries<>));

            container.Register<IFeatureFlagProvider, ConfigurationBasedFeatureFlagProvider>();

            container.Register<IDomainDataLoader, DomainDataLoader>();
        }
    }
}