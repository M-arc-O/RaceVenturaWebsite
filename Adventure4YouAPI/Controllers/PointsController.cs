using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Adventure4You.Points;
using Adventure4YouData.Models;
using Adventure4YouData.Models.Points;
using Adventure4YouAPI.ViewModels;
using Adventure4YouAPI.ViewModels.Points;

namespace Adventure4YouAPI.Controllers
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class PointsController : Adventure4YouControllerBase
    {
        private readonly IPointBL _PointBL;
        private readonly IMapper _Mapper;

        public PointsController(IPointBL pointBL, IMapper mapper)
        {
            _PointBL = pointBL;
            _Mapper = mapper;
        }

        [HttpGet]
        [Route("getstagepoints")]
        public ActionResult<List<PointDetailViewModel>> GetStagePoints([FromQuery(Name = "stageId")]Guid stageId)
        {
            try
            {
                var retVal = new List<PointDetailViewModel>();
                var points = new List<Point>();

                var result = _PointBL.GetPoints(GetUserId(), stageId, out points);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                foreach (var point in points)
                {
                    retVal.Add(_Mapper.Map<PointDetailViewModel>(point));
                }

                return Ok(retVal);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("getpointdetails")]
        public ActionResult<PointDetailViewModel> GetPointDetails([FromQuery(Name = "stageId")]Guid stageId, [FromQuery(Name = "pointId")]Guid pointId)
        {
            try
            {
                var point = new Point();

                var result = _PointBL.GetPointDetails(GetUserId(), stageId, pointId, out point);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                var retVal = _Mapper.Map<PointDetailViewModel>(point);

                return Ok(retVal);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("addpoint")]
        public ActionResult<PointDetailViewModel> AddPoint([FromBody]PointDetailViewModel viewModel)
        {
            try
            {
                var point = _Mapper.Map<Point>(viewModel);

                var result = _PointBL.AddPoint(GetUserId(), point);

                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                return Ok(_Mapper.Map<PointDetailViewModel>(point));
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{pointId}/{stageId}/remove")]
        public ActionResult<Guid> DeletePoint(Guid pointId, Guid stageId)
        {
            try
            {
                var result = _PointBL.DeletePoint(GetUserId(), pointId, stageId);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }
            }
            catch
            {
                return StatusCode(500);
            }

            return Ok(pointId);
        }

        [HttpPut]
        [Route("editpoint")]
        public ActionResult<PointDetailViewModel> EditPoint([FromBody]PointDetailViewModel viewModel)
        {
            try
            {
                var pointModel = _Mapper.Map<Point>(viewModel);

                var result = _PointBL.EditPoint(GetUserId(), pointModel);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                var retVal = _Mapper.Map<PointDetailViewModel>(pointModel);

                return Ok(retVal);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}