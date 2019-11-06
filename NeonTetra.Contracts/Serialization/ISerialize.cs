namespace NeonTetra.Contracts.Serialization
{
    public interface ISerialize
    {
        string ContentType { get; }

        byte[] Serialize(object entity);

        string SerializeToString(object entity);
    }
}