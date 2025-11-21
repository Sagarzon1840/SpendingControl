using SpendingControl.Application.DTOs;
using SpendingControl.Application.Interfaces;
using SpendingControl.Domain.Entities;
using SpendingControl.Domain.Interfaces.Repositories;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpendingControl.Application.UseCases
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> UserRegisterAsync(User user, string password)
        {
            // Generate salt and hash
            using var hmac = new HMACSHA512();
            user.PasswordSalt = hmac.Key;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            user.DateRegistered = DateTime.UtcNow;

            var result = await _userRepository.AddAsync(user);

            return result != null;
        }

        public async Task<User?> UserLoginAsync(UsersLoginDTO login)
        {
            User? user = await _userRepository.FindByUserNameAsync(login.Username);

            if (user is null) return null;

            if (user.PasswordHash == null || user.PasswordSalt == null) return null;

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

            for (int i = 0; i < computed.Length; i++)
            {
                if (computed[i] != user.PasswordHash[i]) return null;
            }

            return user;
        }
    }
}
