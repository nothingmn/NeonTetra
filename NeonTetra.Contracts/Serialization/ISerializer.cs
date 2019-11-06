namespace NeonTetra.Contracts.Serialization
{
    public interface ISerializer : ISerialize, IDeserialize
    {
#pragma warning disable 108, 114
        string ContentType { get; }
#pragma warning restore 108,114

        KnownSerializerFormats SerializerFormat { get; }

        void MutateSerializerSettings(object settings);
    }
}