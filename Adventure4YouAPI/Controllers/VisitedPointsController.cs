using Adventure4You;
using Adventure4You.Races;
using Adventure4YouAPI.ViewModels;
using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouData.Models.Races;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Adventure4YouAPI.Controllers
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class VisitedPointsController : Adventure4YouControllerBase, ICudController<VisitedPointViewModel>
    {
        private readonly IGenericCudBL<VisitedPoint> _VisitedPointBL;
        private readonly IMapper _Mapper;
        private readonly ILogger _Logger;

        public VisitedPointsController(IGenericCudBL<VisitedPoint> visitedPointBL, IMapper mapper, ILogger<VisitedPointsController> logger)
        {
            _VisitedPointBL = visitedPointBL ?? throw new ArgumentNullException(nameof(visitedPointBL));
            _Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("addvisitedpoint")]
        public IActionResult Add(VisitedPointViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var model = _Mapper.Map<VisitedPoint>(viewModel);

                _VisitedPointBL.Add(GetUserId(), model);

                return Ok(_Mapper.Map<VisitedPointViewModel>(model));
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(VisitedPointsController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public IActionResult Edit([FromBody] VisitedPointViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{teamPointVisitedId}/removepointvisited")]
        public IActionResult Delete(Guid teamPointVisitedId)
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
                _Logger.LogError(ex, $"Error in {typeof(VisitedPointsController)}: {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}
