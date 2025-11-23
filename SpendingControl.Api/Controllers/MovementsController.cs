using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SpendingControl.Api.Helpers;
using SpendingControl.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace SpendingControl.Api.Controllers
{
    [ApiController]
    [Route("movements")]
    [Authorize]
    public class MovementsController : ControllerBase
    {
        private readonly IMovementService _movementService;
        public MovementsController(IMovementService movementService)
        {
            _movementService = movementService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null, [FromQuery] int page = 1, [FromQuery] int size = 50)
        {
            var userId = UserContextHelper.GetUserId(HttpContext);
            var movements = await _movementService.GetMovementsAsync(userId, from, to, page, size);
            return Ok(movements);
        }
    }
}
