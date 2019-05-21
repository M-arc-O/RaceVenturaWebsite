using Adventure4You.DatabaseContext;
using Adventure4You.Models;
using System.Collections.Generic;
using System.Linq;

namespace Adventure4You
{
    public class RaceBL : IRaceBL
    {
        private readonly IAdventure4YouDbContext _Context;

        public RaceBL(IAdventure4YouDbContext context)
        {
            _Context = context;
        }

        public List<Race> GetAllRaces()
        {
            return _Context.Races.ToList();
        }
    }
}
