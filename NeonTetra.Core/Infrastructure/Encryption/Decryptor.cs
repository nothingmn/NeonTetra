using System;
using System.IO;
using System.Security.Cryptography;
using NeonTetra.Contracts.Infrastructure.Encryption;

namespace NeonTetra.Core.Infrastructure.Encryption
{
    public class Decryptor
    {
        private byte[] initVec;
        private readonly DecryptTransformer transformer;

        public Decryptor(EncryptionAlgorithm algId)
        {
            transformer = new DecryptTransformer(algId);
        }

        public byte[] IV
        {
            set => initVec = value;
        }

        public byte[] Decrypt(byte[] bytesData, IEncryptionKey key = null)
        {
            using (var memoryStream = new MemoryStream())
            {
                if (key.IV == null)
                    transformer.IV = initVec;
                else
                    transformer.IV = key.IV;
                using (var iCryptoTransform = transformer.GetCryptoServiceProvider(key.Key))
                {
                    using (var cryptoStream =
                        new CryptoStream(memoryStream, iCryptoTransform, CryptoStreamMode.Write))
                    {
                        try
                        {
                            cryptoStream.Write(bytesData, 0, bytesData.Length);
                            cryptoStream.FlushFinalBlock();
                            cryptoStream.Close();
                            var bs = memoryStream.ToArray();
                            return bs;
                        }
                        catch (Exception e)
                        {
                            throw new Exception(
                                string.Concat("Error while writing encrypted data to the stream: \n", e.Message));
                        }
                    }
                }
            }
        }
    }
}