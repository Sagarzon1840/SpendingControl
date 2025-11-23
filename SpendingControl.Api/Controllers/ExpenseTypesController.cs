using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SpendingControl.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SpendingControl.Api.Helpers;
using SpendingControl.Api.Models;
using System.Linq;
using SpendingControl.Domain.Entities;

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
            var userId = UserContextHelper.GetUserId(HttpContext);
            var list = await _service.GetByUserAsync(userId);
            var result = list.Select(s => new SpendTypeResponseDto { Id = s.Id, Code = s.Code, Name = s.Name, IsActive = s.IsActive });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SpendTypeCreateDto dto)
        {
            var entity = new SpendType
            {
                UserId = UserContextHelper.GetUserId(HttpContext),
                Name = dto.Name
            };
            var created = await _service.CreateAsync(entity);
            var response = new SpendTypeResponseDto { Id = created.Id, Code = created.Code, Name = created.Name };
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = UserContextHelper.GetUserId(HttpContext);
            var entity = await _service.GetByIdAsync(id, userId);
            var response = new SpendTypeResponseDto { Id = entity.Id, Code = entity.Code, Name = entity.Name };
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] SpendTypePatchDto dto)
        {
            SpendType spendType = new()
            {
                UserId = UserContextHelper.GetUserId(HttpContext),
                Name = dto.Name,
                IsActive = dto.IsActive
            };
            await _service.UpdateAsync(id, spendType);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = UserContextHelper.GetUserId(HttpContext);
            await _service.DeleteAsync(id, userId);
            return NoContent();
        }
    }
}