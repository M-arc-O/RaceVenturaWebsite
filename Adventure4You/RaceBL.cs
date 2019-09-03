using Adventure4You.DatabaseContext;
using Adventure4You.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adventure4You
{
    public class RaceBL: BaseBL, IRaceBL
    {
        public RaceBL(IAdventure4YouDbContext context): base(context)
        {
        }

        public List<Race> GetAllRaces()
        {
            return _Context.Races.ToList();
        }

        public BLReturnCodes GetRaceDetails(Guid id, Guid raceId, out Race raceModel)
        {
            raceModel = null;

            if (CheckIfUserHasAccessToRace(id, raceId) == null)
            {
                return BLReturnCodes.UserUnauthorized;
            }

            raceModel = _Context.Races.FirstOrDefault(model => model.Id == raceId);
            if (raceModel == null)
            {
                return BLReturnCodes.UnknownRace;
            }

            return BLReturnCodes.Ok;            
        }

        public BLReturnCodes AddRace(Guid userId, Race raceModel)
        {
            if (!CheckIfRaceNameIsTaken(raceModel.Name))
            {
                _Context.Races.Add(raceModel);
                _Context.SaveChanges();

                _Context.UserLinks.Add(new UserLink
                {
                    RaceId = raceModel.Id,
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

        public BLReturnCodes EditRace(Guid id, Race raceModelInput)
        {
            var raceModel = GetRaceModel(raceModelInput.Id);

            if (raceModel != null)
            {
                if (CheckIfUserHasAccessToRace(id, raceModel.Id) == null)
                {
                    return BLReturnCodes.UserUnauthorized;
                }

                if (raceModel.Name.Equals(raceModelInput.Name) || !CheckIfRaceNameIsTaken(raceModelInput.Name))
                {
                    raceModel.Name = raceModelInput.Name;
                    raceModel.CoordinatesCheckEnabled = raceModelInput.CoordinatesCheckEnabled;
                    raceModel.SpecialTasksAreStage = raceModelInput.SpecialTasksAreStage;
                    raceModel.MaximumTeamSize = raceModelInput.MaximumTeamSize;
                    raceModel.MinimumPointsToCompleteStage = raceModelInput.MinimumPointsToCompleteStage;
                    raceModel.StartTime = raceModelInput.StartTime;
                    raceModel.EndTime = raceModelInput.EndTime;
                    _Context.SaveChanges();

                    return BLReturnCodes.Ok;
                }
                else
                {
                    return BLReturnCodes.Duplicate;
                }
            }
            else
            {
                return BLReturnCodes.UnknownRace;
            }
        }

        public BLReturnCodes DeleteRace(Guid id, Guid raceId)
        {
            var raceModel = GetRaceModel(raceId);

            if (raceModel != null)
            {
                var userLink = CheckIfUserHasAccessToRace(id, raceModel.Id);
                if (userLink == null)
                {
                    return BLReturnCodes.UserUnauthorized;
                }
                else
                {
                    _Context.UserLinks.Remove(userLink);
                }

                _Context.Races.Remove(raceModel);
                _Context.Stages.RemoveRange(_Context.Stages.Where(stage => stage.RaceId == raceId));

                _Context.SaveChanges();
            }
            else
            {
                return BLReturnCodes.UnknownRace;
            }

            return BLReturnCodes.Ok;
        }

        private Race GetRaceModel(Guid raceId)
        {
            return _Context.Races.FirstOrDefault(race => race.Id == raceId);
        }

        private bool CheckIfRaceNameIsTaken(string name)
        {
            return _Context.Races.Any(race => race.Name.Equals(name));
        }       
    }
}
