using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouAPI.ViewModels;
using Adventure4You;
using Adventure4You.Models;
using AutoMapper;
using System;

namespace Adventure4YouAPI.Controllers
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class RacesController : ControllerBase
    {
        private readonly IRaceBL _RaceBL;
        private readonly IMapper _Mapper;

        public RacesController(IRaceBL raceBL, IMapper mapper)
        {
            _RaceBL = raceBL;
            _Mapper = mapper;
        }

        [HttpGet]
        [Route("getallraces")]
        public ActionResult<List<RaceViewModel>> GetAllRaces()
        {
            var retVal = new List<RaceViewModel>();

            try
            {
                foreach (var race in _RaceBL.GetAllRaces())
                {
                    retVal.Add(_Mapper.Map<RaceViewModel>(race));
                }
            }
            catch
            {
                return StatusCode(500);
            }

            return Ok(retVal);
        }

        [HttpGet()]
        [Route("getracedetails")]
        public ActionResult<RaceDetailViewModel> GetRaceDetails([FromQuery(Name = "raceId")]Guid raceId)
        {
            try
            {
                var id = GetUserId();
                var raceModel = new Race();

                var result = _RaceBL.GetRaceDetails(id, raceId, out raceModel);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                return Ok(_Mapper.Map<RaceDetailViewModel>(raceModel));
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("addRace")]
        public ActionResult<RaceViewModel> AddRace([FromBody]RaceDetailViewModel viewModel)
        {
            try
            {
                var id = GetUserId();
                var raceModel = _Mapper.Map<Race>(viewModel);

                var result = _RaceBL.AddRace(id, raceModel);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                return Ok(_Mapper.Map<RaceViewModel>(raceModel));
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("deleterace")]
        public ActionResult<RaceDetailViewModel> DeleteRace([FromBody]Guid raceId)
        {
            try
            {
                var id = GetUserId();

                var result = _RaceBL.DeleteRace(id, raceId);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
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
            try
            {
                var id = GetUserId();
                var raceModel = _Mapper.Map<Race>(viewModel);

                var result = _RaceBL.EditRace(id, raceModel);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                return Ok(_Mapper.Map<RaceDetailViewModel>(raceModel));
            }
            catch
            {
                return StatusCode(500);
            }
        }

        private Guid GetUserId()
        {
            return new Guid(User.FindFirst("id")?.Value);
        }
    }
}
