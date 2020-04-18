using Adventure4YouData.Models.Races;
using System;
using System.Collections.Generic;

namespace Adventure4You.Races
{
    public interface IRaceBL : IGenericBL<Race>
    {
        IEnumerable<Race> Get(Guid userId);
        Race GetById(Guid userId, Guid raceId);
    }
}