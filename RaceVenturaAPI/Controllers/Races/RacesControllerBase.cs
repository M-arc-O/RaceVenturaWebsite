using Microsoft.AspNetCore.Mvc;
using System;

namespace RaceVenturaAPI.Controllers.Races
{
    public abstract class RacesControllerBase: ControllerBase
    {
        protected Guid GetUserId()
        {
            return new Guid(User.FindFirst("id")?.Value);
        }
    }
}
