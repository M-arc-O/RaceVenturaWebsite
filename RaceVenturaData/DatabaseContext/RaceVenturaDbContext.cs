using RaceVenturaData.Models;
using RaceVenturaData.Models.Identity;
using RaceVenturaData.Models.Races;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RaceVenturaData.DatabaseContext
{
    public class RaceVenturaDbContext : IdentityDbContext<AppUser>, IRaceVenturaDbContext
    {
        public RaceVenturaDbContext(DbContextOptions<RaceVenturaDbContext> options)
            : base(options)
        {

        }        
        
        public DbSet<Race> Races { get; set; }

        public DbSet<UserLink> UserLinks { get; set; }

        public DbSet<Organisation> Organisations { get; set; }
    }
}
