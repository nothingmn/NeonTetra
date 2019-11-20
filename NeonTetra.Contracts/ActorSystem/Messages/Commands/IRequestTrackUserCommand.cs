using System;
using System.Collections.Generic;
using System.Text;

namespace NeonTetra.Contracts.ActorSystem.Messages.Commands
{
    public interface IRequestTrackUserCommand : IMessage
    {
        string UserId { get; set; }
    }

    public interface IRequestUserLoginCommand : IMessage
    {
        string Username { get; set; }
        string Password { get; set; }
    }

    public interface IRespondUserLoginEvent : IMessage
    {
        bool Success { get; set; }
        string Message { get; set; }
    }
}