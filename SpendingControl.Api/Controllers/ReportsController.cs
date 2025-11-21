using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace SpendingControl.Api.Controllers
{
    [ApiController]
    [Route("reports")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        [HttpGet("budget-vs-execution")]
        public async Task<IActionResult> BudgetVsExecution([FromQuery] Guid userId, [FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            // TODO: implement report generation
            return Ok();
        }
    }
}
