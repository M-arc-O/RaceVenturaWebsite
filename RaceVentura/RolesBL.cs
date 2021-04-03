using Microsoft.AspNetCore.Identity;
using RaceVentura.Models;
using RaceVenturaData.Models.Identity;
using System.Threading.Tasks;

namespace RaceVentura
{
    public class RolesBL : IRolesBL
    {
        private readonly UserManager<AppUser> _userManager;

        public RolesBL(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> IsInRoleAsync(string userId, Roles role)
        {
            var user = (await _userManager.FindByIdAsync(userId)) ?? throw new BusinessException("Unkown user.", BLErrorCodes.NotFound);

            return await _userManager.IsInRoleAsync(user, role.ToString());
        }

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
        }
    }
}
