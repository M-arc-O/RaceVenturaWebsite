using System;
using System.Collections.Generic;
using System.Linq;
using Adventure4You.DatabaseContext;
using Adventure4You.Models;
using Adventure4You.Models.Stages;

namespace Adventure4You
{
    public class StageBL : BaseBL, IStageBL
    {
        private readonly IPointBL _PointBL;

        public StageBL(IAdventure4YouDbContext context, IPointBL pointBL) : base(context)
        {
            _PointBL = pointBL;
        }

        public BLReturnCodes GetStages(Guid userId, Guid raceId, out List<Stage> stages)
        {
            stages = null;

            var retVal = CheckIfUserHasAccessToRaceAndRaceExists(userId, raceId);
            if (retVal == BLReturnCodes.Ok)
            {
                stages = _Context.Stages.Where(stage => stage.RaceId == raceId).OrderBy(stage => stage.Name).ToList();
                if (stages == null)
                {
                    return BLReturnCodes.NoStagesFound;
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
                    return BLReturnCodes.UnknownStage;
                }
            }

            return retVal;
        }

        public BLReturnCodes AddStage(Guid userId, Stage stage, Guid raceId)
        {
            var retVal = CheckIfUserHasAccessToRaceAndRaceExists(userId, raceId);
            if (retVal == BLReturnCodes.Ok)
            {
                if (CheckIfStageNameExists(stage))
                {
                    return BLReturnCodes.Duplicate;
                }

                _Context.Stages.Add(stage);
                _Context.SaveChanges();
            }

            return retVal;
        }

        public BLReturnCodes DeleteStage(Guid userId, Guid stageId, Guid raceId)
        {
            var retVal = CheckIfUserHasAccessToRaceAndRaceExists(userId, raceId);
            if (retVal == BLReturnCodes.Ok)
            {
                var stageModel = GetStage(stageId);
                if (stageModel == null)
                {
                    return BLReturnCodes.UnknownStage;
                }

                _PointBL.RemovePoints(userId, stageId);

                _Context.Stages.Remove(stageModel);
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
                var stage = GetStage(stageNew.Id);
                if (stage == null)
                {
                    return BLReturnCodes.UnknownStage;
                }

                if (!stage.Name.ToUpper().Equals(stageNew.Name.ToUpper()) && CheckIfStageNameExists(stageNew))
                {
                    return BLReturnCodes.Duplicate;
                }

                stage.Name = stageNew.Name;
                stage.MimimumPointsToCompleteStage = stageNew.MimimumPointsToCompleteStage;
                _Context.SaveChanges();
            }

            return retVal;
        }

        public BLReturnCodes RemoveStages(Guid userId, Guid raceId)
        {
            var retVal = CheckIfUserHasAccessToRaceAndRaceExists(userId, raceId);
            if (retVal == BLReturnCodes.Ok)
            {
                var stages = _Context.Stages.Where(stage => stage.RaceId == raceId);
                stages.ToList().ForEach(s => _PointBL.RemovePoints(userId, s.Id));

                _Context.Stages.RemoveRange(stages);
                _Context.SaveChanges();
            }

            return retVal;
        }

        private Stage GetStage(Guid stageId)
        {
            return _Context.Stages.FirstOrDefault(s => s.Id == stageId);
        }

        private BLReturnCodes CheckIfUserHasAccessToRaceAndRaceExists(Guid userId, Guid raceId)
        {
            var race = _Context.Races.Where(r => r.Id == raceId);
            if (race == null || race.Count() == 0)
            {
                return BLReturnCodes.UnknownRace;
            }

            if (CheckIfUserHasAccessToRace(userId, raceId) == null)
            {
                return BLReturnCodes.UserUnauthorized;
            }

            return BLReturnCodes.Ok;
        }

        private bool CheckIfStageNameExists(Stage stage)
        {
            return _Context.Stages.Where(s => s.RaceId == stage.RaceId).Any(s => s.Name.ToUpper().Equals(stage.Name.ToUpper()));
        }
    }
}
