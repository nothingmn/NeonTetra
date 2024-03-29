﻿using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts.Membership;

namespace NeonTetra.Contracts.ActorSystem.Messages.Commands
{
    public interface IUpdateUserActorStateCommand : IMessage
    {
        IUser UpdatedUser { get; set; }
    }
}