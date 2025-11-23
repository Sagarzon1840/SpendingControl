using SpendingControl.Domain.Entities;
using System.Threading.Tasks;

namespace SpendingControl.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> UserRegisterAsync(User user, string password);
        Task<User?> UserLoginAsync(string username, string password);
        Task<User?> GetByUsernameAsync(string username);
    }
}
