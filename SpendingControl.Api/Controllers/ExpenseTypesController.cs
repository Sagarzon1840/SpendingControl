using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;
using SpendingControl.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> GetForUser([FromQuery] Guid userId)
        {
            var list = await _service.GetByUserAsync(userId);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SpendType dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SpendType dto)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();
            existing.Name = dto.Name;
            await _service.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
