using Adventure4YouData.Models;
using Adventure4YouData.Models.Races;
using Adventure4YouData.Repositories;
using System.Threading.Tasks;

namespace Adventure4YouData
{
    public interface IAdventure4YouUnitOfWork
    {
        GenericRepository<UserLink> UserLinkRepository { get; }
        GenericRepository<Point> PointRepository { get; }
        GenericRepository<Race> RaceRepository { get; }
        GenericRepository<Stage> StageRepository { get; }
        GenericRepository<Team> TeamRepository { get; }
        GenericRepository<VisitedPoint> VisitedPointRepository { get; }

        void Dispose();
        void Save();
        Task<int> SaveAsync();
    }
}