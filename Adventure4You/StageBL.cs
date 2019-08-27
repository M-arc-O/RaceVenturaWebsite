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

            var stageLinks = _Context.StageLinks.Where(link => link.RaceId == raceId);
            stages = _Context.Stages.Where(stage => stageLinks.Any(link => link.StageId == stage.Id)).ToList();
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

            _Context.Stages.Add(stageModel);
            _Context.SaveChanges();

            var stageLink = new StageLink
            {
                StageId = stageModel.Id,
                RaceId = raceId
            };
            _Context.StageLinks.Add(stageLink);
            _Context.SaveChanges();

            return BLReturnCodes.Ok;
        }

        public BLReturnCodes EditStage(Guid userId, Stage stageModel)
        {
            var stageLink = _Context.StageLinks.FirstOrDefault(link => link.StageId == stageModel.Id);

            if (stageLink != null)
            {
                if (CheckIfUserHasAccessToRace(userId, stageLink.RaceId) == null)
                {
                    return BLReturnCodes.UserUnauthorized;
                }

                var stage = _Context.Stages.First(s => s.Id == stageModel.Id);
                stage.Name = stageModel.Name;
                stage.MimimumPointsToCompleteStage = stageModel.MimimumPointsToCompleteStage;
                _Context.SaveChanges();

                return BLReturnCodes.Ok;
            }

            return BLReturnCodes.UnknownStage;
        }
    }
}
