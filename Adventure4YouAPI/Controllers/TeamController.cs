using Adventure4You;
using Adventure4YouAPI.ViewModels.Teams;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Adventure4YouAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamBL _TeamBL;
        private readonly IMapper _Mapper;


        public TeamController(ITeamBL teamBL, IMapper mapper)
        {
            _TeamBL = teamBL;
            _Mapper = mapper;
        }

        [HttpPost]
        [Route("registerteam")]
        public ActionResult<bool> RegisterTeam([FromBody]RegisterTeamViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}