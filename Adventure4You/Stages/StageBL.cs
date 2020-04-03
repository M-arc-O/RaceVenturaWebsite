using System;
using System.Collections.Generic;
using System.Linq;
using Adventure4YouData.DatabaseContext;
using Adventure4YouData.Models;
using Adventure4YouData.Models.Stages;

namespace Adventure4You.Stages
{
    public class StageBL : BaseBL, IStageBL
    {
        public StageBL(IAdventure4YouDbContext context) : base(context)
        {
        }

        public BLReturnCodes GetStages(Guid userId, Guid raceId, out List<Stage> stages)
        {
            stages = null;

            var retVal = CheckIfUserHasAccessToRaceAndRaceExists(userId, raceId);
            if (retVal == BLReturnCodes.Ok)
            {
                stages = GetRaceById(raceId)?.Stages?.Where(stage => stage.RaceId == raceId).OrderBy(stage => stage.Number).ToList();
                if (stages == null)
                {
                    return BLReturnCodes.NotFound;
                }
            }

            return retVal;
        }

        public BLReturnCodes GetStageDetails(Guid userId, Guid stageId, Guid raceId, out Stage stage)
        {
            stage = null;

            var retVal = CheckIfUserHasAccessToRaceAndRaceExists(userId, raceId);
            if (retVal == BLReturnCodes.Ok)
            {
                stage = GetStage(stageId);
                if (stage == null)
                {
                    return BLReturnCodes.Unknown;
                }
            }

            return retVal;
        }

        public BLReturnCodes AddStage(Guid userId, Stage stage, Guid raceId)
        {
            var retVal = CheckIfUserHasAccessToRaceAndRaceExists(userId, raceId);
            if (retVal == BLReturnCodes.Ok)
            {
                if (CheckIfStageNumberExists(stage))
                {
                    return BLReturnCodes.Duplicate;
                }

                GetRaceById(raceId)?.Stages.Add(stage);
                _Context.SaveChanges();
            }

            return retVal;
        }

        public BLReturnCodes DeleteStage(Guid userId, Guid stageId, Guid raceId)
        {
            var retVal = CheckIfUserHasAccessToRaceAndRaceExists(userId, raceId);
            if (retVal == BLReturnCodes.Ok)
            {
                var stage = GetStage(stageId);
                if (stage == null)
                {
                    return BLReturnCodes.Unknown;
                }

                GetRaceById(stage.RaceId)?.Stages.Remove(stage);
                _Context.SaveChanges();

                return BLReturnCodes.Ok;
            }

            return retVal;
        }

        public BLReturnCodes EditStage(Guid userId, Stage stageNew)
        {
            var retVal = CheckIfUserHasAccessToRaceAndRaceExists(userId, stageNew.RaceId);
            if (retVal == BLReturnCodes.Ok)
            {
                var stage = GetStage(stageNew.StageId);
                if (stage == null)
                {
                    return BLReturnCodes.Unknown;
                }

                if (stageNew.Number != stage.Number && CheckIfStageNumberExists(stageNew))
                {
                    return BLReturnCodes.Duplicate;
                }

                stage.Name = stageNew.Name;
                stage.Number = stageNew.Number;
                stage.MimimumPointsToCompleteStage = stageNew.MimimumPointsToCompleteStage;
                _Context.SaveChanges();
            }

            return retVal;
        }

        private Stage GetStage(Guid stageId)
        {
            return GetRaceByStageId(stageId)?.Stages.FirstOrDefault(s => s.StageId == stageId);
        }

        private BLReturnCodes CheckIfUserHasAccessToRaceAndRaceExists(Guid userId, Guid raceId)
        {
            var race = _Context.Races.Where(r => r.RaceId == raceId);
            if (race == null || race.Count() == 0)
            {
                return BLReturnCodes.Unknown;
            }

            if (CheckIfUserHasAccessToRace(userId, raceId) == null)
            {
                return BLReturnCodes.UserUnauthorized;
            }

            return BLReturnCodes.Ok;
        }

        private bool CheckIfStageNumberExists(Stage stage)
        {
            var race = GetRaceById(stage.RaceId);
            return race == null ? false : race.Stages.Any(s => s.Number == stage.Number);
        }
    }
}
