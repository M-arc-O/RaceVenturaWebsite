using RaceVenturaData.DatabaseContext;
using RaceVenturaData.Models;
using RaceVenturaData.Models.Races;
using RaceVenturaData.Repositories;
using System;
using System.Threading.Tasks;

namespace RaceVenturaData
{
    public class RaceVenturaUnitOfWork : IDisposable, IRaceVenturaUnitOfWork
    {
        private bool _Disposed = false;
        private readonly IRaceVenturaDbContext _Context;
        private GenericRepository<UserLink> _UserLinkRepository;
        private GenericRepository<Race> _RaceRepository;
        private GenericRepository<Stage> _StageRepository;
        private GenericRepository<Point> _PointRepository;
        private GenericRepository<Team> _TeamRepository;
        private GenericRepository<VisitedPoint> _VisitedPointRepository;
        private GenericRepository<FinishedStage> _FinishedStageRepository;
        private GenericRepository<RegisteredId> _RegisteredIdRepository;

        public IGenericRepository<UserLink> UserLinkRepository
        {
            get
            {
                if (_UserLinkRepository == null)
                {
                    _UserLinkRepository = new GenericRepository<UserLink>(_Context);
                }

                return (IGenericRepository<UserLink>)_UserLinkRepository;
            }
        }

        public IGenericRepository<Race> RaceRepository
        {
            get
            {
                if (_RaceRepository == null)
                {
                    _RaceRepository = new GenericRepository<Race>(_Context);
                }

                return (IGenericRepository<Race>)_RaceRepository;
            }
        }

        public IGenericRepository<Stage> StageRepository
        {
            get
            {
                if (_StageRepository == null)
                {
                    _StageRepository = new GenericRepository<Stage>(_Context);
                }

                return (IGenericRepository<Stage>)_StageRepository;
            }
        }

        public IGenericRepository<Point> PointRepository
        {
            get
            {
                if (_PointRepository == null)
                {
                    _PointRepository = new GenericRepository<Point>(_Context);
                }

                return (IGenericRepository<Point>)_PointRepository;
            }
        }

        public IGenericRepository<Team> TeamRepository
        {
            get
            {
                if (_TeamRepository == null)
                {
                    _TeamRepository = new GenericRepository<Team>(_Context);
                }

                return (IGenericRepository<Team>)_TeamRepository;
            }
        }

        public IGenericRepository<VisitedPoint> VisitedPointRepository
        {
            get
            {
                if (_VisitedPointRepository == null)
                {
                    _VisitedPointRepository = new GenericRepository<VisitedPoint>(_Context);
                }

                return (IGenericRepository<VisitedPoint>)_VisitedPointRepository;
            }
        }

        public IGenericRepository<FinishedStage> FinishedStageRepository
        {
            get
            {
                if (_FinishedStageRepository == null)
                {
                    _FinishedStageRepository = new GenericRepository<FinishedStage>(_Context);
                }

                return (IGenericRepository<FinishedStage>)_FinishedStageRepository;
            }
        }

        public IGenericRepository<RegisteredId> RegisteredIdRepository
        {
            get
            {
                if (_RegisteredIdRepository == null)
                {
                    _RegisteredIdRepository = new GenericRepository<RegisteredId>(_Context);
                }

                return (IGenericRepository<RegisteredId>)_RegisteredIdRepository;
            }
        }

        public RaceVenturaUnitOfWork(IRaceVenturaDbContext context)
        {
            _Context = context;
        }

        public void Save()
        {
            _Context.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return _Context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    _Context.Dispose();
                }
            }
            _Disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
