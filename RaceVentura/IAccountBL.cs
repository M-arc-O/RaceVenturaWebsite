using RaceVenturaData.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;

namespace RaceVentura
{
    public interface IAccountBL
    {
        Task<IdentityResult> CreateUser(string registrationSecret, AppUser userIdentity, string password);
        Task<AppUser> FindByNameAsync(string userName); 
        Task<AppUser> FindByEmailAsync(string email);
        Task<string> FindEmailByIdAsync(Guid userId);
        Task<bool> CheckPasswordAsync(AppUser userToVerify, string password);
        Task ForgotPassword(string emailAddress);
        Task<IdentityResult> ResetPassword(string emailAddress, string password, string code);
        Task ConfirmEmail(string emailAddress, string code);
    }
}