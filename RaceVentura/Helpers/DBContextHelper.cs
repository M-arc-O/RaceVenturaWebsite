using RaceVenturaData.DatabaseContext;
using RaceVenturaData.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System;

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
