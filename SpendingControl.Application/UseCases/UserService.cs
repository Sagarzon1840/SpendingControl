using SpendingControl.Application.Interfaces;
using SpendingControl.Domain.Entities;
using SpendingControl.Domain.Interfaces.Repositories;
using System.Security.Cryptography;
using System.Text;

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
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.UserName = user.UserName?.Trim();
            if (string.IsNullOrWhiteSpace(user.UserName)) throw new ArgumentException("Username is required", nameof(user.UserName));
            if (string.IsNullOrWhiteSpace(user.Name)) throw new ArgumentException("Name is required", nameof(user.Name));

            ValidatePassword(password);

            // uniqueness for username
            var existing = await _userRepository.FindByUserNameAsync(user.UserName);
            if (existing != null) throw new InvalidOperationException("Username already exists");

            using var hmac = new HMACSHA512();
            user.PasswordSalt = hmac.Key;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            user.DateRegistered = DateTime.UtcNow;
            user.IsActive = true;

            var result = await _userRepository.AddAsync(user);
            return result != null;
        }

        public async Task<User?> UserLoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required", nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password is required", nameof(password));

            var user = await _userRepository.FindByUserNameAsync(username.Trim());
            if (user == null || !user.IsActive) return null;
            if (user.PasswordHash == null || user.PasswordSalt == null) return null;

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computed.Length; i++)
            {
                if (computed[i] != user.PasswordHash[i]) return null;
            }

            return user;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;
            return await _userRepository.FindByUserNameAsync(username.Trim());
        }

        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password is required", nameof(password));
            if (password.Length < 8) throw new ArgumentException("Password must be at least 8 characters", nameof(password));
            // Basic strength check (optional): at least one letter and one digit
            if (!HasLetter(password) || !HasDigit(password)) throw new ArgumentException("Password must contain letters and digits", nameof(password));
        }

        private static bool HasLetter(string s) => s.Any(char.IsLetter);
        private static bool HasDigit(string s) => s.Any(char.IsDigit);
    }
}
