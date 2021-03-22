using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RaceVenturaData.DatabaseContext;
using RaceVenturaData.Models.Identity;

namespace RaceVentura.Extensions
{
    public static class DBContextExtensions
    {
        public static void AddDBContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<RaceVenturaDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("RaceVenturaConnection"), b => b.MigrationsAssembly("RaceVenturaAPI"));
            });

            services.AddScoped<IRaceVenturaDbContext>(provider => provider.GetService<RaceVenturaDbContext>());
        }

        public static void AddAppUserAuthentication(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Password.RequireNonAlphanumeric = true;
                o.Password.RequiredLength = 8;
            })
                .AddEntityFrameworkStores<RaceVenturaDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
