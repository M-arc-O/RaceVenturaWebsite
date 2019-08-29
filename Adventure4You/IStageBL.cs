
using Adventure4You.Models;
using Adventure4You.Models.Stages;
using System;
using System.Collections.Generic;

namespace Adventure4You
{
    public interface IStageBL
    {
        BLReturnCodes GetStages(Guid userId, Guid raceId, out List<Stage> stages);

        BLReturnCodes GetStageDetails(Guid userId, Guid stageId, Guid raceId, out Stage stageModel);

        BLReturnCodes AddStage(Guid userId, Stage stageModel, Guid raceId);

        BLReturnCodes EditStage(Guid id, Stage stageModel);

        BLReturnCodes DeleteStage(Guid id, Guid stageId, Guid raceId);
    }
}
