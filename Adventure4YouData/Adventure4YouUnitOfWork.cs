using Adventure4YouData.DatabaseContext;
using Adventure4YouData.Models;
using Adventure4YouData.Models.Races;
using Adventure4YouData.Repositories;
using System;
using System.Threading.Tasks;

namespace Adventure4YouData
{
    public class Adventure4YouUnitOfWork : IDisposable, IAdventure4YouUnitOfWork
    {
        private bool _Disposed = false;
        private readonly IAdventure4YouDbContext _Context;
        private GenericRepository<UserLink> _UserLinkRepository;
        private GenericRepository<Race> _RaceRepository;
        private GenericRepository<Stage> _StageRepository;
        private GenericRepository<Point> _PointRepository;
        private GenericRepository<Team> _TeamRepository;
        private GenericRepository<VisitedPoint> _VisitedPointRepository;

        public GenericRepository<UserLink> UserLinkRepository
        {
            get
            {
                if (_UserLinkRepository == null)
                {
                    _UserLinkRepository = new GenericRepository<UserLink>(_Context);
                }

                return _UserLinkRepository;
            }
        }

        public GenericRepository<Race> RaceRepository
        {
            get
            {
                if (_RaceRepository == null)
                {
                    _RaceRepository = new GenericRepository<Race>(_Context);
                }

                return _RaceRepository;
            }
        }

        public GenericRepository<Stage> StageRepository
        {
            get
            {
                if (_StageRepository == null)
                {
                    _StageRepository = new GenericRepository<Stage>(_Context);
                }

                return _StageRepository;
            }
        }

        public GenericRepository<Point> PointRepository
        {
            get
            {
                if (_PointRepository == null)
                {
                    _PointRepository = new GenericRepository<Point>(_Context);
                }

                return _PointRepository;
            }
        }

        public GenericRepository<Team> TeamRepository
        {
            get
            {
                if (_TeamRepository == null)
                {
                    _TeamRepository = new GenericRepository<Team>(_Context);
                }

                return _TeamRepository;
            }
        }
        
        public GenericRepository<VisitedPoint> VisitedPointRepository
        {
            get
            {
                if (_VisitedPointRepository == null)
                {
                    _VisitedPointRepository = new GenericRepository<VisitedPoint>(_Context);
                }

                return _VisitedPointRepository;
            }
        }

        public Adventure4YouUnitOfWork(IAdventure4YouDbContext context)
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
