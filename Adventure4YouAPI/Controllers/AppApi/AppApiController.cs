using Adventure4You;
using Adventure4You.AppApi;
using Adventure4YouAPI.ViewModels;
using Adventure4YouAPI.ViewModels.AppApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Adventure4YouAPI.Controllers.AppApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppApiController : ControllerBase
    {
        private readonly IAppApiBL _AppApiBL;
        private readonly ILogger _Logger;

        public AppApiController(IAppApiBL appApiBL, ILogger<AppApiController> logger)
        {
            _AppApiBL = appApiBL ?? throw new ArgumentNullException(nameof(appApiBL));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("registertorace")]
        public IActionResult RegisterToRace(RegisterToRaceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                _AppApiBL.RegisterToRace(viewModel.RaceId, viewModel.TeamId, viewModel.UniqueId);

                return Ok(viewModel);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(AppApiController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("registerpoint")]
        public IActionResult RegisterPoint(RegisterPointViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                viewModel.Question = _AppApiBL.RegisterPoint(
                    viewModel.RaceId, 
                    viewModel.TeamId, 
                    viewModel.UniqueId,
                    viewModel.PointId,
                    viewModel.Latitude,
                    viewModel.Longitude,
                    viewModel.Answer);

                return Ok(viewModel);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(AppApiController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("registerStageEnd")]
        public IActionResult RegisterStageEnd(RegisterStageEndViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                _AppApiBL.RegisterStageEnd(viewModel.RaceId, viewModel.TeamId, viewModel.UniqueId, viewModel.StageId);

                return Ok(viewModel);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(AppApiController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("registerRaceEnd")]
        public IActionResult RegisterRaceEnd(RegisterToRaceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                _AppApiBL.RegisterRaceEnd(viewModel.RaceId, viewModel.TeamId, viewModel.UniqueId);

                return Ok(viewModel);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(AppApiController)}: {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}
