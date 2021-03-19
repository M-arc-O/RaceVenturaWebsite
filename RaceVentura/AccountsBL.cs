using RaceVenturaData.DatabaseContext;
using RaceVenturaData.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using RaceVentura.Models;
using Microsoft.Extensions.Configuration;

namespace RaceVentura
{
    public class AccountsBL : IAccountBL
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public AccountsBL(UserManager<AppUser> userManager, IRaceVenturaDbContext context, IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IdentityResult> CreateUser(string registrationSecret, AppUser userIdentity, string password)
        {
            if (!registrationSecret.Equals(_configuration.GetValue<string>("RegistrationSecret")))
            {
                throw new BusinessException($"Wrong registration secret '{registrationSecret}'", BLErrorCodes.UserUnauthorized);
            }

            var result = await _userManager.CreateAsync(userIdentity, password);

            if (result.Succeeded)
            {
                await SendConfirmEmail(userIdentity, "Confirm your email");
            }

            return result;
        }

        public async Task ConfirmEmail(string emailAddress, string code)
        {
            var user = await _userManager.FindByEmailAsync(emailAddress);
            if (user == null)
            {
                throw new BusinessException($"Could not find user with email address '{emailAddress}'", BLErrorCodes.NotFound);
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
            {
                throw new BusinessException($"Error while confirming email address '{emailAddress}'", BLErrorCodes.UserUnauthorized);
            }
        }

        public Task<AppUser> FindByNameAsync(string userName)
        {
            return _userManager.FindByNameAsync(userName);
        }

        public async Task<bool> CheckPasswordAsync(AppUser userToVerify, string password)
        {
            if (userToVerify == null || !(await _userManager.IsEmailConfirmedAsync(userToVerify)))
            {
                await SendConfirmEmail(userToVerify, "Login attempt, Confirm your email");
                throw new BusinessException("Email address not confirmed.", BLErrorCodes.EmailNotConfirmed);
            }

            return await _userManager.CheckPasswordAsync(userToVerify, password);
        }

        public async Task ForgotPassword(string emailAddress)
        {
            var user = await _userManager.FindByEmailAsync(emailAddress);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                throw new BusinessException("Email address not confirmed.", BLErrorCodes.EmailNotConfirmed);
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var email = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Email));

            var url = $"{_configuration.GetValue<string>("WebsiteUrl")}/resetpassword/{code}/{email}";

            await _emailSender.SendEmailAsync(
                emailAddress,
                "Reset Password",
                $"Please reset your password by <a href='{url}'>clicking here</a>.");
        }

        public async Task<IdentityResult> ResetPassword(string emailAddress, string password, string code)
        {
            var email = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(emailAddress));

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return IdentityResult.Success;
            }

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ResetPasswordAsync(user, token, password);

            return result;
        }

        private async Task SendConfirmEmail(AppUser userIdentity, string title)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(userIdentity);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var url = $"{_configuration.GetValue<string>("WebsiteUrl")}/confirmemail/{code}/{userIdentity.Email}";
            await _emailSender.SendEmailAsync(userIdentity.Email, title,
                $"Please confirm your account by <a href='{url}'>clicking here</a>.<br/>After confirming your email address use the forgot password option to set your password.");
        }
    }
}
