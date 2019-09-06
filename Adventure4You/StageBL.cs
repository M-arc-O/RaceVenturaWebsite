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
        public StageBL(IAdventure4YouDbContext context) : base(context)
        {

        }

        public BLReturnCodes GetStages(Guid userId, Guid raceId, out List<Stage> stages)
        {
            stages = null;

            var race = _Context.Races.Where(r => r.Id == raceId);
            if (race == null || race.Count() == 0)
            {
                return BLReturnCodes.UnknownRace;
            }

            if (CheckIfUserHasAccessToRace(userId, raceId) == null)
            {
                return BLReturnCodes.UserUnauthorized;
            }

            stages = _Context.Stages.Where(stage => stage.RaceId == raceId).OrderBy(stage => stage.Name).ToList();
            if (stages == null)
            {
                return BLReturnCodes.NoStagesFound;
            }

            return BLReturnCodes.Ok;
        }

        public BLReturnCodes GetStageDetails(Guid userId, Guid stageId, Guid raceId, out Stage stageModel)
        {
            stageModel = null;

            if (CheckIfUserHasAccessToRace(userId, raceId) == null)
            {
                return BLReturnCodes.UserUnauthorized;
            }

            stageModel = _Context.Stages.FirstOrDefault(s => s.Id == stageId);
            if (stageModel == null)
            {
                return BLReturnCodes.UnknownStage;
            }

            return BLReturnCodes.Ok;
        }

        public BLReturnCodes AddStage(Guid userId, Stage stageModel, Guid raceId)
        {
            if (CheckIfUserHasAccessToRace(userId, raceId) == null)
            {
                return BLReturnCodes.UserUnauthorized;
            }

            if (_Context.Stages.Any(stage => stage.Name.Equals(stageModel.Name)))
            {
                return BLReturnCodes.Duplicate;
            }

            _Context.Stages.Add(stageModel);
            _Context.SaveChanges();

            return BLReturnCodes.Ok;
        }

        public BLReturnCodes DeleteStage(Guid userId, Guid stageId, Guid raceId)
        {
            var userLink = CheckIfUserHasAccessToRace(userId, raceId);
            if (userLink != null)
            {
                var stageModel = _Context.Stages.FirstOrDefault(stage => stage.Id == stageId);
                if (stageModel == null)
                {
                    return BLReturnCodes.UnknownStage;
                }

                _Context.Stages.Remove(stageModel);
                _Context.SaveChanges();

                return BLReturnCodes.Ok;
            }
            
            return BLReturnCodes.UserUnauthorized;
        }

        public BLReturnCodes EditStage(Guid userId, Stage stageModel)
        {
            if (CheckIfUserHasAccessToRace(userId, stageModel.RaceId) == null)
            {
                return BLReturnCodes.UserUnauthorized;
            }

            var stage = _Context.Stages.First(s => s.Id == stageModel.Id);
            if (stage == null)
            {
                return BLReturnCodes.UnknownStage;
            }

            if (_Context.Stages.Any(s => s.Name.Equals(stageModel.Name)))
            {
                return BLReturnCodes.Duplicate;
            }

            stage.Name = stageModel.Name;
            stage.MimimumPointsToCompleteStage = stageModel.MimimumPointsToCompleteStage;
            _Context.SaveChanges();

            return BLReturnCodes.Ok;
        }
    }
}
