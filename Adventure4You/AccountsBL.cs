using Adventure4You.DatabaseContext;
using Adventure4You.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Adventure4You
{
    public class AccountsBL : IAccountBL
    {
        private readonly Adventure4YouDbContext _Context;
        private readonly UserManager<AppUser> _UserManager;

        public AccountsBL(UserManager<AppUser> userManager, Adventure4YouDbContext context)
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

        public Task<bool> CheckPasswordAsync(object userToVerify, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}
