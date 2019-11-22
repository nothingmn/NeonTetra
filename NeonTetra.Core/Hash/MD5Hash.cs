using System.Text;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Hash
{
    public class MD5Hash : IHashProvider
    {
        private readonly System.Text.Encoding _encoding = System.Text.Encoding.UTF8;

        public byte[] Hash(byte[] input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                return md5.ComputeHash(input);
            }
        }

        public string Hash(string input)
        {
            var bytes = _encoding.GetBytes(input);
            var hash = Hash(bytes);

            var sb = new StringBuilder();
            foreach (var number in hash)
            {
                sb.Append(number.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}