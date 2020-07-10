using RaceVenturaData.Models;
using RaceVenturaData.Models.Races;
using RaceVenturaData.Repositories;
using System.Threading.Tasks;

namespace RaceVenturaData
{
    public interface IRaceVenturaUnitOfWork
    {
        IGenericRepository<UserLink> UserLinkRepository { get; }
        IGenericRepository<Point> PointRepository { get; }
        IGenericRepository<Race> RaceRepository { get; }
        IGenericRepository<Stage> StageRepository { get; }
        IGenericRepository<Team> TeamRepository { get; }
        IGenericRepository<VisitedPoint> VisitedPointRepository { get; }
        IGenericRepository<FinishedStage> FinishedStageRepository { get; }
        IGenericRepository<RegisteredId> RegisteredIdRepository { get; }

        void Dispose();
        void Save();
        Task<int> SaveAsync();
    }
}