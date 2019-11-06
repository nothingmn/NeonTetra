namespace NeonTetra.Contracts.Infrastructure
{
    public interface IPacketFormatter
    {
        byte PacketTerminator { get; }

        byte[] RemoveFooter(byte[] data);

        byte[] AddFooter(byte[] data);
    }
}