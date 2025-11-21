using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace SpendingControl.Api.Controllers
{
    [ApiController]
    [Route("movements")]
    [Authorize]
    public class MovementsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid userId, [FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null, [FromQuery] int page = 1, [FromQuery] int size = 50)
        {
            // TODO: implement combining deposits and expenses into MovementViewDTO
            return Ok();
        }
    }
}
