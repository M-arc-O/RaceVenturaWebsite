using Adventure4YouData.Models;
using Adventure4YouData.Models.Races;
using Adventure4YouData.Repositories;
using System.Threading.Tasks;

namespace Adventure4YouData
{
    public interface IAdventure4YouUnitOfWork
    {
        IGenericRepository<UserLink> UserLinkRepository { get; }
        IGenericRepository<Point> PointRepository { get; }
        IGenericRepository<Race> RaceRepository { get; }
        IGenericRepository<Stage> StageRepository { get; }
        IGenericRepository<Team> TeamRepository { get; }
        IGenericRepository<VisitedPoint> VisitedPointRepository { get; }

        void Dispose();
        void Save();
        Task<int> SaveAsync();
    }
}