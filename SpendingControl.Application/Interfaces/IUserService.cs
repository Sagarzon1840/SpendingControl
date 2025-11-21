using SpendingControl.Application.DTOs;
using SpendingControl.Domain.Entities;
using System.Threading.Tasks;

namespace SpendingControl.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> UserRegisterAsync(User user, string password);

        Task<User> UserLoginAsync(UsersLoginDTO user);

    }
}
