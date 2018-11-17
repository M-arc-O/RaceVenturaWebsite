using Adventure4You.DatabaseContext;
using Adventure4You.Models;
using Adventure4You.ViewModels;
using Adventure4You.ViewModels.Stages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adventure4You.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StageController : ControllerBase
    {
        private readonly Adventure4YouDbContext _Context;

        public StageController(Adventure4YouDbContext context)
        {
            _Context = context;
        }

        [HttpGet]
        [Route("getstages")]
        public ActionResult<List<StageViewModel>> GetStages([FromQuery(Name = "raceId")]int raceId)
        {
            var retVal = new List<StageViewModel>();
            var stageLinks = _Context.StageLinks.Where(link => link.RaceId == raceId);
            foreach (var stage in _Context.Stages.Where(stage => stageLinks.Any(link => link.StageId == stage.Id)))
            {
                retVal.Add(new StageViewModel(stage));
            }

            return Ok(retVal);
        }

        [HttpPost]
        [Route("addstage")]
        public ActionResult<StageViewModel> AddStage([FromBody]AddStageViewModel viewModel)
        {
            var stage = new Stage
            {
                Name = viewModel.Name
            };
            _Context.Stages.Add(stage);
            _Context.SaveChanges();

            var stageLink = new StageLink
            {
                StageId = stage.Id,
                RaceId = viewModel.RaceId
            };
            _Context.StageLinks.Add(stageLink);
            _Context.SaveChanges();

            return Ok(new StageViewModel(stage));
        }

        [HttpPost]
        [Route("closestage")]
        public ActionResult<bool> CloseStage([FromBody]bool viewModel)
        {
            throw new NotImplementedException();
        }
    }
}