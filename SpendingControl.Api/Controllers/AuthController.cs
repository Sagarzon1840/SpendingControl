using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SpendingControl.Application.DTOs;
using SpendingControl.Application.Interfaces;
using SpendingControl.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;

namespace SpendingControl.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;

        public AuthController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO dto)
        {
            var user = new User { UserName = dto.Username, Name = dto.Name };
            var ok = await _userService.UserRegisterAsync(user, dto.Password);

            if (!ok) return BadRequest();

            return Ok(new { user.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsersLoginDTO dto)
        {
            var user = await _userService.UserLoginAsync(dto);

            if (user is null) return Unauthorized();

            var token = GenerateToken(user);

            return Ok(new { token });
        }

        private string GenerateToken(User user)
        {
            var jwt = _config.GetSection("Jwt");
            var key = jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key not configured");
            var issuer = jwt["Issuer"] ?? "spendingcontrol";
            var audience = jwt["Audience"] ?? "spendingcontrol";

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.UtcNow.AddHours(8), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
