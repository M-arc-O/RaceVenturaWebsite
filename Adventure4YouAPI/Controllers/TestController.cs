using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Adventure4YouAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public TestController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            //var appSettingTest1 = configuration["JwtSecret"];
            return Ok($"Ok");
        }
    }
}