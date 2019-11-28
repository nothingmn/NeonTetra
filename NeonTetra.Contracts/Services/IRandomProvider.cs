using System;
using System.Collections.Generic;
using System.Text;

namespace NeonTetra.Contracts.Services
{
    public enum RandomType
    {
        Secure,
        InSecure
    }

    public interface IRandomProvider
    {
        byte[] Generate(RandomType type = RandomType.InSecure, int length = 100);

        string GenerateString(RandomType type = RandomType.InSecure, int length = 100);

        double GenerateDouble(RandomType type = RandomType.InSecure, int length = 100);

        int GenerateInt(RandomType type = RandomType.InSecure, int length = 100);
    }
}