using System;
using System.IO;
using System.IO.Compression;
using NeonTetra.Contracts.Infrastructure.Compression;

namespace NeonTetra.Core.Infrastructure
{
    public class CompressionFactory : ICompressionFactory
    {
        public ICompression GetCompressionForIntent(CompressionIntent intent)
        {
            return new Compression();
        }

        public ICompression GetCompressionForIntent(CompressionType type)
        {
            return new Compression();
        }

        public byte[] Compress(byte[] data, CompressionType type)
        {
            return new Compression().Compress(data, type);
        }

        public byte[] Decompress(byte[] data, CompressionType type)
        {
            return new Compression().Decompress(data, type);
        }

        public byte[] Compress(byte[] data, CompressionIntent intent)
        {
            return new Compression().Compress(data, CompressionType.Gzip);
        }

        public byte[] Decompress(byte[] data, CompressionIntent intent)
        {
            return new Compression().Decompress(data, CompressionType.Gzip);
        }
    }

    public class Compression : ICompression
    {
        public byte[] Compress(byte[] data, CompressionType compressionType)
        {
            return CompressInternal(data, compressionType, CompressionLevel.Optimal);
        }

        public byte[] Compress(byte[] data, CompressionType compressionType, CompressionLevel compressionLevel)
        {
            return CompressInternal(data, compressionType, compressionLevel);
        }

        public byte[] Decompress(byte[] data, CompressionType compressionType)
        {
            return DecompressInternal(data, compressionType);
        }

        private byte[] CompressInternal(byte[] data, CompressionType compressionType,
            CompressionLevel? compressionLevel)
        {
            if (compressionType == CompressionType.None) return data;
            if (data.Length == 0) return new byte[0];
            using (var memStream = new MemoryStream())
            {
                var compressionStream = GetCompressionStream(memStream, compressionType, CompressionMode.Compress,
                    compressionLevel);
                using (compressionStream)
                {
                    compressionStream.Write(data, 0, data.Length);
                }

                return memStream.ToArray();
            }
        }

        private byte[] DecompressInternal(byte[] data, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None) return data;

            if (data.Length == 0) return new byte[0];

            using (var inMemStream = new MemoryStream(data))
            using (var compressionStream =
                GetCompressionStream(inMemStream, compressionType, CompressionMode.Decompress, null))
            using (var outMemStream = new MemoryStream())
            {
                compressionStream.CopyTo(outMemStream);
                return outMemStream.ToArray();
            }
        }

        private Stream GetCompressionStream(Stream innerStream, CompressionType compressionType,
            CompressionMode compressionMode, CompressionLevel? compressionLevel)
        {
            switch (compressionType)
            {
                case CompressionType.Gzip:
                    switch (compressionMode)
                    {
                        case CompressionMode.Compress:
                            return compressionLevel == null
                                ? new GZipStream(innerStream, CompressionMode.Compress)
                                : new GZipStream(innerStream, compressionLevel.Value, true);

                        case CompressionMode.Decompress:
                            return new GZipStream(innerStream, CompressionMode.Decompress, true);

                        default:
                            throw new ArgumentOutOfRangeException(nameof(compressionMode), compressionMode, null);
                    }

                case CompressionType.Deflate:
                    switch (compressionMode)
                    {
                        case CompressionMode.Compress:
                            return compressionLevel == null
                                ? new DeflateStream(innerStream, CompressionMode.Compress)
                                : new DeflateStream(innerStream, compressionLevel.Value, true);

                        case CompressionMode.Decompress:
                            return new DeflateStream(innerStream, CompressionMode.Decompress, true);

                        default:
                            throw new ArgumentOutOfRangeException(nameof(compressionMode), compressionMode, null);
                    }

                default:
                    throw new ArgumentOutOfRangeException(nameof(compressionType), compressionType, null);
            }
        }
    }
}