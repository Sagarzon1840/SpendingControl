using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using SpendingControl.Application.Interfaces;
using SpendingControl.Api.Helpers;
using SpendingControl.Api.Models;
using SpendingControl.Domain.Entities;
using System.Linq;

namespace SpendingControl.Api.Controllers
{
    [ApiController]
    [Route("budgets")]
    [Authorize]
    public class BudgetsController : ControllerBase
    {
        private readonly IBudgetService _service;
        public BudgetsController(IBudgetService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetForUser([FromQuery] int year, [FromQuery] int month)
        {
            var userId = UserContextHelper.GetUserId(HttpContext);
            var list = await _service.GetByUserAndMonthAsync(userId, year, month);
            var result = list.Select(b => new BudgetResponseDto
            {
                Id = b.Id,
                SpendTypeId = b.SpendTypeId,
                Year = b.Year,
                Month = b.Month,
                Amount = b.Amount
            });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BudgetCreateDto dto)
        {
            var entity = new Budget
            {
                Id = Guid.NewGuid(),
                UserId = UserContextHelper.GetUserId(HttpContext),
                SpendTypeId = dto.SpendTypeId,
                Year = dto.Year,
                Month = dto.Month,
                Amount = dto.Amount
            };
            var created = await _service.CreateAsync(entity);
            var response = new BudgetResponseDto
            {
                Id = created.Id,
                SpendTypeId = created.SpendTypeId,
                Year = created.Year,
                Month = created.Month,
                Amount = created.Amount
            };
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            if (entity.UserId != UserContextHelper.GetUserId(HttpContext)) return Forbid();
            var response = new BudgetResponseDto
            {
                Id = entity.Id,
                SpendTypeId = entity.SpendTypeId,
                Year = entity.Year,
                Month = entity.Month,
                Amount = entity.Amount
            };
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] BudgetPatchDto dto)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();
            if (existing.UserId != UserContextHelper.GetUserId(HttpContext)) return Forbid();

            if (dto.Amount.HasValue)
            {
                existing.Amount = dto.Amount.Value;
            }
            await _service.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();
            if (existing.UserId != UserContextHelper.GetUserId(HttpContext)) return Forbid();
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
