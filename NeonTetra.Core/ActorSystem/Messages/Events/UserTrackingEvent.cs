using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts.ActorSystem.Messages.Events;

namespace NeonTetra.Core.ActorSystem.Messages.Events
{
    public class UserTrackingEvent : Message, IUserTrackingEvent
    {
    }
}