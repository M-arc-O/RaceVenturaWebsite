using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouAPI.ViewModels;
using Adventure4You.Races;
using AutoMapper;
using Adventure4You;
using Adventure4YouData.Models.Races;
using Microsoft.Extensions.Logging;

namespace Adventure4YouAPI.Controllers
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class RacesController : Adventure4YouControllerBase
    {
        private readonly IRaceBL _RaceBL;
        private readonly IResultsBL _ResultsBL;
        private readonly IMapper _Mapper;
        private readonly ILogger _Logger;

        public RacesController(IRaceBL raceBL, IResultsBL resultsBL, IMapper mapper, ILogger logger)
        {
            _RaceBL = raceBL ?? throw new ArgumentNullException(nameof(raceBL));
            _ResultsBL = resultsBL ?? throw new ArgumentNullException(nameof(resultsBL));
            _Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("getallraces")]
        public ActionResult<List<RaceViewModel>> GetAllRaces()
        {
            try
            {
                var races = _RaceBL.Get(GetUserId());

                var retVal = new List<RaceViewModel>();

                foreach (var race in races)
                {
                    retVal.Add(_Mapper.Map<RaceViewModel>(race));
                }

                return Ok(retVal);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
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
                var raceModel = _RaceBL.GetById(GetUserId(), raceId);
                return Ok(_Mapper.Map<RaceDetailViewModel>(raceModel));
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
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
                _RaceBL.Add(GetUserId(), raceModel);

                return Ok(_Mapper.Map<RaceViewModel>(raceModel));
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Route("editrace")]
        public ActionResult<RaceDetailViewModel> EditRace([FromBody]RaceDetailViewModel viewModel)
        {
            try
            {
                var raceModel = _Mapper.Map<Race>(viewModel);
                _RaceBL.Edit(GetUserId(), raceModel);

                return Ok(_Mapper.Map<RaceDetailViewModel>(raceModel));
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{raceId}/remove")]
        public ActionResult<Guid> DeleteRace(Guid raceId)
        {
            try
            {
                _RaceBL.Delete(GetUserId(), raceId);

                return Ok(raceId);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("getraceresults")]
        public ActionResult<List<TeamResultViewModel>> GetRaceResults([FromQuery(Name = "raceId")]Guid raceId)
        {
            try
            {
                var result = _ResultsBL.GetRaceResults(GetUserId(), raceId);

                var retVal = new List<TeamResultViewModel>();

                foreach (var teamResult in result)
                {
                    retVal.Add(_Mapper.Map<TeamResultViewModel>(teamResult));
                }

                return Ok(retVal);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}
