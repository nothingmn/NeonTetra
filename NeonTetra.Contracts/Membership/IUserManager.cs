using System.Threading.Tasks;

namespace NeonTetra.Contracts.Membership
{
    public interface IUserManager
    {
        Task<IUser> Get(string id);

        Task<IUser> Login(string username, string password);

        Task<IUser> Logout(string id);
    }
}