using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SpendingControl.Domain.Entities;
using SpendingControl.Api.Helpers;
using SpendingControl.Application.Interfaces;
using SpendingControl.Api.Models;
using System.Linq;

namespace SpendingControl.Api.Controllers
{
    [ApiController]
    [Route("expenses")]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _service;
        public ExpensesController(IExpenseService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null, [FromQuery] int page = 1, [FromQuery] int size = 50)
        {
            var userId = UserContextHelper.GetUserId(HttpContext);
            var items = await _service.GetListAsync(userId, from, to, page, size);
            var result = items.Select(h => new SpendingHeaderResponseDto
            {
                Id = h.Id,
                MonetaryFundId = h.MonetaryFundId,
                Date = h.Date,
                MerchantName = h.MerchantName,
                Observations = h.Observations,
                DocumentType = h.DocumentType,
                Details = h.Details.Select(d => new SpendingDetailResponseDto
                {
                    Id = d.Id,
                    ExpenseTypeId = d.ExpenseTypeId,
                    Amount = d.Amount,
                    Description = d.Description
                })
            });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SpendingHeaderCreateDto dto)
        {
            var userId = UserContextHelper.GetUserId(HttpContext);
            var header = new SpendingHeader
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Date = dto.Date,
                MonetaryFundId = dto.MonetaryFundId,
                MerchantName = dto.MerchantName,
                Observations = dto.Observations,
                DocumentType = dto.DocumentType,
                Details = dto.Details.Select(d => new SpendingDetail
                {
                    Id = Guid.NewGuid(),
                    ExpenseTypeId = d.ExpenseTypeId,
                    Amount = d.Amount,
                    Description = d.Description
                }).ToList()
            };
            var (created, warnings) = await _service.CreateExpenseAsync(header);
            var response = new SpendingHeaderResponseDto
            {
                Id = created.Id,
                MonetaryFundId = created.MonetaryFundId,
                Date = created.Date,
                MerchantName = created.MerchantName,
                Observations = created.Observations,
                DocumentType = created.DocumentType,
                Details = created.Details.Select(d => new SpendingDetailResponseDto
                {
                    Id = d.Id,
                    ExpenseTypeId = d.ExpenseTypeId,
                    Amount = d.Amount,
                    Description = d.Description
                }),
                OverdraftWarnings = warnings.Select(w => new OverdraftWarningDto
                {
                    ExpenseTypeName = w.ExpenseTypeName,
                    Budget = w.Budget,
                    Executed = w.Executed,
                    Overdraft = w.Overdraft
                })
            };
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = UserContextHelper.GetUserId(HttpContext);
            var entity = await _service.GetByIdAsync(id, userId);
            if (entity == null) return NotFound();
            var response = new SpendingHeaderResponseDto
            {
                Id = entity.Id,
                MonetaryFundId = entity.MonetaryFundId,
                Date = entity.Date,
                MerchantName = entity.MerchantName,
                Observations = entity.Observations,
                DocumentType = entity.DocumentType,
                Details = entity.Details.Select(d => new SpendingDetailResponseDto
                {
                    Id = d.Id,
                    ExpenseTypeId = d.ExpenseTypeId,
                    Amount = d.Amount,
                    Description = d.Description
                })
            };
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var userId = UserContextHelper.GetUserId(HttpContext);
            var exists = await _service.GetByIdAsync(id, userId);
            if (exists == null) return NotFound();
            var success = await _service.SoftDeleteAsync(id, userId);
            if (!success) return BadRequest();
            return NoContent();
        }
    }
}
