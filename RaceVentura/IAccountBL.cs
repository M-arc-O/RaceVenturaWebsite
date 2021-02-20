using RaceVenturaData.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace RaceVentura
{
    public interface IAccountBL
    {
        Task<IdentityResult> CreateUser(string registrationSecret, AppUser userIdentity, string password);
        Task<AppUser> FindByNameAsync(string userName);
        Task<bool> CheckPasswordAsync(AppUser userToVerify, string password);
        Task ForgotPassword(string emailAddress);
        Task<IdentityResult> ResetPassword(string emailAddress, string password, string code);
        Task ConfirmEmail(string emailAddress, string code);
    }
}