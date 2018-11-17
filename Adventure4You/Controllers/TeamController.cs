using Adventure4You.DatabaseContext;
using Adventure4You.ViewModels.Teams;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Adventure4You.Controllers
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