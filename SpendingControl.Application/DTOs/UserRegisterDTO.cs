using System.ComponentModel.DataAnnotations;

namespace SpendingControl.Application.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
