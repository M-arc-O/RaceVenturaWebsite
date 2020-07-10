using RaceVenturaData.DatabaseContext;
using RaceVenturaData.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace RaceVentura
{
    public class AccountsBL : IAccountBL
    {
        private readonly IRaceVenturaDbContext _Context;
        private readonly UserManager<AppUser> _UserManager;

        public AccountsBL(UserManager<AppUser> userManager, IRaceVenturaDbContext context)
        {
            _UserManager = userManager;
            _Context = context;
        }

        public async Task<IdentityResult> CreateUser(AppUser userIdentity, string password)
        {
            var result = await _UserManager.CreateAsync(userIdentity, password);

            if (result.Succeeded)
            {
                await _Context.SaveChangesAsync();
            }

            return result;
        }

        public Task<AppUser> FindByNameAsync(string userName)
        {
            return _UserManager.FindByNameAsync(userName);
        }

        public Task<bool> CheckPasswordAsync(AppUser userToVerify, string password)
        {
            return _UserManager.CheckPasswordAsync(userToVerify, password);
        }
    }
}
