using NeonTetra.Contracts.ActorSystem.Messages.Commands;

namespace NeonTetra.Core.ActorSystem.Messages.Commands
{
    /// <summary>
    /// used to generically query any actor for its current state
    /// </summary>
    public class QueryActorStateCommand : Message, IQueryActorStateCommand
    {
    }
}