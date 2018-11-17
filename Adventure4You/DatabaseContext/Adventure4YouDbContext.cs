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

        public DbSet<Point> Points { get; set; }

        public DbSet<PointLink> PointLinks { get; set; }

        public DbSet<Race> Races { get; set; }

        public DbSet<Stage> Stages { get; set; }

        public DbSet<StageLink> StageLinks { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamLink> TeamLinks { get; set; }

        public DbSet<TeamPointVisited> TeamPointsVisited { get; set; }

        public DbSet<TeamStageFinished> TeamStagesFinished { get; set; }

        public DbSet<UserLink> UserLinks { get; set; }
    }
}
