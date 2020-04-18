using Microsoft.AspNetCore.Mvc;
using System;
using Adventure4YouAPI.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class StagesController : Adventure4YouControllerBase
    {
        private readonly IGenericBL<Stage> _StageBL;
        private readonly IMapper _Mapper;
        private readonly ILogger _Logger;

        public StagesController(IGenericBL<Stage> stageBL, IMapper mapper, ILogger logger)
        {
            _StageBL = stageBL ?? throw new ArgumentNullException(nameof(stageBL));
            _Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("addstage")]
        public ActionResult<StageViewModel> AddStage([FromBody]StageViewModel viewModel)
        {
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
        public ActionResult<StageViewModel> EditStage([FromBody]StageViewModel viewModel)
        {
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
        public ActionResult<Guid> DeleteStage(Guid stageId)
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