using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using SpendingControl.Application.Interfaces;
using SpendingControl.Domain.Entities;
using SpendingControl.Api.Helpers;
using SpendingControl.Api.Models;

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
        public async Task<IActionResult> GetForUser([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            var userId = UserContextHelper.GetUserId(HttpContext);
            var list = await _service.GetByUserAsync(userId, from, to);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepositCreateDto dto)
        {
            if (dto == null) return BadRequest();
            var entity = new Deposit
            {
                Id = Guid.NewGuid(),
                FundId = dto.FundId,
                Date = dto.Date == default ? DateTime.UtcNow : dto.Date,
                Amount = dto.Amount,
                Description = dto.Description
            };
            var created = await _service.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(); // TODO: implement lookup constrained by user
        }
    }
}
