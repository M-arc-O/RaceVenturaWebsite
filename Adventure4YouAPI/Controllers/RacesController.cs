using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouAPI.ViewModels;
using Adventure4YouAPI.Helpers;
using Adventure4You;
using Adventure4You.Models;
using AutoMapper;

namespace Adventure4YouAPI.Controllers
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class RacesController : Adventure4YouControllerBase
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
            try
            {
                List<Race> races;
                var result = _RaceBL.GetAllRaces(GetUserId(), out races);

                if (result == BLReturnCodes.Ok)
                {
                    var retVal = new List<RaceViewModel>();

                    foreach (var race in races)
                    {
                        retVal.Add(_Mapper.Map<RaceViewModel>(race));
                    }

                    return Ok(retVal);
                }

                return BadRequest((ErrorCodes)result);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet()]
        [Route("getracedetails")]
        public ActionResult<RaceDetailViewModel> GetRaceDetails([FromQuery(Name = "raceId")]Guid raceId)
        {
            try
            {
                var raceModel = new Race();

                var result = _RaceBL.GetRaceDetails(GetUserId(), raceId, out raceModel);
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
        [Route("addrace")]
        public ActionResult<RaceViewModel> AddRace([FromBody]RaceDetailViewModel viewModel)
        {
            try
            {
                var raceModel = _Mapper.Map<Race>(viewModel);

                var result = _RaceBL.AddRace(GetUserId(), raceModel);
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

        [HttpDelete]
        [Route("{raceId}/remove")]
        public ActionResult<Guid> DeleteRace(Guid raceId)
        {
            try
            {
                var result = _RaceBL.DeleteRace(GetUserId(), raceId);
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

        [HttpPut]
        [Route("editrace")]
        public ActionResult<RaceDetailViewModel> EditRace([FromBody]RaceDetailViewModel viewModel)
        {
            try
            {
                var raceModel = _Mapper.Map<Race>(viewModel);

                var result = _RaceBL.EditRace(GetUserId(), raceModel);
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
    }
}
