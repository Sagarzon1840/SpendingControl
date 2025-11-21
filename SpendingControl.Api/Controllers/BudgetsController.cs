using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using SpendingControl.Application.Interfaces;
using SpendingControl.Domain.Entities;

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
        public async Task<IActionResult> GetForUser([FromQuery] Guid userId, [FromQuery] int year, [FromQuery] int month)
        {
            var list = await _service.GetByUserAndMonthAsync(userId, year, month);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Budget dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Budget dto)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();
            existing.Amount = dto.Amount;
            await _service.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
