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
        private bool _disposed = false;
        private readonly IRaceVenturaDbContext _context;
        private GenericRepository<UserLink> _userLinkRepository;
        private GenericRepository<Organisation> _organisationRepository;
        private GenericRepository<Race> _raceRepository;
        private GenericRepository<Stage> _stageRepository;
        private GenericRepository<Point> _pointRepository;
        private GenericRepository<Team> _teamRepository;
        private GenericRepository<VisitedPoint> _visitedPointRepository;
        private GenericRepository<FinishedStage> _finishedStageRepository;
        private GenericRepository<RegisteredId> _registeredIdRepository;

        public IGenericRepository<UserLink> UserLinkRepository
        {
            get
            {
                if (_userLinkRepository == null)
                {
                    _userLinkRepository = new GenericRepository<UserLink>(_context);
                }

                return (IGenericRepository<UserLink>)_userLinkRepository;
            }
        }
        public IGenericRepository<Organisation> OrganisationRepository
        {
            get
            {
                if (_organisationRepository == null)
                {
                    _organisationRepository = new GenericRepository<Organisation>(_context);
                }

                return (IGenericRepository<Organisation>)_organisationRepository;
            }
        }

        public IGenericRepository<Race> RaceRepository
        {
            get
            {
                if (_raceRepository == null)
                {
                    _raceRepository = new GenericRepository<Race>(_context);
                }

                return (IGenericRepository<Race>)_raceRepository;
            }
        }

        public IGenericRepository<Stage> StageRepository
        {
            get
            {
                if (_stageRepository == null)
                {
                    _stageRepository = new GenericRepository<Stage>(_context);
                }

                return (IGenericRepository<Stage>)_stageRepository;
            }
        }

        public IGenericRepository<Point> PointRepository
        {
            get
            {
                if (_pointRepository == null)
                {
                    _pointRepository = new GenericRepository<Point>(_context);
                }

                return (IGenericRepository<Point>)_pointRepository;
            }
        }

        public IGenericRepository<Team> TeamRepository
        {
            get
            {
                if (_teamRepository == null)
                {
                    _teamRepository = new GenericRepository<Team>(_context);
                }

                return (IGenericRepository<Team>)_teamRepository;
            }
        }

        public IGenericRepository<VisitedPoint> VisitedPointRepository
        {
            get
            {
                if (_visitedPointRepository == null)
                {
                    _visitedPointRepository = new GenericRepository<VisitedPoint>(_context);
                }

                return (IGenericRepository<VisitedPoint>)_visitedPointRepository;
            }
        }

        public IGenericRepository<FinishedStage> FinishedStageRepository
        {
            get
            {
                if (_finishedStageRepository == null)
                {
                    _finishedStageRepository = new GenericRepository<FinishedStage>(_context);
                }

                return (IGenericRepository<FinishedStage>)_finishedStageRepository;
            }
        }

        public IGenericRepository<RegisteredId> RegisteredIdRepository
        {
            get
            {
                if (_registeredIdRepository == null)
                {
                    _registeredIdRepository = new GenericRepository<RegisteredId>(_context);
                }

                return (IGenericRepository<RegisteredId>)_registeredIdRepository;
            }
        }

        public RaceVenturaUnitOfWork(IRaceVenturaDbContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
