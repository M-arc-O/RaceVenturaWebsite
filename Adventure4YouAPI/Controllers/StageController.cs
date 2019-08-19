using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;

using Adventure4You;
using Adventure4You.Models;
using Adventure4You.Models.Stages;
using Adventure4YouAPI.ViewModels;
using Adventure4YouAPI.ViewModels.Stages;
using AutoMapper;

namespace Adventure4YouAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StageController : ControllerBase
    {
        private readonly IStageBL _StageBL;
        private readonly IMapper _Mapper;

        public StageController(IStageBL stageBL, IMapper mapper)
        {
            _StageBL = stageBL;
            _Mapper = mapper;
        }

        [HttpGet]
        [Route("getstages")]
        public ActionResult<List<StageViewModel>> GetStages([FromQuery(Name = "raceId")]Guid raceId)
        {
            Thread.Sleep(1000);
            try
            {
                var retVal = new List<StageViewModel>();
                var stages = new List<Stage>();
                    
                var result = _StageBL.GetStages(raceId, out stages);
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

        [HttpPost]
        [Route("addstage")]
        public ActionResult<StageViewModel> AddStage([FromBody]AddStageViewModel viewModel)
        {
            Thread.Sleep(1000);

            try
            {
                var stageModel = _Mapper.Map<Stage>(viewModel);

                var result = _StageBL.AddRace(stageModel, viewModel.RaceId);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                return Ok(_Mapper.Map<StageViewModel>(stageModel));
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("closestage")]
        public ActionResult<bool> CloseStage([FromBody]bool viewModel)
        {
            throw new NotImplementedException();
        }
    }
}