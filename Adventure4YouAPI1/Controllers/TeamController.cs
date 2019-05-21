using Adventure4YouAPI.DatabaseContext;
using Adventure4YouAPI.ViewModels.Teams;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Adventure4YouAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly Adventure4YouDbContext _Context;

        public TeamController(Adventure4YouDbContext context)
        {
            _Context = context;
        }

        [HttpPost]
        [Route("registerteam")]
        public ActionResult<bool> RegisterTeam([FromBody]RegisterTeamViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}