using System.Threading.Tasks;

namespace NeonTetra.Contracts.Membership
{
    public interface IUserManager
    {
        Task<IUser> Get(string id);
    }
}