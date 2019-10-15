using Adventure4You.TeamRace;
using Adventure4YouAPI.ViewModels;
using Adventure4YouAPI.ViewModels.TeamRace;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Adventure4YouAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamRaceController : ControllerBase
    {
        private readonly ITeamRaceBL _TeamRaceBL;
        private readonly IMapper _Mapper;

        public TeamRaceController(ITeamRaceBL teamRaceBL, IMapper mapper)
        {
            _TeamRaceBL = teamRaceBL;
            _Mapper = mapper;
        }

        [Route("registerpoint")]
        public ActionResult<ErrorCodes> RegisterPoint(RegisterPointViewModel viewModel)
        {

            return Ok();
        }
    }
}