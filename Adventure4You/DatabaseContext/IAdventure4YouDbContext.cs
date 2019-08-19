using Adventure4You.Models;
using Adventure4You.Models.Points;
using Adventure4You.Models.Stages;
using Adventure4You.Models.Teams;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Adventure4You.DatabaseContext
{
    public interface IAdventure4YouDbContext
    {
        DbSet<PointLink> PointLinks { get; set; }
        DbSet<Point> Points { get; set; }
        DbSet<Race> Races { get; set; }
        DbSet<StageLink> StageLinks { get; set; }
        DbSet<Stage> Stages { get; set; }
        DbSet<TeamLink> TeamLinks { get; set; }
        DbSet<TeamPointVisited> TeamPointsVisited { get; set; }
        DbSet<Team> Teams { get; set; }
        DbSet<TeamStageFinished> TeamStagesFinished { get; set; }
        DbSet<UserLink> UserLinks { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}