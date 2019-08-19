using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Adventure4You;
using Adventure4You.Models;
using Adventure4You.Models.Points;
using Adventure4YouAPI.ViewModels;
using Adventure4YouAPI.ViewModels.Points;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Adventure4YouAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointController : ControllerBase
    {
        private readonly IPointBL _PointBL;
        private readonly IMapper _Mapper;

        public PointController(IPointBL pointBL, IMapper mapper)
        {
            _PointBL = pointBL;
            _Mapper = mapper;
        }

        [HttpGet]
        [Route("getpoints")]
        public ActionResult<List<PointViewModel>> GetPoints([FromQuery(Name = "stageId")]Guid stageId)
        {
            Thread.Sleep(1000);

            try
            {
                var retVal = new List<PointViewModel>();
                var points = new List<Point>();

                var result = _PointBL.GetPoint(stageId, out points);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                foreach (var point in points)
                {
                    retVal.Add(_Mapper.Map<PointViewModel>(point));
                }

                return Ok(retVal);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("addpoint")]
        public ActionResult<PointViewModel> AddPoint([FromBody]AddPointViewModel viewModel)
        {
            Thread.Sleep(1000);

            try
            {
                var point = _Mapper.Map<Point>(viewModel);

                var result = _PointBL.AddPoint(point, viewModel.StageId);

                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                return Ok(_Mapper.Map<PointViewModel>(point));
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("visitedpoint")]
        public ActionResult<bool> VisitedPoint()
        {
            throw new NotImplementedException();
        }
    }
}