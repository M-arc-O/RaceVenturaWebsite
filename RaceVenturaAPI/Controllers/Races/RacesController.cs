using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RaceVenturaAPI.ViewModels.Races;
using RaceVenturaAPI.ViewModels;
using RaceVentura.Races;
using AutoMapper;
using RaceVentura;
using RaceVenturaData.Models.Races;
using Microsoft.Extensions.Logging;

namespace RaceVenturaAPI.Controllers.Races
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class RacesController : RacesControllerBase, ICrudController<RaceViewModel, RaceDetailViewModel>
    {
        private readonly IGenericCrudBL<Race> _RaceBL;
        private readonly IMapper _Mapper;
        private readonly ILogger _Logger;

        public RacesController(IGenericCrudBL<Race> raceBL, IMapper mapper, ILogger<RacesController> logger)
        {
            _RaceBL = raceBL ?? throw new ArgumentNullException(nameof(raceBL));
            _Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("getallraces")]
        public IActionResult Get()
        {
            try
            {
                var races = _RaceBL.Get(GetUserId());

                var retVal = new List<RaceViewModel>();

                foreach (var race in races)
                {
                    retVal.Add(_Mapper.Map<RaceViewModel>(race));
                }

                return Ok(retVal);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpGet()]
        [Route("getracedetails")]
        public IActionResult GetById(Guid raceId)
        {
            try
            {
                var raceModel = _RaceBL.GetById(GetUserId(), raceId);
                return Ok(_Mapper.Map<RaceDetailViewModel>(raceModel));
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("addrace")]
        public IActionResult Add(RaceDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var raceModel = _Mapper.Map<Race>(viewModel);
                _RaceBL.Add(GetUserId(), raceModel);

                return Ok(_Mapper.Map<RaceDetailViewModel>(raceModel));
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Route("editrace")]
        public IActionResult Edit(RaceDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var raceModel = _Mapper.Map<Race>(viewModel);
                _RaceBL.Edit(GetUserId(), raceModel);

                return Ok(_Mapper.Map<RaceDetailViewModel>(raceModel));
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{raceId}/remove")]
        public IActionResult Delete(Guid raceId)
        {
            try
            {
                _RaceBL.Delete(GetUserId(), raceId);

                return Ok(raceId);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}
