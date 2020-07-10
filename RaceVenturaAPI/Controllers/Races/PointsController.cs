using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using RaceVenturaAPI.ViewModels;
using RaceVentura;
using RaceVentura.Races;
using RaceVenturaData.Models.Races;
using Microsoft.Extensions.Logging;
using RaceVenturaAPI.ViewModels.Races;

namespace RaceVenturaAPI.Controllers.Races
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class PointsController : RacesControllerBase, ICudController<PointViewModel>
    {
        private readonly IGenericCudBL<Point> _PointBL;
        private readonly IMapper _Mapper;
        private readonly ILogger _Logger;

        public PointsController(IGenericCudBL<Point> pointBL, IMapper mapper, ILogger<PointsController> logger)
        {
            _PointBL = pointBL ?? throw new ArgumentNullException(nameof(pointBL));
            _Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("addpoint")]
        public IActionResult Add(PointViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var point = _Mapper.Map<Point>(viewModel);

                _PointBL.Add(GetUserId(), point);

                return Ok(_Mapper.Map<PointViewModel>(point));
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

        [HttpPut]
        [Route("editpoint")]
        public IActionResult Edit(PointViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var pointModel = _Mapper.Map<Point>(viewModel);

                _PointBL.Edit(GetUserId(), pointModel);

                return Ok(_Mapper.Map<PointViewModel>(pointModel));
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
        [Route("{pointId}/remove")]
        public IActionResult Delete(Guid pointId)
        {
            try
            {
                _PointBL.Delete(GetUserId(), pointId);

                return Ok(pointId);
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