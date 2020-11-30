using System;

namespace RaceVentura.AppApi
{
    public interface IAppApiBL
    {
        void RegisterToRace(Guid raceId, Guid teamId, string uniqueId);
        string RegisterPoint(Guid raceId, string uniqueId, Guid pointId, double latitude, double longitude, string answer);
        void RegisterStageEnd(Guid raceId, string uniqueId, Guid stageId);
        void RegisterRaceEnd(Guid raceId, string uniqueId);
    }
}
