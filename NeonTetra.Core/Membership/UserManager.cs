using System;
using System.Threading.Tasks;
using NeonTetra.Contracts;
using NeonTetra.Contracts.ActorSystem;
using NeonTetra.Contracts.ActorSystem.Actors;
using NeonTetra.Contracts.ActorSystem.Messages.Commands;
using NeonTetra.Contracts.Membership;

namespace NeonTetra.Core.Membership
{
    public class UserManager : IUserManager
    {
        private readonly IUserManagerActor _actorManager;

        public UserManager(IUserManagerActor actorManager)
        {
            _actorManager = actorManager;
        }

        public async Task<IUser> Get(string id)
        {
        }

        private string FormatActorName(string id)
        {
            return $"user-{id}";
        }
    }
}