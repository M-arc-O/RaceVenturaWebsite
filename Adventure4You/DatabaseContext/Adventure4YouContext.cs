using System;
using Adventure4You.Models;
using Microsoft.EntityFrameworkCore;

namespace Adventure4You.DatabaseContext
{
    public class Adventure4YouContext: DbContext
    {
        public Adventure4YouContext(DbContextOptions<Adventure4YouContext> options) 
            : base(options)
        {
        }

        public DbSet<RaceModel> Races { get; set; }
    }
}
