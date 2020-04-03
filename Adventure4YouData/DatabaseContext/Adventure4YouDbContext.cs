
using Adventure4YouData.Models;
using Adventure4YouData.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Adventure4YouData.DatabaseContext
{
    public class Adventure4YouDbContext : IdentityDbContext<AppUser>, IAdventure4YouDbContext
    {
        public Adventure4YouDbContext(DbContextOptions<Adventure4YouDbContext> options)
            : base(options)
        {
        }        
        
        public DbSet<Race> Races { get; set; }

        public DbSet<UserLink> UserLinks { get; set; }
    }
}
