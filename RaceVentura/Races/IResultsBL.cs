using System;
using System.Collections.Generic;
using RaceVentura.Models.Results;

namespace RaceVentura.Races
{
    public interface IResultsBL
    {
        IEnumerable<TeamResult> GetRaceResults(Guid raceId);
    }
}
