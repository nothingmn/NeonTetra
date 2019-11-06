using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Infrastructure
{
    public class EndOfTransmissionPacketFormatter : IPacketFormatter
    {
        public byte PacketTerminator { get; } = 10;

        /// <summary>
        ///     Remove the standard 4 (end of transmission) byte to the payload
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] RemoveFooter(byte[] data)
        {
            if (data[data.Length - 1] != PacketTerminator) return data;

            var bytes = new byte[data.Length - 1];
            for (var x = 0; x < data.Length - 1; x++) bytes[x] = data[x];
            data = null;
            return bytes;
        }

        /// <summary>
        ///     Add a standard 4 (end of transmission) byte to the payload
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] AddFooter(byte[] data)
        {
            if (data[data.Length - 1] == PacketTerminator) return data;

            var bytes = new byte[data.Length + 1];
            data.CopyTo(bytes, 0);
            bytes[data.Length] = PacketTerminator;
            data = null;
            return bytes;
        }
    }
}