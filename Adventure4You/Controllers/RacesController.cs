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
                retVal.Add(new RaceViewModel
                {
                    Id = race.Id,
                    Name = race.Name
                });
            }

            return retVal;
        }

        [HttpPost]
        [Route("addRace")]
        public ActionResult<bool> AddRace([FromBody]AddRaceViewModel viewModel)
        {
            var id = User.FindFirst("id")?.Value;

            try
            {
                _Context.Races.Add(new Race
                {
                    Name = viewModel.Name,
                    CoordinatesCheckEnabled = viewModel.CoordinatesCheckEnabled,
                    SpecialTasksAreStage = viewModel.SpecialTasksAreStage
                });

                _Context.UserLinks.Add(new UserLink
                {
                    RaceId = _Context.Races.ToList().Last<Race>().Id,
                    UserId = int.Parse(id)
                });

                _Context.SaveChanges();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok(true);
        }

        [HttpGet]
        [Route("getRaceDetails")]
        public ActionResult<RaceDetailViewModel> GetRaceDetails(int userId, int raceId)
        {
            var id = User.FindFirst("id")?.Value;

            if (_Context.UserLinks.FirstOrDefault(link => link.UserId == userId && link.RaceId == raceId) != null)
            {
                var race = _Context.Races.First(model => model.Id == raceId);

                return Ok(new RaceDetailViewModel
                {
                    Id = race.Id,
                    Name = race.Name,
                    CoordinatesCheckEnabled = race.CoordinatesCheckEnabled,
                    SpecialTasksAreStage = race.CoordinatesCheckEnabled,
                    StartTime = race.StartTime,
                    EndTime = race.EndTime
                });
            }

            return NotFound();
        }
    }
}
