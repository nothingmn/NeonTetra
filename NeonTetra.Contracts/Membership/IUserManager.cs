using System.Threading.Tasks;

namespace NeonTetra.Contracts.Membership
{
    /// <summary>
    /// CRUD USERS
    /// </summary>
    public interface IUserManager
    {
        Task<IUser> Get(string id);
    }

    /// <summary>
    /// Authentication and Authorization
    /// </summary>
    public interface IAccountManager
    {
        Task<IUser> Login(string username, string password);

        Task<IUser> Logout(string id);

        Task<IUser> RegisterUser(IUser user);
    }
}