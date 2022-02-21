using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RaceVentura;
using RaceVentura.Races;
using RaceVenturaAPI.ViewModels;
using RaceVenturaAPI.ViewModels.Races;
using RaceVenturaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaceVenturaAPI.Controllers.Races
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class RaceAccessController : RacesControllerBase
    {
        private readonly IRaceAccessBL _raceAccessBL;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RaceAccessController(IRaceAccessBL raceAccessBL, IMapper mapper, ILogger<RaceAccessController> logger)
        {
            _raceAccessBL = raceAccessBL ?? throw new ArgumentNullException(nameof(raceAccessBL));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("getaccesses")]
        public async Task<IActionResult> Get(Guid raceId)
        {
            try
            {
                var accesses = (await _raceAccessBL.Get(GetUserId(), raceId)).ToList();
                var retVal = new List<RaceAccessViewModel>(accesses.Count);

                foreach (var access in accesses)
                {
                    retVal.Add(_mapper.Map<RaceAccessViewModel>(access));
                }

                return Ok(retVal);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(RaceAccessController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("addaccess")]
        public async Task<IActionResult> AddAccess(RaceAccessViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _raceAccessBL.Add(GetUserId(), viewModel.RaceId, viewModel.UserEmail, (RaceAccessLevel)viewModel.AccessLevel);
                return Ok(viewModel);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(RaceAccessController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Route("editaccess")]
        public async Task<IActionResult> EditAccess(RaceAccessViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _raceAccessBL.Edit(GetUserId(), viewModel.RaceId, viewModel.UserEmail, (RaceAccessLevel)viewModel.AccessLevel);
                return Ok(viewModel);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(RaceAccessController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{raceId}/{emailAddress}/remove")]
        public async Task<IActionResult> Delete(Guid raceId, string emailAddress)
        {
            try
            {
                await _raceAccessBL.Delete(GetUserId(), raceId, emailAddress);
                return Ok(new RaceAccessViewModel { UserEmail = emailAddress, RaceId = raceId });
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(RaceAccessController)}: {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}
