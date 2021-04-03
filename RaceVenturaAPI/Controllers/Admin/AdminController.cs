using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RaceVenturaAPI.Controllers.Admin
{
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet]
        [Route("test")]
        public IActionResult Get()
        {
            return Ok("Admin controller test");
        }
    }
}
