
using Adventure4You.Models;
using Adventure4You.Models.Stages;
using System.Collections.Generic;

namespace Adventure4You
{
    public interface IStageBL
    {
        BLReturnCodes GetStages(int raceId, out List<Stage> stages);

        BLReturnCodes AddRace(Stage stageModel, int raceId);
    }
}
