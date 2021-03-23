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
using System.Threading.Tasks;
using RaceVentura.PdfGeneration;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using RaceVenturaAPI.ViewModels.AppApi;
using RaceVenturaAPI.Extensions;
using RaceVenturaAPI.Helpers;

namespace RaceVenturaAPI.Controllers.Races
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class RacesController : RacesControllerBase, ICrudController<RaceViewModel, RaceDetailViewModel>
    {
        private readonly IGenericCrudBL<Race> _raceBL;
        private readonly IRaceAccessBL _raceAccessBL;
        private readonly IHtmlToPdfBL _htmlToPdfBL;
        private readonly IRazorToHtml _razorToHtml;
        private readonly IWebHostEnvironment _webHostingEnvironment;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RacesController(IGenericCrudBL<Race> raceBL, IRaceAccessBL raceAccessBL, IHtmlToPdfBL htmlToPdfBL, IRazorToHtml razorToHtml, IWebHostEnvironment webHostingEnvironment, IMapper mapper, ILogger<RacesController> logger)
        {
            _raceBL = raceBL ?? throw new ArgumentNullException(nameof(raceBL));
            _raceAccessBL = raceAccessBL ?? throw new ArgumentNullException(nameof(raceAccessBL));
            _htmlToPdfBL = htmlToPdfBL ?? throw new ArgumentNullException(nameof(htmlToPdfBL));
            _razorToHtml = razorToHtml ?? throw new ArgumentNullException(nameof(razorToHtml));
            _webHostingEnvironment = webHostingEnvironment ?? throw new ArgumentNullException(nameof(webHostingEnvironment));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("getallraces")]
        public IActionResult Get()
        {
            try
            {
                var races = _raceBL.Get(GetUserId());

                var retVal = new List<RaceViewModel>();

                foreach (var race in races)
                {
                    retVal.Add(_mapper.Map<RaceViewModel>(race));
                }

                return Ok(retVal);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpGet()]
        [Route("getracedetails")]
        public IActionResult GetById(Guid raceId)
        {
            try
            {
                var userId = GetUserId();
                var raceModel = _raceBL.GetById(userId, raceId);
                var viewModel = _mapper.Map<RaceDetailViewModel>(raceModel);
                viewModel.AccessLevel = (AccessLevelViewModel)_raceAccessBL.GetAccessLevel(userId, raceId);
                CreateQrCodesForTeams(viewModel);
                return Ok(viewModel);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("addrace")]
        public IActionResult Add(RaceDetailViewModel viewModel)
        {
            ValidateViewModel(viewModel);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var raceModel = _mapper.Map<Race>(viewModel);
                _raceBL.Add(GetUserId(), raceModel);

                return Ok(_mapper.Map<RaceDetailViewModel>(raceModel));
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        private void ValidateViewModel(RaceDetailViewModel viewModel)
        {
            if (viewModel.RaceType != RaceTypeViewModel.NoTimeLimit)
            {
                switch (viewModel.RaceType)
                {
                    case RaceTypeViewModel.Classic:
                        if (!viewModel.StartTime.HasValue)
                        {
                            ModelState.AddModelError("StartTime", "'StartTime' cannot be null when race type is classic.");
                        }
                        CheckMaxDurationAndPenalty(viewModel);
                        break;
                    case RaceTypeViewModel.TimeLimit:
                        CheckMaxDurationAndPenalty(viewModel);
                        break;
                    default:
                        throw new Exception($"Unknown race type {viewModel.RaceType}");
                }
            }

            if (viewModel.CoordinatesCheckEnabled && !viewModel.AllowedCoordinatesDeviation.HasValue)
            {
                ModelState.AddModelError("AllowedCoordinatesDeviation", "'AllowedCoordinatesDeviation' cannot be null when coordinate check is true.");
            }
        }

        private void CheckMaxDurationAndPenalty(RaceDetailViewModel viewModel)
        {
            if (!viewModel.PenaltyPerMinuteLate.HasValue)
            {
                ModelState.AddModelError("PenaltyPerMinuteLate", "'PenaltyPerMinuteLate' cannot be null when race type is classic or time limit.");
            }
            if (!viewModel.MaxDuration.HasValue)
            {
                ModelState.AddModelError("MaxDuration", "'MaxDuration' cannot be null when race type is classic or time limit.");
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
                var raceModel = _mapper.Map<Race>(viewModel);
                _raceBL.Edit(GetUserId(), raceModel);

                return Ok(_mapper.Map<RaceDetailViewModel>(raceModel));
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{raceId}/remove")]
        public IActionResult Delete(Guid raceId)
        {
            try
            {
                _raceBL.Delete(GetUserId(), raceId);

                return Ok(raceId);
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("getpointspdf")]
        public async Task<IActionResult> GetPointsPdf(Guid raceId)
        {
            try
            {
                var race = _raceBL.GetById(GetUserId(), raceId);
                var raceViewModel = _mapper.Map<RaceDetailViewModel>(race);
                CreateQrCodesForPoints(raceViewModel);
                AddAvatar(raceViewModel);
                var html = await _razorToHtml.RenderRazorViewAsync("PointsPdf", raceViewModel);
                var bytes = await _htmlToPdfBL.ConvertHtmlToPdf(html, "views/css/bootstrap.css");
                return File(bytes, "application/pdf", "Points.pdf");
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("getteamspdf")]
        public async Task<IActionResult> GetTeamsPdf(Guid raceId)
        {
            try
            {
                var race = _raceBL.GetById(GetUserId(), raceId);
                var raceViewModel = _mapper.Map<RaceDetailViewModel>(race);
                CreateQrCodesForTeams(raceViewModel);
                var html = await _razorToHtml.RenderRazorViewAsync("TeamsPdf", raceViewModel);
                var bytes = await _htmlToPdfBL.ConvertHtmlToPdf(html, "views/css/bootstrap.css");
                return File(bytes, "application/pdf", "Teams.pdf");
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("getstagesandraceendpdf")]
        public async Task<IActionResult> GetStagesAndRaceEndPdf(Guid raceId)
        {
            try
            {
                var race = _raceBL.GetById(GetUserId(), raceId);
                var raceViewModel = _mapper.Map<RaceDetailViewModel>(race);
                CreateQrCodesForStagesAndRaceEnd(raceViewModel);
                var html = await _razorToHtml.RenderRazorViewAsync("StagesAndRaceEndPdf", raceViewModel);
                var bytes = await _htmlToPdfBL.ConvertHtmlToPdf(html, "views/css/bootstrap.css");
                return File(bytes, "application/pdf", "StagesAndRaceEnd.pdf");
            }
            catch (BusinessException ex)
            {
                return BadRequest((ErrorCodes)ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {typeof(RacesController)}: {ex.Message}");
                return StatusCode(500);
            }
        }

        private static void CreateQrCodesForStagesAndRaceEnd(RaceDetailViewModel raceViewModel)
        {
            var txtQRCode = $"QrCodeType: {QrCodeTypes.RegisterRaceEnd}, RaceId:{raceViewModel.RaceId}";
            var stream = RaceQrCodeHelper.CreateQrCodes(txtQRCode);
            raceViewModel.QrCodeArray = stream.ToArray();

            foreach (var stage in raceViewModel.Stages)
            {
                txtQRCode = $"QrCodeType: {QrCodeTypes.RegisterStageEnd}, RaceId:{raceViewModel.RaceId}, StageId:{stage.StageId}";
                stream = RaceQrCodeHelper.CreateQrCodes(txtQRCode);
                stage.QrCodeArray = stream.ToArray();
            }
        }

        private static void CreateQrCodesForTeams(RaceDetailViewModel raceViewModel)
        {
            foreach (var team in raceViewModel.Teams)
            {
                team.AddQrCode(raceViewModel.RaceId);
            }
        }

        private void AddAvatar(RaceDetailViewModel raceViewModel, string avatarFileName = "DefaultAvatar.png")
        {
            var avatar = new Bitmap($@"{_webHostingEnvironment.ContentRootPath}\Content\Images\{avatarFileName}");
            using MemoryStream stream = new MemoryStream();
            avatar.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            raceViewModel.Avatar = stream.ToArray();
        }

        private static void CreateQrCodesForPoints(RaceDetailViewModel raceViewModel)
        {
            foreach (var stage in raceViewModel.Stages)
            {
                foreach (var point in stage.Points)
                {
                    var txtQRCode = $"QrCodeType: {QrCodeTypes.RegisterPoint}, RaceId:{raceViewModel.RaceId}, PointId:{point.PointId}";
                    var stream = RaceQrCodeHelper.CreateQrCodes(txtQRCode);
                    point.QrCodeArray = stream.ToArray();
                }
            }
        }
    }
}
