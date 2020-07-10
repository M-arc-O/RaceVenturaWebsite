using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using RaceVenturaAPI.ViewModels.Identity;
using RaceVenturaAPI.Helpers;
using RaceVentura;
using RaceVenturaData.Models.Identity;
using RaceVenturaAPI.ViewModels;
using Microsoft.Extensions.Logging;

namespace RaceVenturaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountBL _AccountBL;
        private readonly IMapper _Mapper;
        private readonly ILogger<AccountsController> _Logger;

        public AccountsController(IAccountBL accountBL, IMapper mapper, ILogger<AccountsController> logger)
        {
            _Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _AccountBL = accountBL ?? throw new ArgumentNullException(nameof(accountBL));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userIdentity = _Mapper.Map<AppUser>(viewModel);

                var result = await _AccountBL.CreateUser(userIdentity, viewModel.Password);

                if (!result.Succeeded)
                {
                    return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
                }

                return new OkResult();
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(AccountsController)}: {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}