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
using QRCoder;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using RaceVenturaAPI.ViewModels.AppApi;

namespace RaceVenturaAPI.Controllers.Races
{
    [Authorize(Policy = "RaceUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class RacesController : RacesControllerBase, ICrudController<RaceViewModel, RaceDetailViewModel>
    {
        private readonly IGenericCrudBL<Race> _RaceBL;
        private readonly IHtmlToPdfBL _HtmlToPdfBL;
        private readonly IRazorToHtml _RazorToHtml;
        private readonly IWebHostEnvironment _WebHostingEnvironment;
        private readonly IMapper _Mapper;
        private readonly ILogger _Logger;

        public RacesController(IGenericCrudBL<Race> raceBL, IHtmlToPdfBL htmlToPdfBL, IRazorToHtml razorToHtml, IWebHostEnvironment webHostingEnvironment, IMapper mapper, ILogger<RacesController> logger)
        {
            _RaceBL = raceBL ?? throw new ArgumentNullException(nameof(raceBL));
            _HtmlToPdfBL = htmlToPdfBL ?? throw new ArgumentNullException(nameof(htmlToPdfBL));
            _RazorToHtml = razorToHtml ?? throw new ArgumentNullException(nameof(razorToHtml));
            _WebHostingEnvironment = webHostingEnvironment ?? throw new ArgumentNullException(nameof(webHostingEnvironment));
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

        [HttpGet]
        [Route("getpointspdf")]
        public async Task<IActionResult> GetPointsPdf(Guid raceId)
        {
            try
            {
                var race = _RaceBL.GetById(GetUserId(), raceId);
                var raceViewModel = _Mapper.Map<RaceDetailViewModel>(race);
                CreateQrCodes(raceViewModel);
                AddAvatar(raceViewModel);
                var html = await _RazorToHtml.RenderRazorViewAsync("PointsPdf", raceViewModel);
                var bytes = await _HtmlToPdfBL.ConvertHtmlToPdf(html, "views/css/pdf.css");
                return File(bytes, "application/pdf", "RacePoints.pdf");
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

        private void AddAvatar(RaceDetailViewModel raceViewModel)
        {
            var avatar = new Bitmap($@"{_WebHostingEnvironment.ContentRootPath}\Content\Images\DefaultAvatar.png");
            using MemoryStream stream = new MemoryStream();
            avatar.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            raceViewModel.Avatar = stream.ToArray();
        }

        private static void CreateQrCodes(RaceDetailViewModel raceViewModel)
        {
            foreach (var stage in raceViewModel.Stages)
            {
                foreach (var point in stage.Points)
                {
                    var txtQRCode = $"QrCodeType: {QrCodeTypes.RegisterPoint}, RaceId:{raceViewModel.RaceId}, TeamId:{point.PointId}";

                    QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(txtQRCode, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    using MemoryStream stream = new MemoryStream();
                    qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    point.QrCodeArray = stream.ToArray();
                }
            }
        }
    }
}
