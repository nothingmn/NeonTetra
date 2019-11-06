namespace NeonTetra.Contracts.Serialization
{
    public enum KnownSerializerIntents
    {
        Logging,
    }

    public interface ISerializerFactory
    {
        ISerializer SerializerForContentType(KnownSerializerFormats format);

        ISerialize SerializeForContentType(KnownSerializerFormats format);

        IDeserialize DeserializeForContentType(KnownSerializerFormats format);

        ISerializer SerializerForIntent(KnownSerializerIntents intent);

        ISerialize SerializeForIntent(KnownSerializerIntents intent);

        IDeserialize DeserializeForIntent(KnownSerializerIntents intent);

        void MutateSerializerSettings(KnownSerializerFormats format, object settings);
    }
}