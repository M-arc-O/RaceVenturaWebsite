using Adventure4You;
using Adventure4You.Models;
using Adventure4You.Models.Teams;
using Adventure4YouAPI.ViewModels;
using Adventure4YouAPI.ViewModels.Teams;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Adventure4YouAPI.Controllers
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : Adventure4YouControllerBase
    {
        private readonly ITeamBL _TeamBL;
        private readonly IMapper _Mapper;


        public TeamsController(ITeamBL teamBL, IMapper mapper)
        {
            _TeamBL = teamBL;
            _Mapper = mapper;
        }

        [HttpGet]
        [Route("getraceteams")]
        public ActionResult<List<TeamViewModel>> GetRaceTeams([FromQuery(Name = "raceId")]Guid raceId)
        {
            try
            {
                var retVal = new List<TeamViewModel>();
                var teams = new List<Team>();

                var result = _TeamBL.GetTeams(GetUserId(), raceId, out teams);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                foreach (var team in teams)
                {
                    retVal.Add(_Mapper.Map<TeamViewModel>(team));
                }

                return Ok(retVal);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("getteamdetails")]
        public ActionResult<TeamDetailViewModel> GetTeamDetails([FromQuery(Name = "teamId")]Guid stageId, [FromQuery(Name = "raceId")]Guid raceId)
        {
            try
            {
                var team = new Team();

                var result = _TeamBL.GetTeamDetails(GetUserId(), stageId, raceId, out team);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                var retVal = _Mapper.Map<TeamDetailViewModel>(team);

                return Ok(retVal);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("addteam")]
        public ActionResult<TeamViewModel> AddTeam([FromBody]TeamDetailViewModel viewModel)
        {
            Thread.Sleep(1000);

            try
            {
                var team = _Mapper.Map<Team>(viewModel);

                var result = _TeamBL.AddTeam(GetUserId(), team, viewModel.RaceId);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                return Ok(_Mapper.Map<TeamViewModel>(team));
            }
            catch
            {
                return StatusCode(500);
            }
        }


        [HttpDelete]
        [Route("{teamId}/{raceId}/remove")]
        public ActionResult<Guid> DeleteTeam(Guid teamId, Guid raceId)
        {
            try
            {
                var result = _TeamBL.DeleteTeam(GetUserId(), teamId, raceId);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }
            }
            catch
            {
                return StatusCode(500);
            }

            return Ok(teamId);
        }

        [HttpPut]
        [Route("editteam")]
        public ActionResult<TeamDetailViewModel> EditTeam([FromBody]TeamDetailViewModel viewModel)
        {
            try
            {
                var team = _Mapper.Map<Team>(viewModel);

                var result = _TeamBL.EditTeam(GetUserId(), team);
                if (result != BLReturnCodes.Ok)
                {
                    return BadRequest((ErrorCodes)result);
                }

                var retVal = _Mapper.Map<TeamDetailViewModel>(team);

                return Ok(retVal);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}