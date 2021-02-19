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

        [HttpPost()]
        [Route("createaccount")]
        public async Task<IActionResult> CreateAccount([FromBody]RegistrationViewModel viewModel)
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

                return Ok();
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

        [HttpPost()]
        [Route("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _AccountBL.ForgotPassword(viewModel.EmailAddress);
                return Ok();
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

        [HttpPost()]
        [Route("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _AccountBL.ResetPassword(viewModel.EmailAddress, viewModel.Password, viewModel.Code);

                if (!result.Succeeded)
                {
                    return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
                }

                return Ok();
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