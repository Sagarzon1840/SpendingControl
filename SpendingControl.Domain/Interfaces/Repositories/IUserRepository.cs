using System;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> FindByUserNameAsync(string username);
        Task<User> AddAsync(User user);
    }
}
