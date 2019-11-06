using System.IO.Compression;

namespace NeonTetra.Contracts.Infrastructure.Compression
{
    public interface ICompression
    {
        byte[] Compress(byte[] data, CompressionType compressionType);

        byte[] Compress(byte[] data, CompressionType compressionType, CompressionLevel compressionLevel);

        byte[] Decompress(byte[] data, CompressionType compressionType);
    }
}