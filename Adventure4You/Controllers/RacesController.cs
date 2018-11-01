using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Adventure4You.DatabaseContext;
using Adventure4You.ViewModels;

namespace Adventure4You.Controllers
{
    [Authorize(Policy = "RaceUser")]
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
                    Id = race.Id,
                    Name = race.Name,
                    CoordinatesCheckEnabled = race.CoordinatesCheckEnabled,
                    SpecialTasksAreStage = race.SpecialTasksAreStage
                });
            }

            return retVal;
        }
    }
}
