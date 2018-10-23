using Adventure4You.Models;
using Adventure4You.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Adventure4You.DatabaseContext
{
    public class Adventure4YouDbContext: IdentityDbContext<AppUser>
    {
        public Adventure4YouDbContext(DbContextOptions<Adventure4YouDbContext> options) 
            : base(options)
        {
        }

        public DbSet<UserLink> UserLinks { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamLink> TeamLinks { get; set; }

        public DbSet<RaceModel> Races { get; set; }

        public DbSet<Point> Points { get; set; }

        public DbSet<PointLink> PointLinks { get; set; }
    }
}
