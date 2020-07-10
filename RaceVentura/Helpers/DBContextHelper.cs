using RaceVenturaData.DatabaseContext;
using RaceVenturaData.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace RaceVentura.Helpers
{
    public class DBContextHelper
    {
        public static void AddDBContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<RaceVenturaDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("RaceVenturaConnection"), b => b.MigrationsAssembly("RaceVenturaAPI"));
            });

            services.AddScoped<IRaceVenturaDbContext>(provider => provider.GetService<RaceVenturaDbContext>());
        }

        public static void AddAuthentication(IServiceCollection services)
        {
            services.AddIdentityCore<AppUser>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<RaceVenturaDbContext>();
        }
    }
}
