using Adventure4YouAPI.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Adventure4You;
using Adventure4YouData.Models.Races;
using Adventure4You.Races;
using Microsoft.Extensions.Logging;
using Adventure4YouAPI.ViewModels.Races;

namespace Adventure4YouAPI.Controllers
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : Adventure4YouControllerBase
    {
        private readonly IGenericBL<Team> _TeamBL;
        private readonly IGenericBL<VisitedPoint> _VisitedPointBL;
        private readonly IMapper _Mapper;
        private readonly ILogger _Logger;

        public TeamsController(IGenericBL<Team> teamBL, IGenericBL<VisitedPoint> visitedPointBL, IMapper mapper, ILogger logger)
        {
            _TeamBL = teamBL ?? throw new ArgumentNullException(nameof(teamBL));
            _VisitedPointBL = visitedPointBL ?? throw new ArgumentNullException(nameof(visitedPointBL));
            _Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        [HttpPost]
        [Route("addteam")]
        public ActionResult<TeamViewModel> AddTeam([FromBody]TeamViewModel viewModel)
        {
            try
            {
                var team = _Mapper.Map<Team>(viewModel);

                _TeamBL.Add(GetUserId(), team);

                return Ok(_Mapper.Map<TeamViewModel>(team));
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(TeamsController)}: {ex.Message}");
                return StatusCode(500);
            }
        }


        [HttpPut]
        [Route("editteam")]
        public ActionResult<TeamViewModel> EditTeam([FromBody]TeamViewModel viewModel)
        {
            try
            {
                var team = _Mapper.Map<Team>(viewModel);

                _TeamBL.Edit(GetUserId(), team);

                return Ok(_Mapper.Map<TeamViewModel>(team));
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(PointsController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{teamId}/removeteam")]
        public ActionResult<Guid> DeleteTeam(Guid teamId)
        {
            try
            {
                _TeamBL.Delete(GetUserId(), teamId);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(PointsController)}: {ex.Message}");
                return StatusCode(500);
            }

            return Ok(teamId);
        }

        [HttpPost]
        [Route("addvisitedpoint")]
        public ActionResult<VisitedPointViewModel> AddVisitedPoint(VisitedPointViewModel viewModel)
        {
            try
            {
                var model = _Mapper.Map<VisitedPoint>(viewModel);

                _VisitedPointBL.Add(GetUserId(), model);

                return Ok(model);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(PointsController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{teamPointVisitedId}/{teamId}/{raceId}/removepointvisited")]
        public ActionResult<Guid> DeletePointVisited(Guid teamPointVisitedId, Guid teamId, Guid raceId)
        {
            try
            {
                _VisitedPointBL.Delete(GetUserId(), teamPointVisitedId);

                return Ok(teamPointVisitedId);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(PointsController)}: {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}