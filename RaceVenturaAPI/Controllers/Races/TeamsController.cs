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
using RaceVenturaAPI.Extensions;

namespace RaceVenturaAPI.Controllers.Races
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : RacesControllerBase, ICudController<TeamViewModel>
    {
        private readonly IGenericCudBL<Team> _teamBL;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TeamsController(IGenericCudBL<Team> teamBL, IMapper mapper, ILogger<TeamsController> logger)
        {
            _teamBL = teamBL ?? throw new ArgumentNullException(nameof(teamBL));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        [HttpPost]
        [Route("addteam")]
        public IActionResult Add(TeamViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var team = _mapper.Map<Team>(viewModel);

                _teamBL.Add(GetUserId(), team);

                viewModel = _mapper.Map<TeamViewModel>(team);
                viewModel.AddQrCode(viewModel.RaceId);
                return Ok(viewModel);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(TeamsController)}: {ex.Message}");
                return StatusCode(500);
            }
        }


        [HttpPut]
        [Route("editteam")]
        public IActionResult Edit(TeamViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var team = _mapper.Map<Team>(viewModel);

                _teamBL.Edit(GetUserId(), team);

                viewModel = _mapper.Map<TeamViewModel>(team);
                viewModel.AddQrCode(viewModel.RaceId);
                return Ok(viewModel);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(TeamsController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{teamId}/removeteam")]
        public IActionResult Delete(Guid teamId)
        {
            try
            {
                _teamBL.Delete(GetUserId(), teamId);

                return Ok(teamId);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(TeamsController)}: {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}