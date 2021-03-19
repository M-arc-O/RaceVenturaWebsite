using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using RaceVenturaAPI.Auth;
using RaceVenturaAPI.Helpers;
using RaceVenturaAPI.ViewModels.Identity;
using RaceVentura;
using System;
using Microsoft.Extensions.Logging;
using RaceVenturaAPI.ViewModels;

namespace RaceVenturaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountBL _AccountBL;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ILogger _Logger;

        public AuthController(IAccountBL accountBL, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, ILogger<AuthController> logger)
        {
            _AccountBL = accountBL;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _Logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]CredentialsViewModel credentials)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var identity = await GetClaimsIdentity(credentials.Email, credentials.Password);
                if (identity == null)
                {
                    return BadRequest(ErrorCodes.UserUnauthorized);
                }

                var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.Email, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
                return new OkObjectResult(jwt);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _AccountBL.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _AccountBL.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}