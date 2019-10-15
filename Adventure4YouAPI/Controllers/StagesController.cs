using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using Adventure4You.Stages;
using Adventure4You.Models;
using Adventure4You.Models.Stages;
using Adventure4YouAPI.ViewModels;
using Adventure4YouAPI.ViewModels.Stages;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Adventure4YouAPI.Controllers
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class StagesController : Adventure4YouControllerBase
    {
        private readonly IStageBL _StageBL;
        private readonly IMapper _Mapper;

        public StagesController(IStageBL stageBL, IMapper mapper)
        {
            _StageBL = stageBL;
            _Mapper = mapper;
        }

        [HttpGet]
        [Route("getracestages")]
        public ActionResult<List<StageViewModel>> GetRaceStages([FromQuery(Name = "raceId")]Guid raceId)
        {
            try
            {
                var retVal = new List<StageViewModel>();
                var stages = new List<Stage>();
                    
                var result = _StageBL.GetStages(GetUserId(), raceId, out stages);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                foreach (var stage in stages)
                {
                    retVal.Add(_Mapper.Map<StageViewModel>(stage));
                }

                return Ok(retVal);
            }
            catch
            { 
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("getstagedetails")]
        public ActionResult<StageDetailViewModel> GetStageDetails([FromQuery(Name = "stageId")]Guid stageId, [FromQuery(Name = "raceId")]Guid raceId)
        {
            try
            {
                var stage = new Stage();

                var result = _StageBL.GetStageDetails(GetUserId(), stageId, raceId, out stage);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                var retVal = _Mapper.Map<StageDetailViewModel>(stage);

                return Ok(retVal);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("addstage")]
        public ActionResult<StageViewModel> AddStage([FromBody]StageDetailViewModel viewModel)
        {
            Thread.Sleep(1000);

            try
            {
                var stage = _Mapper.Map<Stage>(viewModel);

                var result = _StageBL.AddStage(GetUserId(), stage, viewModel.RaceId);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                return Ok(_Mapper.Map<StageViewModel>(stage));
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{stageId}/{raceId}/remove")]
        public ActionResult<Guid> DeleteStage(Guid stageId, Guid raceId)
        {
            try
            {
                var result = _StageBL.DeleteStage(GetUserId(), stageId, raceId);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }
            }
            catch
            {
                return StatusCode(500);
            }

            return Ok(stageId);
        }

        [HttpPut]
        [Route("editstage")]
        public ActionResult<StageDetailViewModel> EditStage([FromBody]StageDetailViewModel viewModel)
        {
            try
            {
                var stage = _Mapper.Map<Stage>(viewModel);

                var result = _StageBL.EditStage(GetUserId(), stage);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                var retVal = _Mapper.Map<StageDetailViewModel>(stage);

                return Ok(retVal);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}