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
    public class TeamsController : Adventure4YouControllerBase, ICudController<TeamViewModel>
    {
        private readonly IGenericCudBL<Team> _TeamBL;
        private readonly IMapper _Mapper;
        private readonly ILogger _Logger;

        public TeamsController(IGenericCudBL<Team> teamBL, IMapper mapper, ILogger logger)
        {
            _TeamBL = teamBL ?? throw new ArgumentNullException(nameof(teamBL));
            _Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        [HttpPost]
        [Route("addteam")]
        public ActionResult<TeamViewModel> Add([FromBody]TeamViewModel viewModel)
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
        public ActionResult<TeamViewModel> Edit([FromBody]TeamViewModel viewModel)
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
                _Logger.LogError(ex, $"Error in {typeof(TeamsController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{teamId}/removeteam")]
        public ActionResult<Guid> Delete(Guid teamId)
        {
            try
            {
                _TeamBL.Delete(GetUserId(), teamId);

                return Ok(teamId);
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
    }
}