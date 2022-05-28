using KsiazeczkaPttk.DAL;
using KsiazeczkaPttk.Domain.Models;
using KsiazeczkaPTTK.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KsiazeczkaPTTK.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TouristsBookContext _context;

        public UserRepository(TouristsBookContext context)
        {
            _context = context;
        }

        public async Task<User> AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserRole> GetRole(string roleName)
        {
            return await _context.UserRoles.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(r => r.Email == email);
        }
    }
}
