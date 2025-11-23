using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SpendingControl.Application.Interfaces;
using SpendingControl.Domain.Entities;
using SpendingControl.Api.Helpers;
using SpendingControl.Api.Models;
using System.Linq;

namespace SpendingControl.Api.Controllers
{
    [ApiController]
    [Route("monetary-funds")]
    [Authorize]
    public class MonetaryFundsController : ControllerBase
    {
        private readonly IMonetaryFundService _service;
        public MonetaryFundsController(IMonetaryFundService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetForUser()
        {
            var userId = UserContextHelper.GetUserId(HttpContext);
            var list = await _service.GetByUserAsync(userId);
            var result = list.Select(f => new MonetaryFundResponseDto
            {
                Id = f.Id,
                Name = f.Name,
                Type = f.Type,
                AccountNumberOrDescription = f.AccountNumberOrDescription,
                InitialBalance = f.InitialBalance,
                CurrentBalance = f.CurrentBalance
            });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MonetaryFundCreateDto dto)
        {
            var userId = UserContextHelper.GetUserId(HttpContext);
            var entity = new MonetaryFund
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = dto.Name,
                Type = dto.Type,
                AccountNumberOrDescription = dto.AccountNumberOrDescription,
                InitialBalance = dto.InitialBalance
            };
            var created = await _service.CreateAsync(entity);
            var response = new MonetaryFundResponseDto
            {
                Id = created.Id,
                Name = created.Name,
                Type = created.Type,
                AccountNumberOrDescription = created.AccountNumberOrDescription,
                InitialBalance = created.InitialBalance,
                CurrentBalance = created.CurrentBalance
            };
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = UserContextHelper.GetUserId(HttpContext);
            var entity = await _service.GetByIdAsync(id, userId);
            var response = new MonetaryFundResponseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Type = entity.Type,
                AccountNumberOrDescription = entity.AccountNumberOrDescription,
                InitialBalance = entity.InitialBalance,
                CurrentBalance = entity.CurrentBalance
            };
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] MonetaryFundPatchDto dto)
        {
            var userId = UserContextHelper.GetUserId(HttpContext);
            // Obtener actual para aplicar cambios parciales
            var current = await _service.GetByIdAsync(id, userId);

            // Aplicar sólo si se envían
            if (dto.Name != null) current.Name = dto.Name;
            if (dto.Type.HasValue) current.Type = dto.Type.Value;
            if (dto.AccountNumberOrDescription != null) current.AccountNumberOrDescription = dto.AccountNumberOrDescription;
            if (dto.InitialBalance.HasValue)
            {
                current.InitialBalance = dto.InitialBalance.Value;

            }

            await _service.UpdateAsync(current, userId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = UserContextHelper.GetUserId(HttpContext);
            await _service.DeleteAsync(id, userId);
            return NoContent();
        }
    }
}
