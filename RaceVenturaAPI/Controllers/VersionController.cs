using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RaceVenturaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly ILogger<VersionController> _Logger;
        private readonly IConfiguration _configuration;

        public VersionController(ILogger<VersionController> logger, IConfiguration configuration)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpGet("getversion")]
        public IActionResult GetVersion()
        {
            try
            {
                return Ok("1.0.10.0");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(AccountsController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpGet("getappversion")]
        public IActionResult GetAppVersion()
        {
            try
            {
                return Ok(_configuration.GetValue<string>("AppVersion"));
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(AccountsController)}: {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}