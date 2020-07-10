using RaceVentura;
using RaceVentura.Races;
using RaceVenturaAPI.ViewModels;
using RaceVenturaAPI.ViewModels.Races;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace RaceVenturaAPI.Controllers.Races
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : RacesControllerBase
    {
        private readonly IResultsBL _ResultsBL;
        private readonly IMapper _Mapper;
        private readonly ILogger _Logger;

        public ResultsController(IResultsBL resultsBL, IMapper mapper, ILogger<ResultsController> logger)
        {
            _ResultsBL = resultsBL ?? throw new ArgumentNullException(nameof(resultsBL));
            _Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
