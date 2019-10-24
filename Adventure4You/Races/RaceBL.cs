using Adventure4You.DatabaseContext;
using Adventure4You.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adventure4You.Races
{
    public class RaceBL : BaseBL, IRaceBL
    {
        public RaceBL(IAdventure4YouDbContext context) : base(context)
        {
        }

        public BLReturnCodes GetAllRaces(Guid userId, out List<Race> races)
        {
            races = null;
            var links = _Context.UserLinks.Where(link => link.UserId == userId);

            races = _Context.Races.Where(race => links.Any(link => link.RaceId == race.RaceId)).OrderBy(race => race.Name).ToList();
            if (races == null)
            {
                return BLReturnCodes.NotFound;
            }

            return BLReturnCodes.Ok;
        }

        public BLReturnCodes GetRaceDetails(Guid userId, Guid raceId, out Race race)
        {
            race = null;

            if (CheckIfUserHasAccessToRace(userId, raceId) == null)
            {
                return BLReturnCodes.UserUnauthorized;
            }

            race = GetRaceById(raceId);
            if (race == null)
            {
                return BLReturnCodes.Unknown;
            }

            return BLReturnCodes.Ok;
        }

        public BLReturnCodes AddRace(Guid userId, Race race)
        {
            if (!CheckIfRaceNameExists(race.Name))
            {
                _Context.Races.Add(race);
                _Context.SaveChanges();

                _Context.UserLinks.Add(new UserLink
                {
                    RaceId = race.RaceId,
                    UserId = userId
                });

                _Context.SaveChanges();
            }
            else
            {
                return BLReturnCodes.Duplicate;
            }

            return BLReturnCodes.Ok;
        }

        public BLReturnCodes DeleteRace(Guid userId, Guid raceId)
        {
            var userLink = CheckIfUserHasAccessToRace(userId, raceId);
            if (userLink == null)
            {
                return BLReturnCodes.UserUnauthorized;
            }

            var race = GetRaceById(raceId);
            if (race != null)
            {
                _Context.UserLinks.Remove(userLink);
                _Context.Races.Remove(race);
                
                _Context.SaveChanges();
            }
            else
            {
                return BLReturnCodes.Unknown;
            }

            return BLReturnCodes.Ok;
        }

        public BLReturnCodes EditRace(Guid userId, Race raceNew)
        {
            if (CheckIfUserHasAccessToRace(userId, raceNew.RaceId) == null)
            {
                return BLReturnCodes.UserUnauthorized;
            }

            var race = GetRaceById(raceNew.RaceId);
            if (race == null)
            {
                return BLReturnCodes.Unknown;
            }

            if (race.Name.ToUpper().Equals(raceNew.Name.ToUpper()) || !CheckIfRaceNameExists(raceNew.Name))
            {
                race.Name = raceNew.Name;
                race.CoordinatesCheckEnabled = raceNew.CoordinatesCheckEnabled;
                race.SpecialTasksAreStage = raceNew.SpecialTasksAreStage;
                race.MaximumTeamSize = raceNew.MaximumTeamSize;
                race.MinimumPointsToCompleteStage = raceNew.MinimumPointsToCompleteStage;
                race.StartTime = raceNew.StartTime;
                race.EndTime = raceNew.EndTime;
                _Context.SaveChanges();

                return BLReturnCodes.Ok;
            }
            else
            {
                return BLReturnCodes.Duplicate;
            }
        }

        private bool CheckIfRaceNameExists(string name)
        {
            return _Context.Races.Any(race => race.Name.ToUpper().Equals(name.ToUpper()));
        }
    }
}
