using KsiazeczkaPttk.Domain.Models;

namespace KsiazeczkaPTTK.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);

        Task<User> AddUser(User user);

        Task<UserRole> GetRole(string roleName);
    }
}
