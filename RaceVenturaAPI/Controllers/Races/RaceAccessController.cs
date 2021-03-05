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

namespace RaceVenturaAPI.Controllers.Races
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class RaceAccessController : RacesControllerBase
    {
        private readonly IGenericCrudBL<UserLink> _raceAccessBL;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RaceAccessController(IGenericCrudBL<UserLink> raceAccessBL, IMapper mapper, ILogger<RacesController> logger)
        {
            _raceAccessBL = raceAccessBL ?? throw new ArgumentNullException(nameof(raceAccessBL));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("getaccesses")]
        public IActionResult Get(Guid raceId)
        {
            try
            {
                var accesses = _raceAccessBL.Get(raceId).ToList();
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
        public IActionResult AddAccess(RaceAccessViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var model = _mapper.Map<UserLink>(viewModel);
                _raceAccessBL.Add(GetUserId(), model);

                return Ok(_mapper.Map<RaceAccessViewModel>(model));
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
        public IActionResult EditAccess(RaceAccessViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var model = _mapper.Map<UserLink>(viewModel);
                _raceAccessBL.Edit(GetUserId(), model);

                return Ok(_mapper.Map<RaceAccessViewModel>(model));
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
        [Route("{raceId}/remove")]
        public IActionResult Delete(Guid accessId)
        {
            try
            {
                _raceAccessBL.Delete(GetUserId(), accessId);

                return Ok(accessId);
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
