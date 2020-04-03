using Adventure4YouData.DatabaseContext;
using Adventure4YouData.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adventure4You.Helpers
{
    public class DBContextHelper
    {
        public static void AddDBContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<Adventure4YouDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Adventure4YouConnection"), b => b.MigrationsAssembly("Adventure4YouAPI"));
            });

            services.AddScoped<IAdventure4YouDbContext>(provider => provider.GetService<Adventure4YouDbContext>());
        }

        public static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var builder = services.AddIdentityCore<AppUser>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<Adventure4YouDbContext>().AddDefaultTokenProviders();
        }
    }
}
