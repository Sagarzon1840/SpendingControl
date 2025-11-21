using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using SpendingControl.Application.Interfaces;
using SpendingControl.Domain.Entities;

namespace SpendingControl.Api.Controllers
{
    [ApiController]
    [Route("deposits")]
    [Authorize]
    public class DepositsController : ControllerBase
    {
        private readonly IDepositService _service;
        public DepositsController(IDepositService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetForUser([FromQuery] Guid userId, [FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            var list = await _service.GetByUserAsync(userId, from, to);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Deposit dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            // Not implemented in service
            return Ok();
        }
    }
}
