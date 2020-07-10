using RaceVenturaData.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace RaceVentura
{
    public interface IAccountBL
    {
        Task<IdentityResult> CreateUser(AppUser userIdentity, string password);
        Task<AppUser> FindByNameAsync(string userName);
        Task<bool> CheckPasswordAsync(AppUser userToVerify, string password);
    }
}