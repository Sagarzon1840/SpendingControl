using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;
using SpendingControl.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace SpendingControl.Api.Controllers
{
    [ApiController]
    [Route("expense-types")]
    [Authorize]
    public class ExpenseTypesController : ControllerBase
    {
        private readonly ISpendTypeService _service;
        public ExpenseTypesController(ISpendTypeService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetForUser()
        {
            var userId = GetUserIdFromToken();
            var list = await _service.GetByUserAsync(userId);

            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SpendType dto)
        {
            dto.UserId = GetUserIdFromToken();
            var created = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetUserIdFromToken();
            var entity = await _service.GetByIdAsync(id, userId);

            return Ok(entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SpendType dto)
        {
            var userId = GetUserIdFromToken();
            dto.Id = id;

            await _service.UpdateAsync(dto, userId);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id, GetUserIdFromToken());

            return NoContent();
        }

        private Guid GetUserIdFromToken()
        {
            var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(sub) || !Guid.TryParse(sub, out var userId))
                throw new UnauthorizedAccessException("Invalid user id in token");

            return userId;
        }
    }
}