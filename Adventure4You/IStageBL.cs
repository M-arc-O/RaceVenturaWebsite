
using Adventure4You.Models;
using Adventure4You.Models.Stages;
using System;
using System.Collections.Generic;

namespace Adventure4You
{
    public interface IStageBL
    {
        BLReturnCodes GetStages(Guid raceId, out List<Stage> stages);

        BLReturnCodes AddRace(Stage stageModel, Guid raceId);
    }
}
