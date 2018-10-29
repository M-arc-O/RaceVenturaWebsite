using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Adventure4You.DatabaseContext;
using Adventure4You.ViewModels;

namespace Adventure4You.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RacesController : ControllerBase
    {
        private readonly Adventure4YouDbContext _Context;

        public RacesController(Adventure4YouDbContext context)
        {
            _Context = context;
        }

        [HttpGet]
        [Route("getallraces")]
        public ActionResult<List<RaceViewModel>> GetAllRaces()
        {
            var retVal = new List<RaceViewModel>();

            foreach (var race in _Context.Races)
            {
                retVal.Add(new RaceViewModel
                {
                    RaceId = race.RaceId,
                    RaceName = race.RaceName,
                    RaceCoordinatesCheckEnabled = race.RaceCoordinatesCheckEnabled
                });
            }

            return retVal;
        }
    }
}
