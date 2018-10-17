using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Adventure4You.DatabaseContext;
using Adventure4You.ViewModels;

namespace Adventure4You.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly Adventure4YouContext _Context;

        public ValuesController(Adventure4YouContext context)
        {
            _Context = context;
        }

        [HttpGet]
        public ActionResult<List<RaceViewModel>> GetAllRaces()
        {
            var retVal = new List<RaceViewModel>();

            foreach (var race in _Context.Races)
            {
                retVal.Add(new RaceViewModel
                {
                    RaceId = race.RaceId,
                    RaceName = race.RaceName,
                    RaceGuid = race.RaceGuid,
                    RaceCoordinatesCheckEnabled = race.RaceCoordinatesCheckEnabled
                });
            }

            return retVal;
        }
    }
}
