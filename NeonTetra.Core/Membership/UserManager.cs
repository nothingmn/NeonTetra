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
        private readonly IActorManager _actorManager;
        private readonly IResolve _resolver;

        public UserManager(IActorManager actorManager, IResolve resolver)
        {
            _actorManager = actorManager;
            _resolver = resolver;
        }

        public async Task<IUser> Get(string id)
        {
            var user = await GetOrCreateActor<IUserActor>(id);
            if (user != null)
            {
                var request = _resolver.Resolve<IQueryActorStateCommand>();
                return await user.Ask<IUser>(request);
            }
            return null;
        }

        private string FormatActorName(string id)
        {
            return $"user-{id}";
        }

        private async Task<INeonActor> GetOrCreateActor<T>(string id)
        {
            var user = await _actorManager.GetByPath<T>(FormatActorName(id));
            if (user == null)
            {
                user = _actorManager.Create<IUserActor>(FormatActorName(id));
            }

            return user;
        }

        public Task Create(IUser user)
        {
            return null;
        }
    }
}