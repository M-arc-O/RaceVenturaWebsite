using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Adventure4You.DatabaseContext;
using Adventure4You.ViewModels;
using System.Linq;

namespace Adventure4You.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RacesController : ControllerBase
    {
        private readonly Adventure4YouContext _Context;

        public RacesController(Adventure4YouContext context)
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
                    RaceName = race.RaceName
                });
            }

            return retVal;
        }

        [HttpGet]
        [Route("getrace")]
        public ActionResult<RaceAdminViewModel> GetRace(int id)
        {
            var model = _Context.Races.FirstOrDefault(race => race.RaceId == id);

            if (model != null)
            {
                return Ok(new RaceAdminViewModel
                {
                    RaceId = model.RaceId,
                    RaceName = model.RaceName,
                    RaceCoordinatesCheckEnabled = model.RaceCoordinatesCheckEnabled
                });
            }
            else
            {
                return NotFound();
            }
        }
    }
}
