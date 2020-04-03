using Adventure4YouData.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Adventure4YouData.DatabaseContext
{
    public interface IAdventure4YouDbContext
    {
        DbSet<Race> Races { get; set; }

        DbSet<UserLink> UserLinks { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}