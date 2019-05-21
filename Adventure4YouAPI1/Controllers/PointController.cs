using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Adventure4YouAPI.DatabaseContext;
using Adventure4YouAPI.Models;
using Adventure4YouAPI.ViewModels.Points;
using Microsoft.AspNetCore.Mvc;

namespace Adventure4YouAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointController : ControllerBase
    {
        private readonly Adventure4YouDbContext _Context;

        public PointController(Adventure4YouDbContext context)
        {
            _Context = context;
        }

        [HttpGet]
        [Route("getpoints")]
        public ActionResult<List<PointViewModel>> GetPoints([FromQuery(Name = "stageId")]int stageId)
        {
            Thread.Sleep(1000);

            var retVal = new List<PointViewModel>();
            var pointLinks = _Context.PointLinks.Where(link => link.StageId == stageId);
            foreach (var point in _Context.Points.Where(point => pointLinks.Any(link => link.StageId == point.Id)))
            {
                retVal.Add(new PointViewModel(point));
            }

            return Ok(retVal);
        }

        [HttpPost]
        [Route("addpoint")]
        public ActionResult<PointViewModel> AddPoint([FromBody]AddPointViewModel viewModel)
        {
            Thread.Sleep(1000);

            var point = new Point
            {
                Name = viewModel.Name,
                Value = viewModel.Value,
                Coordinates = viewModel.Coordinates
            };
            _Context.Points.Add(point);
            _Context.SaveChanges();

            var pointLink = new PointLink
            {
                PointId = point.Id,
                StageId = viewModel.StageId
            };
            _Context.PointLinks.Add(pointLink);
            _Context.SaveChanges();

            return Ok(new PointViewModel(point));
        }

        [HttpPost]
        [Route("visitedpoint")]
        public ActionResult<bool> VisitedPoint()
        {
            throw new NotImplementedException();
        }
    }
}