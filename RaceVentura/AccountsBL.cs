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
        private readonly IRaceVenturaDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public AccountsBL(UserManager<AppUser> userManager, IRaceVenturaDbContext context, IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }

        public async Task<IdentityResult> CreateUser(AppUser userIdentity, string password)
        {
            var result = await _userManager.CreateAsync(userIdentity, password);

            if (result.Succeeded)
            {
                await _context.SaveChangesAsync();
            }

            return result;
        }

        public Task<AppUser> FindByNameAsync(string userName)
        {
            return _userManager.FindByNameAsync(userName);
        }

        public Task<bool> CheckPasswordAsync(AppUser userToVerify, string password)
        {
            return _userManager.CheckPasswordAsync(userToVerify, password);
        }

        public async Task ForgotPassword(string emailAddress)
        {
            var user = await _userManager.FindByEmailAsync(emailAddress);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return;
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
    }
}
