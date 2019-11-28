using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Contracts.Membership;
using System.Linq;
using NeonTetra.Contracts.Services;

namespace NeonTetra.Core.Services
{
    public class CoreAccountManager : IAccountManager
    {
        private readonly IHash _hash;
        private readonly IRandomProvider _randomProvider;
        private static readonly IList<IUser> _users = new List<IUser>();

        public CoreAccountManager(IHash hash, IRandomProvider randomProvider)
        {
            _hash = hash;
            _randomProvider = randomProvider;
        }

        private Tuple<bool, IUser> ValidatePassword(string username, string password)
        {
            //cant have null usernames or passwords
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return new Tuple<bool, IUser>(false, default);

            //user must exist in our database with that username.
            var user = (from u in _users where u.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase) select u)?.FirstOrDefault();
            if (user == null) return new Tuple<bool, IUser>(false, default);

            //the user must have a password
            if (string.IsNullOrEmpty(user.Password)) return new Tuple<bool, IUser>(false, default);

            //hash our incoming password
            var hashedPassword = System.Convert.ToBase64String(
                _hash.Hash(System.Text.Encoding.UTF8.GetBytes(password), HashProviders.UserAccountSecurity)
                );

            //the user password is hashed and has a salt appended to it.
            //so the incoming password should be the first set of characters on the users password in our repo
            //so we use .startswith to check.
            if (user.Password.StartsWith(hashedPassword, StringComparison.InvariantCultureIgnoreCase)) return new Tuple<bool, IUser>(true, user);
            //strange our only success path is here.  this member could do with a refactoring.

            //all else fails dump out a negative result.
            return new Tuple<bool, IUser>(false, default);
        }

        public Task<IUser> Login(string username, string password)
        {
            var passwordResults = ValidatePassword(username, password);
            return Task.FromResult(passwordResults.Item2);
        }

        public Task<IUser> Logout(string id)
        {
            return Task.FromResult(default(IUser));
        }

        public Task<IUser> RegisterUser(IUser user)
        {
            //this incoming user's password will need to be salted and hashed, then stored in our repo.

            var hashedPassword = System.Convert.ToBase64String(_hash.Hash(System.Text.Encoding.UTF8.GetBytes(user.Password), HashProviders.UserAccountSecurity));
            var salt = _randomProvider.GenerateString(RandomType.Secure);
            user.Password = hashedPassword + salt;
            _users.Add(user);
            return Task.FromResult(user);
        }
    }
}