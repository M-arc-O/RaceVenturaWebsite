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
using Microsoft.AspNetCore.Authorization;

namespace RaceVenturaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "RaceUser")]
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
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount([FromBody]RegistrationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userIdentity = _Mapper.Map<AppUser>(viewModel);

                var result = await _AccountBL.CreateUser(viewModel.RegistrationSecret, userIdentity, viewModel.Password);

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
        [Route("confirmemail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromBody]ConfirmEmailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _AccountBL.ConfirmEmail(viewModel.EmailAddress, viewModel.Code);

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
        [AllowAnonymous]
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
        [AllowAnonymous]
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