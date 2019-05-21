using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using AutoMapper;

using Adventure4YouAPI.DatabaseContext;
using Adventure4YouAPI.Models.Identity;
using Adventure4YouAPI.ViewModels.Identity;
using Adventure4YouAPI.Helpers;
using System;

namespace Adventure4YouAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly Adventure4YouDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager, IMapper mapper, Adventure4YouDbContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<AppUser>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}