using RaceVenturaAPI.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using RaceVentura;
using RaceVenturaData.Models.Races;
using RaceVentura.Races;
using Microsoft.Extensions.Logging;
using RaceVenturaAPI.ViewModels.Races;

namespace RaceVenturaAPI.Controllers.Races
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : RacesControllerBase, ICudController<TeamViewModel>
    {
        private readonly IGenericCudBL<Team> _TeamBL;
        private readonly IMapper _Mapper;
        private readonly ILogger _Logger;

        public TeamsController(IGenericCudBL<Team> teamBL, IMapper mapper, ILogger<TeamsController> logger)
        {
            _TeamBL = teamBL ?? throw new ArgumentNullException(nameof(teamBL));
            _Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        [HttpPost]
        [Route("addteam")]
        public IActionResult Add([FromBody]TeamViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

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
        public IActionResult Edit([FromBody]TeamViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

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
        public IActionResult Delete(Guid teamId)
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