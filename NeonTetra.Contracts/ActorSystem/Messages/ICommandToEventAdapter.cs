using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace NeonTetra.Contracts.ActorSystem.Messages
{
    public interface ICommandToEventAdapter
    {
        TOut Adapt<TIn, TOut>(TIn input) where TIn : IMessage where TOut : IMessage;
    }
}