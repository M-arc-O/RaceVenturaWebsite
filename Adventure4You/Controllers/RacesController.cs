using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Adventure4You.DatabaseContext;
using Adventure4You.Models;
using Adventure4You.ViewModels.Races;
using System.Threading;
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
                retVal.Add(new RaceViewModel(race));
            }

            return Ok(retVal);
        }

        [HttpPost]
        [Route("addRace")]
        public ActionResult<RaceViewModel> AddRace([FromBody]AddRaceViewModel viewModel)
        {
            var id = User.FindFirst("id")?.Value;
            var raceModel = viewModel.ToRaceModel();

            try
            {
                if (!_Context.Races.Any(race => race.Name.Equals(raceModel.Name)))
                {
                    _Context.Races.Add(raceModel);
                    _Context.SaveChanges();

                    _Context.UserLinks.Add(new UserLink
                    {
                        RaceId = raceModel.Id,
                        UserId = id
                    });

                    _Context.SaveChanges();
                }
                else
                {
                    return BadRequest(ErrorCodes.Duplicate);
                }
            }
            catch
            {
                return StatusCode(500);
            }

            return Ok(new RaceViewModel(raceModel));
        }

        [HttpGet()]
        [Route("getracedetails")]
        public ActionResult<RaceDetailViewModel> GetRaceDetails([FromQuery(Name = "raceId")]int raceId)
        {
            var id = User.FindFirst("id")?.Value;

            if (_Context.UserLinks.FirstOrDefault(link => link.UserId.Equals(id) && link.RaceId == raceId) != null)
            {
                var race = _Context.Races.First(model => model.Id == raceId);

                return Ok(new RaceDetailViewModel(race));
            }

            return NotFound();
        }
    }
}
