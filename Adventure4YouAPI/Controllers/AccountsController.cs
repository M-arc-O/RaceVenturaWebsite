using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using Adventure4YouAPI.ViewModels.Identity;
using Adventure4YouAPI.Helpers;
using Adventure4You;
using Adventure4You.Models.Identity;

namespace Adventure4YouAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountBL _AccountBL;
        private readonly IMapper _Mapper;

        public AccountsController(IAccountBL accountBL, IMapper mapper)
        {
            _Mapper = mapper;
            _AccountBL = accountBL;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userIdentity = _Mapper.Map<AppUser>(viewModel);

                var result = await _AccountBL.CreateUser(userIdentity, viewModel.Password);

                if (!result.Succeeded)
                {
                    return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
                }

                return new OkResult();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}