using System;
using System.Collections.Generic;
using System.Text;

namespace NeonTetra.Contracts.Membership
{
    public interface IUser
    {
        string Name { get; set; }
        DateTimeOffset DateOfBirth { get; set; }
        string Id { get; set; }
    }
}