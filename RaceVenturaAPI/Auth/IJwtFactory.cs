using RaceVentura.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RaceVenturaAPI.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(string user, string id, Roles[] roles);
    }
}
