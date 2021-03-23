using RaceVentura.Models.RaceAccess;
using RaceVenturaData.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RaceVentura.Races
{
    public interface IRaceAccessBL
    {
        Task<IEnumerable<RaceAccess>> Get(Guid userId, Guid raceId);
        RaceAccessLevel GetAccessLevel(Guid userId, Guid raceId);
        Task Add(Guid userId, Guid raceId, string emailaddress, RaceAccessLevel raceAccessLevel);
        Task Edit(Guid userId, Guid raceId, string emailaddress, RaceAccessLevel raceAccessLevel);
        Task Delete(Guid userId, Guid raceId, string emailaddress);
    }
}
