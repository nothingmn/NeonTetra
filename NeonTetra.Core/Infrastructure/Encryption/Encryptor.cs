using System;
using System.IO;
using System.Security.Cryptography;
using NeonTetra.Contracts.Infrastructure.Encryption;

namespace NeonTetra.Core.Infrastructure.Encryption
{
    public class Encryptor
    {
        private readonly EncryptTransformer transformer;

        public Encryptor(EncryptionAlgorithm algId)
        {
            transformer = new EncryptTransformer(algId);
        }

        public byte[] IV { get; set; }

        public byte[] Key { get; private set; }

        public byte[] Encrypt(byte[] bytesData, IEncryptionKey key = null)
        {
            using (var memoryStream = new MemoryStream())
            {
                if (key?.IV != null)
                    transformer.IV = key.IV;
                else
                    transformer.IV = IV;
                using (var iCryptoTransform = transformer.GetCryptoServiceProvider(key.Key))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, iCryptoTransform, CryptoStreamMode.Write))
                    {
                        try
                        {
                            cryptoStream.Write(bytesData, 0, bytesData.Length);
                        }
                        catch (Exception e)
                        {
                            throw new Exception(
                                string.Concat("Error while writing encrypted data to the stream: \n", e.Message));
                        }

                        Key = transformer.Key;
                        if (key.IV != null)
                            IV = transformer.IV;
                        else
                            IV = transformer.IV;
                        cryptoStream.FlushFinalBlock();
                        cryptoStream.Close();
                    }

                    return memoryStream.ToArray();
                }
            }
        }
    }
}