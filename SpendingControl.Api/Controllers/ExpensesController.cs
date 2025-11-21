using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using SpendingControl.Domain.Entities;
using SpendingControl.Domain.Interfaces.Services;

namespace SpendingControl.Api.Controllers
{
    [ApiController]
    [Route("expenses")]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _service;
        public ExpensesController(IExpenseService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SpendingHeader header)
        {
            var (created, overdrawn) = await _service.CreateExpenseAsync(header);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { created, overdrawn });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            // TODO: implement retrieval
            return Ok();
        }
    }
}
