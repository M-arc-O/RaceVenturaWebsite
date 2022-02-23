using RaceVenturaData.Models.Races;
using System;

namespace RaceVentura.AppApi
{
    public interface IAppApiBL
    {
        string RegisterToRace(Guid raceId, Guid teamId, Guid uniqueId);
        Point RegisterPoint(Guid raceId, Guid uniqueId, Guid pointId, double latitude, double longitude, string answer);
        void RegisterStageStart(Guid raceId, Guid uniqueId, Guid stageId);
        void RegisterRaceEnd(Guid raceId, Guid uniqueId);
    }
}
