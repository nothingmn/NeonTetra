using System;
using NeonTetra.Contracts.ActorSystem.Messages.Commands;
using NeonTetra.Contracts.Membership;

namespace NeonTetra.Core.ActorSystem.Messages.Commands
{
    public class UpdateUserActorStateCommand : Message, IUpdateUserActorStateCommand
    {
        public IUser UpdatedUser { get; set; }
    }
}