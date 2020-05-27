using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Adventure4YouAPI.ViewModels;
using Adventure4You;
using Adventure4You.Races;
using Adventure4YouData.Models.Races;
using Microsoft.Extensions.Logging;
using Adventure4YouAPI.ViewModels.Races;

namespace Adventure4YouAPI.Controllers
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class PointsController : Adventure4YouControllerBase, ICudController<PointViewModel>
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
        public IActionResult Add([FromBody]PointViewModel viewModel)
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
        public IActionResult Edit([FromBody]PointViewModel viewModel)
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