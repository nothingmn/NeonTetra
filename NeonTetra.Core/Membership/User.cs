using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts.Membership;

namespace NeonTetra.Core.Membership
{
    public class User : IUser
    {
        public string Name { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Id { get; set; }
    }
}