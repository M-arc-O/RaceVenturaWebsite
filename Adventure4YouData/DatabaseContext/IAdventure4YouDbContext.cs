using Adventure4YouData.Models;
using Adventure4YouData.Models.Races;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Adventure4YouData.DatabaseContext
{
    public interface IAdventure4YouDbContext: IDisposable
    {
        DbSet<Race> Races { get; set; }

        DbSet<UserLink> UserLinks { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges(bool acceptAllChangesOnSuccess);

        int SaveChanges();
        
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}