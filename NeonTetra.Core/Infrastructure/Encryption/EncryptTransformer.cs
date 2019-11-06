using System.Security.Cryptography;
using NeonTetra.Contracts.Infrastructure.Encryption;

namespace NeonTetra.Core.Infrastructure.Encryption
{
    internal class EncryptTransformer
    {
        private readonly EncryptionAlgorithm algorithmID;

        internal EncryptTransformer(EncryptionAlgorithm algId)
        {
            algorithmID = algId;
        }

        internal byte[] IV { get; set; }

        internal byte[] Key { get; private set; }

        internal ICryptoTransform GetCryptoServiceProvider(byte[] bytesKey)
        {
            ICryptoTransform iCryptoTransform;

            switch (algorithmID)
            {
                case EncryptionAlgorithm.Des:
                    using (DES dES = new DESCryptoServiceProvider())
                    {
                        dES.Mode = CipherMode.CBC;
                        if (bytesKey == null)
                        {
                            Key = dES.Key;
                        }
                        else
                        {
                            dES.Key = bytesKey;
                            Key = dES.Key;
                        }

                        if (IV == null)
                            IV = dES.IV;
                        else
                            dES.IV = IV;
                        iCryptoTransform = dES.CreateEncryptor();
                    }

                    break;

                case EncryptionAlgorithm.TripleDes:
                    using (TripleDES tripleDES = new TripleDESCryptoServiceProvider())
                    {
                        tripleDES.Mode = CipherMode.CBC;
                        if (bytesKey == null)
                        {
                            Key = tripleDES.Key;
                        }
                        else
                        {
                            tripleDES.Key = bytesKey;
                            Key = tripleDES.Key;
                        }

                        if (IV == null)
                            IV = tripleDES.IV;
                        else
                            tripleDES.IV = IV;
                        iCryptoTransform = tripleDES.CreateEncryptor();
                    }

                    break;

                case EncryptionAlgorithm.Rc2:
                    RC2 rC2 = new RC2CryptoServiceProvider();
                    rC2.Mode = CipherMode.CBC;
                    if (bytesKey == null)
                    {
                        Key = rC2.Key;
                    }
                    else
                    {
                        rC2.Key = bytesKey;
                        Key = rC2.Key;
                    }

                    if (IV == null)
                        IV = rC2.IV;
                    else
                        rC2.IV = IV;
                    iCryptoTransform = rC2.CreateEncryptor();
                    break;

                case EncryptionAlgorithm.Rijndael:
                    using (Rijndael rijndael = new RijndaelManaged())
                    {
                        rijndael.Mode = CipherMode.CBC;
                        if (bytesKey == null)
                        {
                            Key = rijndael.Key;
                        }
                        else
                        {
                            rijndael.Key = bytesKey;
                            Key = rijndael.Key;
                        }

                        if (IV == null)
                            IV = rijndael.IV;
                        else
                            rijndael.IV = IV;
                        iCryptoTransform = rijndael.CreateEncryptor();
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