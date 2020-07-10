using Microsoft.AspNetCore.Mvc;
using System;
using RaceVenturaAPI.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class StagesController : RacesControllerBase, ICudController<StageViewModel>
    {
        private readonly IGenericCudBL<Stage> _StageBL;
        private readonly IMapper _Mapper;
        private readonly ILogger _Logger;

        public StagesController(IGenericCudBL<Stage> stageBL, IMapper mapper, ILogger<StagesController> logger)
        {
            _StageBL = stageBL ?? throw new ArgumentNullException(nameof(stageBL));
            _Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("addstage")]
        public IActionResult Add(StageViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var stage = _Mapper.Map<Stage>(viewModel);

                _StageBL.Add(GetUserId(), stage);

                return Ok(_Mapper.Map<StageViewModel>(stage));
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(StagesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Route("editstage")]
        public IActionResult Edit(StageViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var stage = _Mapper.Map<Stage>(viewModel);

                _StageBL.Edit(GetUserId(), stage);

                var retVal = _Mapper.Map<StageViewModel>(stage);

                return Ok(retVal);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(StagesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{stageId}/remove")]
        public IActionResult Delete(Guid stageId)
        {
            try
            {
                _StageBL.Delete(GetUserId(), stageId);

                return Ok(stageId);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(StagesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}