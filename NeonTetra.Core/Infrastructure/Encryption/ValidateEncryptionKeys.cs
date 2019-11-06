using System;
using System.Linq;
using NeonTetra.Contracts.Infrastructure.Encryption;

namespace NeonTetra.Core.Infrastructure.Encryption
{
    public class ValidateEncryptionKeys : IValidateEncryptionKeys
    {
        private readonly IEncryptionFactory _factory;
        private readonly Random rnd = new Random();

        public ValidateEncryptionKeys(IEncryptionFactory factory)
        {
            _factory = factory;
        }

        public bool Validate(IEncryptionKey key, EncryptionAlgorithm algorithm)
        {
            if (algorithm == EncryptionAlgorithm.Rijndael)
            {
                if (key.IV.Length != 16) return false;
                if (key.Key.Length * 8 < 128 || key.Key.Length * 8 > 263) return false;
            }
            else if (algorithm == EncryptionAlgorithm.Des)
            {
                if (key.IV.Length != 8) return false;
                if (key.Key.Length * 8 < 64 || key.Key.Length * 8 > 71) return false;
            }
            else if (algorithm == EncryptionAlgorithm.Rc2)
            {
                if (key.IV.Length != 8) return false;
                if (key.Key.Length * 8 < 40 || key.Key.Length * 8 > 135) return false;
            }
            else if (algorithm == EncryptionAlgorithm.TripleDes)
            {
                if (key.IV.Length != 8) return false;
                if (key.Key.Length * 8 < 128 || key.Key.Length * 8 > 199) return false;
            }

            var input = new byte[1024];
            rnd.NextBytes(input);

            var encryptor = _factory.EncryptionForAlgorithm(algorithm);
            var output = encryptor.Decrypt(encryptor.Encrypt(input, key), key);

            var same = input.SequenceEqual(output);

            return same;
        }
    }
}