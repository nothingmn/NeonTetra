using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Services;

namespace NeonTetra.Core.Services
{
    public class CoreRandomProvider : IRandomProvider
    {
        private readonly IConfiguration _configuration;

        public CoreRandomProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public byte[] Generate(RandomType type = RandomType.InSecure, int length = 100)
        {
            var result = new byte[length];

            if (type == RandomType.InSecure)
            {
                var rnd = new Random();
                rnd.NextBytes(result);
            }
            else
            {
                var rngName = _configuration.GetValueOrDefault("Security:RandomNumberGenerator", "RandomNumberGenerator");
                var rnd = RandomNumberGenerator.Create(rngName);
                rnd.GetBytes(result);
            }

            return result;
        }

        public string GenerateString(RandomType type = RandomType.InSecure, int length = 100)
        {
            return Convert.ToBase64String(Generate(type, length));
        }

        public double GenerateDouble(RandomType type = RandomType.InSecure, int length = 100)
        {
            return BitConverter.ToDouble(Generate(type, length), 0);
        }

        public int GenerateInt(RandomType type = RandomType.InSecure, int length = 100)
        {
            return BitConverter.ToInt32(Generate(type, length), 0);
        }
    }
}