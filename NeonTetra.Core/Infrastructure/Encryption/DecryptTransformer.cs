using System.Security.Cryptography;
using NeonTetra.Contracts.Infrastructure.Encryption;

namespace NeonTetra.Core.Infrastructure.Encryption
{
    public class DecryptTransformer
    {
        private readonly EncryptionAlgorithm algorithmID;

        private byte[] initVec;

        internal DecryptTransformer(EncryptionAlgorithm deCryptId)
        {
            algorithmID = deCryptId;
        }

        internal byte[] IV
        {
            set => initVec = value;
        }

        internal ICryptoTransform GetCryptoServiceProvider(byte[] bytesKey)
        {
            ICryptoTransform iCryptoTransform;

            switch (algorithmID)
            {
                case EncryptionAlgorithm.Des:
                    using (DES dES = new DESCryptoServiceProvider())
                    {
                        dES.Mode = CipherMode.CBC;
                        dES.Key = bytesKey;
                        dES.IV = initVec;
                        iCryptoTransform = dES.CreateDecryptor();
                    }

                    break;

                case EncryptionAlgorithm.TripleDes:
                    using (TripleDES tripleDES = new TripleDESCryptoServiceProvider())
                    {
                        tripleDES.Mode = CipherMode.CBC;
                        iCryptoTransform = tripleDES.CreateDecryptor(bytesKey, initVec);
                    }

                    break;

                case EncryptionAlgorithm.Rc2:
                    using (RC2 rC2 = new RC2CryptoServiceProvider())
                    {
                        rC2.Mode = CipherMode.CBC;
                        iCryptoTransform = rC2.CreateDecryptor(bytesKey, initVec);
                    }

                    break;

                case EncryptionAlgorithm.Rijndael:
                    using (Rijndael rijndael = new RijndaelManaged())
                    {
                        rijndael.Mode = CipherMode.CBC;
                        iCryptoTransform = rijndael.CreateDecryptor(bytesKey, initVec);
                    }

                    break;

                default:
                    throw new CryptographicException(string.Concat("Algorithm ID \'", algorithmID,
                        "\' not supported."));
            }

            return iCryptoTransform;
        }
    }
}