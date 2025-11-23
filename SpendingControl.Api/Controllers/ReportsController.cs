using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using SpendingControl.Api.Helpers;
using SpendingControl.Application.Interfaces;

namespace SpendingControl.Api.Controllers
{
    [ApiController]
    [Route("reports")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("budget-vs-execution")]
        public async Task<IActionResult> BudgetVsExecution([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            if (!from.HasValue || !to.HasValue) return BadRequest("from and to are required");
            var userId = UserContextHelper.GetUserId(HttpContext);
            try
            {
                var data = await _reportService.GetBudgetVsExecutionAsync(userId, from.Value, to.Value);
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
