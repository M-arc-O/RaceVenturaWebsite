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
        public ActionResult<RaceViewModel> AddRace([FromBody]RaceDetailViewModel viewModel)
        {
            var id = GetUserId();
            var raceModel = viewModel.ToRaceModel();

            try
            {
                if (!CheckIfRaceNameIsTaken(raceModel.Name))
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

        [HttpPost]
        [Route("deleterace")]
        public ActionResult<RaceDetailViewModel> DeleteRace([FromBody]int raceId)
        {
            var id = GetUserId();

            var raceModel = new Race();
            try
            {
                raceModel = GetRaceModel(raceId);

                if (raceModel != null)
                {
                    var userLink = CheckIfUserHasAccessToRace(id, raceModel.Id);
                    if (userLink == null)
                    {
                        return BadRequest(ErrorCodes.UserUnauthorized);
                    }
                    else
                    {
                        _Context.UserLinks.Remove(userLink);
                    }

                    _Context.Races.Remove(raceModel);

                    _Context.SaveChanges();
                }
                else
                {
                    return BadRequest(ErrorCodes.UnknownRace);
                }
            }
            catch
            {
                return StatusCode(500);
            }

            return Ok(raceId);
        }

        [HttpPost]
        [Route("editrace")]
        public ActionResult<RaceDetailViewModel> EditRace([FromBody]RaceDetailViewModel viewModel)
        {
            var id = GetUserId();

            var raceModel = new Race();
            try
            {
                raceModel = GetRaceModel(viewModel.Id);

                if (raceModel != null)
                {
                    if (CheckIfUserHasAccessToRace(id, raceModel.Id) == null)
                    {
                        return BadRequest(ErrorCodes.UserUnauthorized);
                    }
                    if (!CheckIfRaceNameIsTaken(viewModel.Name))
                    {
                        raceModel.Name = viewModel.Name;
                        raceModel.CoordinatesCheckEnabled = viewModel.CoordinatesCheckEnabled;
                        raceModel.SpecialTasksAreStage = viewModel.SpecialTasksAreStage;
                        raceModel.MaximumTeamSize = viewModel.MaximumTeamSize;
                        raceModel.MinimumPointsToCompleteStage = viewModel.MinimumPointsToCompleteStage;
                        raceModel.StartTime = viewModel.StartTime;
                        raceModel.EndTime = viewModel.EndTime;
                        _Context.SaveChanges();
                    }
                    else
                    {
                        return BadRequest(ErrorCodes.Duplicate);
                    }
                }
                else
                {
                    return BadRequest(ErrorCodes.UnknownRace);
                }
            }
            catch
            {
                return StatusCode(500);
            }

            return Ok(new RaceDetailViewModel(raceModel));
        }

        [HttpGet()]
        [Route("getracedetails")]
        public ActionResult<RaceDetailViewModel> GetRaceDetails([FromQuery(Name = "raceId")]int raceId)
        {
            var id = GetUserId();

            if (CheckIfUserHasAccessToRace(id, raceId) != null)
            {
                var race = _Context.Races.First(model => model.Id == raceId);

                return Ok(new RaceDetailViewModel(race));
            }

            return NotFound();
        }

        private string GetUserId()
        {
            return User.FindFirst("id")?.Value;
        }

        private Race GetRaceModel(int raceId)
        {
            return _Context.Races.FirstOrDefault(race => race.Id == raceId);
        }

        private UserLink CheckIfUserHasAccessToRace(string userId, int raceId)
        {
            return _Context.UserLinks.FirstOrDefault(link => link.UserId == userId && link.RaceId == raceId);
        }

        private bool CheckIfRaceNameIsTaken(string name)
        {
            return _Context.Races.Any(race => race.Name.Equals(name));
        }
    }
}
