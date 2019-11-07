using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts.ActorSystem.Messages.Commands;
using NeonTetra.Contracts.Membership;

namespace NeonTetra.Core.Messages.Commands
{
    public class UpdateUserActorStateCommand : IUpdateUserActorStateCommand
    {
        public IUser UpdatedUser { get; set; }
    }
}