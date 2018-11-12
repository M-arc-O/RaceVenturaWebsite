using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Adventure4You.DatabaseContext;
using Adventure4You.ViewModels;
using Adventure4You.Models;
using System.Linq;
using System;

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

            return retVal;
        }

        [HttpPost]
        [Route("addRace")]
        public ActionResult<RaceViewModel> AddRace([FromBody]AddRaceViewModel viewModel)
        {
            var id = User.FindFirst("id")?.Value;
            var race = new Race
            {
                Name = viewModel.Name,
                CoordinatesCheckEnabled = viewModel.CoordinatesCheckEnabled,
                SpecialTasksAreStage = viewModel.SpecialTasksAreStage
            };

            try
            {
                _Context.Races.Add(race);
                _Context.SaveChanges();

                _Context.UserLinks.Add(new UserLink
                {
                    RaceId = race.Id,
                    UserId = id
                });

                _Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

            return Ok(new RaceViewModel(race));
        }

        [HttpGet]
        [Route("getRaceDetails")]
        public ActionResult<RaceDetailViewModel> GetRaceDetails(int raceId)
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
