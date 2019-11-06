namespace NeonTetra.Contracts.Infrastructure.Compression
{
    public interface ICompressionFactory
    {
        ICompression GetCompressionForIntent(CompressionIntent intent);

        ICompression GetCompressionForIntent(CompressionType type);

        byte[] Compress(byte[] data, CompressionType type);

        byte[] Decompress(byte[] data, CompressionType type);

        byte[] Compress(byte[] data, CompressionIntent intent);

        byte[] Decompress(byte[] data, CompressionIntent intent);
    }
}