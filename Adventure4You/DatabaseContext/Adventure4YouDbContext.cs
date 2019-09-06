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

        public DbSet<Point> Points { get; set; }
        
        public DbSet<Race> Races { get; set; }

        public DbSet<Stage> Stages { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamPointVisited> TeamPointsVisited { get; set; }

        public DbSet<TeamRaceFinished> TeamRacesFinished { get; set; }

        public DbSet<TeamStageFinished> TeamStagesFinished { get; set; }

        public DbSet<UserLink> UserLinks { get; set; }
    }
}
