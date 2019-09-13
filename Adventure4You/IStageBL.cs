
using Adventure4You.Models;
using Adventure4You.Models.Stages;
using System;
using System.Collections.Generic;

namespace Adventure4You
{
    public interface IStageBL
    {
        BLReturnCodes GetStages(Guid userId, Guid raceId, out List<Stage> stages);
        BLReturnCodes GetStageDetails(Guid userId, Guid stageId, Guid raceId, out Stage stage);
        BLReturnCodes AddStage(Guid userId, Stage stage, Guid raceId);
        BLReturnCodes DeleteStage(Guid userId, Guid stageId, Guid raceId);
        BLReturnCodes EditStage(Guid userId, Stage stageNew);
        BLReturnCodes RemoveStages(Guid userId, Guid raceId);
    }
}
