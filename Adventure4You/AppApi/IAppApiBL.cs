using System;

namespace Adventure4You.AppApi
{
    public interface IAppApiBL
    {
        void RegisterToRace(Guid raceId, Guid teamId, string uniqueId);
        string RegisterPoint(Guid raceId, Guid teamId, string uniqueId, Guid pointId, double latitude, double longitude, string answer);
        void RegisterStageEnd(Guid raceId, Guid teamId, string uniqueId, Guid stageId);
        void RegisterRaceEnd(Guid raceId, Guid teamId, string uniqueId);
    }
}
