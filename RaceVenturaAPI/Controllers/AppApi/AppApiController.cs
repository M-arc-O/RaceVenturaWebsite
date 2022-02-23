﻿using RaceVentura;
using RaceVentura.AppApi;
using RaceVenturaAPI.ViewModels;
using RaceVenturaAPI.ViewModels.AppApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using RaceVenturaAPI.ViewModels.Races;

namespace RaceVenturaAPI.Controllers.AppApi
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
                viewModel.Name = _AppApiBL.RegisterToRace(viewModel.RaceId, viewModel.TeamId, viewModel.UniqueId);
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
                var model = _AppApiBL.RegisterPoint(
                    viewModel.RaceId, 
                    viewModel.UniqueId,
                    viewModel.PointId,
                    viewModel.Latitude,
                    viewModel.Longitude,
                    viewModel.Answer);

                viewModel.Message = model.Message;
                viewModel.Type = (PointTypeViewModel)model.Type;

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
        public IActionResult RegisterStageStart(RegisterStageStartViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                _AppApiBL.RegisterStageStart(viewModel.RaceId, viewModel.UniqueId, viewModel.StageId);

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
        public IActionResult RegisterRaceEnd(RegisterRaceEndViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                _AppApiBL.RegisterRaceEnd(viewModel.RaceId, viewModel.UniqueId);

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
