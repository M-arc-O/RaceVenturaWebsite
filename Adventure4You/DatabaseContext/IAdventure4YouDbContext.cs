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
        DbSet<Race> Races { get; set; }

        DbSet<UserLink> UserLinks { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}