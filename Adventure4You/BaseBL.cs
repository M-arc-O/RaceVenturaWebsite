using Adventure4You.DatabaseContext;
using Adventure4You.Models;
using System;
using System.Linq;

namespace Adventure4You
{
    public abstract class BaseBL
    {
        protected readonly IAdventure4YouDbContext _Context;

        public BaseBL(IAdventure4YouDbContext context)
        {
            _Context = context;
        }

        protected UserLink CheckIfUserHasAccessToRace(Guid userId, Guid raceId)
        {
            return _Context.UserLinks.FirstOrDefault(link => link.UserId == userId && link.RaceId == raceId);
        }
    }
}
