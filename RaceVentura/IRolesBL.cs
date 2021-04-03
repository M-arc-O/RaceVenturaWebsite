using RaceVentura.Models;
using System.Threading.Tasks;

namespace RaceVentura
{
    public interface IRolesBL
    {
        Task<bool> IsInRoleAsync(string userId, Roles role);
    }
}