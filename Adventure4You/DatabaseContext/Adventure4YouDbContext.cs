using Adventure4You.Models;
using Adventure4You.Models.Identity;
using Adventure4You.Models.Points;
using Adventure4You.Models.Stages;
using Adventure4You.Models.Teams;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Adventure4You.DatabaseContext
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
