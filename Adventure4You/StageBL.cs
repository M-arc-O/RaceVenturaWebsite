using System.Collections.Generic;
using System.Linq;
using Adventure4You.DatabaseContext;
using Adventure4You.Models;
using Adventure4You.Models.Stages;

namespace Adventure4You
{
    public class StageBL : IStageBL
    {
        private readonly IAdventure4YouDbContext _Context;

        public StageBL(IAdventure4YouDbContext context)
        {
            _Context = context;
        }

        public BLReturnCodes GetStages(int raceId, out List<Stage> stages)
        {
            stages = null;

            var stageLinks = _Context.StageLinks.Where(link => link.RaceId == raceId);
            if (stageLinks == null || stageLinks.Count() == 0)
            {
                return BLReturnCodes.UnknownRace;
            }

            stages = _Context.Stages.Where(stage => stageLinks.Any(link => link.StageId == stage.Id)).ToList();
            if (stages == null)
            {
                return BLReturnCodes.NoStagesFound;
            }

            return BLReturnCodes.Ok;
        }

        public BLReturnCodes AddRace(Stage stageModel, int raceId)
        {
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
    }
}
