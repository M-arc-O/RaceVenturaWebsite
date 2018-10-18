using Adventure4You.Models;
using Microsoft.EntityFrameworkCore;

namespace Adventure4You.DatabaseContext
{
    public class Adventure4YouContext: DbContext
    {
        public Adventure4YouContext(DbContextOptions<Adventure4YouContext> options) 
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<UserLink> UserLinks { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamLink> TeamLinks { get; set; }

        public DbSet<RaceModel> Races { get; set; }

        public DbSet<Point> Points { get; set; }

        public DbSet<PointLink> PointLinks { get; set; }


    }
}
