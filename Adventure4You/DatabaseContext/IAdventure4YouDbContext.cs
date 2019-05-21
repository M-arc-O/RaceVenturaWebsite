using Adventure4You.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}