using RaceVenturaData.Models;
using System;

namespace RaceVentura.Models.RaceAccess
{
    public class RaceAccess
    {
        public Guid RaceId { get; set; }
        public string Email { get; set; }
        public RaceAccessLevel RaceAccessLevel { get; set; }
    }
}
