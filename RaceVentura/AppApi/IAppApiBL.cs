﻿using System;

namespace RaceVentura.AppApi
{
    public interface IAppApiBL
    {
        void RegisterToRace(Guid raceId, Guid teamId, Guid uniqueId);
        string RegisterPoint(Guid raceId, Guid uniqueId, Guid pointId, double latitude, double longitude, string answer);
        void RegisterStageEnd(Guid raceId, Guid uniqueId, Guid stageId);
        void RegisterRaceEnd(Guid raceId, Guid uniqueId);
    }
}
