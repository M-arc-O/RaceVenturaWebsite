using RaceVenturaAPI.Controllers.Races;
using RaceVenturaAPI.ViewModels.Races;
using RaceVenturaData.Models.Races;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RaceVentura.PdfGeneration;
using Moq;
using Microsoft.AspNetCore.Hosting;

namespace RaceVenturaAPITest.Controllers.Races
{
    [TestClass]
    public class RacesControllerTests : ApiControllerCrudTestsBase<RacesController, Race, RaceViewModel, RaceDetailViewModel>
    {
        private readonly Mock<IHtmlToPdfBL> _HtmlToPdfMock = new Mock<IHtmlToPdfBL>();
        private readonly Mock<IRazorToHtml> _RazorToHtmlMock = new Mock<IRazorToHtml>();
        private readonly Mock<IWebHostEnvironment> _WebHostEnviroment = new Mock<IWebHostEnvironment>();

        [TestInitialize]
        public void InitializeTest()
        {
            Sut = new RacesController(_BLMock.Object, _HtmlToPdfMock.Object, _RazorToHtmlMock.Object, _WebHostEnviroment.Object, _MapperMock.Object, _LoggerMock.Object);

            SetControllerContext(Sut);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new RacesController(null, _HtmlToPdfMock.Object, _RazorToHtmlMock.Object, _WebHostEnviroment.Object, _MapperMock.Object, _LoggerMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new RacesController(_BLMock.Object, null, _RazorToHtmlMock.Object, _WebHostEnviroment.Object, _MapperMock.Object, _LoggerMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new RacesController(_BLMock.Object, _HtmlToPdfMock.Object, null, _WebHostEnviroment.Object, _MapperMock.Object, _LoggerMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new RacesController(_BLMock.Object, _HtmlToPdfMock.Object, _RazorToHtmlMock.Object, null, _MapperMock.Object, _LoggerMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new RacesController(_BLMock.Object, _HtmlToPdfMock.Object, _RazorToHtmlMock.Object, _WebHostEnviroment.Object, null, _LoggerMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new RacesController(_BLMock.Object, _HtmlToPdfMock.Object, _RazorToHtmlMock.Object, _WebHostEnviroment.Object, _MapperMock.Object, null));
        }

        [TestMethod]
        public void GetRaceNoErrorsTest()
        {
            GetNoErrorsTest();
        }

        [TestMethod]
        public void GetRaceBusinesErrorTest()
        {
            GetBusinessErrorTest(SetupBlGetBusinessException);
        }

        [TestMethod]
        public void GetRaceExceptionTest()
        {
            GetExceptionTest(SetupBlGetException);
        }

        [TestMethod]
        public void GetByIdRaceNoErrorsTest()
        {
            GetByIdNoErrorsTest();
        }

        [TestMethod]
        public void GetByIdRaceBusinesErrorTest()
        {
            GetByIdBusinessErrorTest(SetupBlGetByIdBusinessException);
        }

        [TestMethod]
        public void GetByIdRaceExceptionTest()
        {
            GetByIdExceptionTest(SetupBlGetByIdException);
        }

        [TestMethod]
        public void AddRaceNoErrorsTest()
        {
            AddNoErrorsTest(Sut);
        }

        [TestMethod]
        public void AddRaceInvalidModelStateTest()
        {
            AddInvalidModelStateTest(Sut);
        }

        [TestMethod]
        public void AddRaceBusinesErrorTest()
        {
            AddBusinessErrorTest(Sut, SetupBlAddBusinessException);
        }

        [TestMethod]
        public void AddRaceExceptionTest()
        {
            AddExceptionTest(Sut, SetupBlAddException);
        }

        [TestMethod]
        public void EditRaceNoErrorsTest()
        {
            EditNoErrorsTest(Sut);
        }

        [TestMethod]
        public void EditRaceInvalidModelStateTest()
        {
            EditInvalidModelStateTest(Sut);
        }

        [TestMethod]
        public void EditRaceBusinesErrorTest()
        {
            EditBusinessErrorTest(Sut, SetupBlEditBusinessException);
        }

        [TestMethod]
        public void EditRaceExceptionTest()
        {
            EditExceptionTest(Sut, SetupBlEditException);
        }

        [TestMethod]
        public void DeleteRaceNoErrorsTest()
        {
            DeleteNoErrorsTest(Sut);
        }

        [TestMethod]
        public void DeleteRaceBusinesErrorTest()
        {
            DeleteBusinessErrorTest(Sut, SetupBlDeleteBusinessException);
        }

        [TestMethod]
        public void DeleteRaceExceptionTest()
        {
            DeleteExceptionTest(Sut, SetupBlDeleteException);
        }
    }
}
