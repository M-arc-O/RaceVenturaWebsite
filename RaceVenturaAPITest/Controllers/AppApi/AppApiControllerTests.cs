using RaceVentura;
using RaceVenturaTestUtils;
using RaceVentura.AppApi;
using RaceVentura.Models;
using RaceVenturaAPI.Controllers.AppApi;
using RaceVenturaAPI.ViewModels.AppApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using RaceVenturaData.Models.Races;

namespace RaceVenturaAPITest.Controllers.Races
{
    [TestClass]
    public class AppApiControllerTests
    {
        protected readonly Mock<IAppApiBL> _BLMock = new Mock<IAppApiBL>();
        protected readonly Mock<ILogger<AppApiController>> _LoggerMock = new Mock<ILogger<AppApiController>>();
        protected AppApiController _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new AppApiController(_BLMock.Object, _LoggerMock.Object);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new AppApiController(null, _LoggerMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new AppApiController(_BLMock.Object, null));
        }

        [TestMethod]
        public void RegisterToRaceNoError()
        {
            var viewModel = new RegisterToRaceViewModel();

            var result = _Sut.RegisterToRace(viewModel);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(viewModel, (result as OkObjectResult).Value);
        }

        [TestMethod]
        public void RegisterToRaceBusinessError()
        {
            var viewModel = new RegisterToRaceViewModel();
            var exception = new BusinessException("", BLErrorCodes.Duplicate);

            _BLMock.Setup(bl => bl.RegisterToRace(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(exception);

            var result = _Sut.RegisterToRace(viewModel);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual((int)BLErrorCodes.Duplicate, (int)(result as BadRequestObjectResult).Value);
        }

        [TestMethod]
        public void RegisterToRaceException()
        {
            var viewModel = new RegisterToRaceViewModel();
            var exceptionMessage = "a";
            var exception = new Exception(exceptionMessage);

            _BLMock.Setup(bl => bl.RegisterToRace(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(exception);

            var result = _Sut.RegisterToRace(viewModel);

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(500, (result as StatusCodeResult).StatusCode);

            _LoggerMock.VerifyLog(LogLevel.Error, Times.Once, $"Error in {typeof(AppApiController)}: {exceptionMessage}");
        }

        [TestMethod]
        public void RegisterPointNoError()
        {
            var viewModel = new RegisterPointViewModel();
            var question = "a";

            _BLMock.Setup(bl => bl.RegisterPoint(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.IsAny<string>())).Returns(new Point { Message = question });

            var result = _Sut.RegisterPoint(viewModel);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(viewModel, (result as OkObjectResult).Value);
            Assert.AreEqual(question, ((RegisterPointViewModel)(result as OkObjectResult).Value).Message);
        }

        [TestMethod]
        public void RegisterPointBusinessError()
        {
            var viewModel = new RegisterPointViewModel();
            var exception = new BusinessException("", BLErrorCodes.Duplicate);

            _BLMock.Setup(bl => bl.RegisterPoint(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.IsAny<string>())).Throws(exception);

            var result = _Sut.RegisterPoint(viewModel);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual((int)BLErrorCodes.Duplicate, (int)(result as BadRequestObjectResult).Value);
        }

        [TestMethod]
        public void RegisterPointException()
        {
            var viewModel = new RegisterPointViewModel();
            var exceptionMessage = "a";
            var exception = new Exception(exceptionMessage);

            _BLMock.Setup(bl => bl.RegisterPoint(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.IsAny<string>())).Throws(exception);

            var result = _Sut.RegisterPoint(viewModel);

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(500, (result as StatusCodeResult).StatusCode);

            _LoggerMock.VerifyLog(LogLevel.Error, Times.Once, $"Error in {typeof(AppApiController)}: {exceptionMessage}");
        }

        [TestMethod]
        public void RegisterStageStartNoError()
        {
            var viewModel = new RegisterStageStartViewModel();

            var result = _Sut.RegisterStageStart(viewModel);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(viewModel, (result as OkObjectResult).Value);
        }

        [TestMethod]
        public void RegisterStageStartBusinessError()
        {
            var viewModel = new RegisterStageStartViewModel();
            var exception = new BusinessException("", BLErrorCodes.Duplicate);

            _BLMock.Setup(bl => bl.RegisterStageStart(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(exception);

            var result = _Sut.RegisterStageStart(viewModel);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual((int)BLErrorCodes.Duplicate, (int)(result as BadRequestObjectResult).Value);
        }

        [TestMethod]
        public void RegisterStageStartException()
        {
            var viewModel = new RegisterStageStartViewModel();
            var exceptionMessage = "a";
            var exception = new Exception(exceptionMessage);

            _BLMock.Setup(bl => bl.RegisterStageStart(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(exception);

            var result = _Sut.RegisterStageStart(viewModel);

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(500, (result as StatusCodeResult).StatusCode);

            _LoggerMock.VerifyLog(LogLevel.Error, Times.Once, $"Error in {typeof(AppApiController)}: {exceptionMessage}");
        }

        [TestMethod]
        public void RegisterRaceEndNoError()
        {
            var viewModel = new RegisterRaceEndViewModel();

            var result = _Sut.RegisterRaceEnd(viewModel);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(viewModel, (result as OkObjectResult).Value);
        }

        [TestMethod]
        public void RegisterRaceEndBusinessError()
        {
            var viewModel = new RegisterRaceEndViewModel();
            var exception = new BusinessException("", BLErrorCodes.Duplicate);

            _BLMock.Setup(bl => bl.RegisterRaceEnd(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(exception);

            var result = _Sut.RegisterRaceEnd(viewModel);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual((int)BLErrorCodes.Duplicate, (int)(result as BadRequestObjectResult).Value);
        }

        [TestMethod]
        public void RegisterRaceEndException()
        {
            var viewModel = new RegisterRaceEndViewModel();
            var exceptionMessage = "a";
            var exception = new Exception(exceptionMessage);

            _BLMock.Setup(bl => bl.RegisterRaceEnd(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(exception);

            var result = _Sut.RegisterRaceEnd(viewModel);

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(500, (result as StatusCodeResult).StatusCode);

            _LoggerMock.VerifyLog(LogLevel.Error, Times.Once, $"Error in {typeof(AppApiController)}: {exceptionMessage}");
        }
    }
}
