using Microsoft.EntityFrameworkCore;
using SpendingControl.Domain.Entities;
using SpendingControl.Infrastructure.Persistence;
using SpendingControl.Domain.Interfaces.Repositories;

namespace SpendingControl.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db) => _db = db;

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task<User?> FindByUserNameAsync(string username)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<User> AddAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return user;
        }
    }
}
