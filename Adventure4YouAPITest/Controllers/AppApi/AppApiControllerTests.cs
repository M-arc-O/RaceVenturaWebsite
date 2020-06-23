using Adventure4You;
using Adventure4You.AppApi;
using Adventure4You.Models;
using Adventure4YouAPI.Controllers.AppApi;
using Adventure4YouAPI.Controllers.Races;
using Adventure4YouAPI.ViewModels.AppApi;
using Adventure4YouAPI.ViewModels.Races.MappingProfiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Adventure4YouAPITest.Controllers.Races
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

            _BLMock.Setup(bl => bl.RegisterToRace(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>())).Throws(exception);

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

            _BLMock.Setup(bl => bl.RegisterToRace(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>())).Throws(exception);

            var result = _Sut.RegisterToRace(viewModel);

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(500, (result as StatusCodeResult).StatusCode);

            _LoggerMock.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().Equals($"Error in {typeof(AppApiController)}: {exceptionMessage}")),
                It.Is<Exception>(e => e.Equals(exception)),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }

        [TestMethod]
        public void RegisterPointNoError()
        {
            var viewModel = new RegisterPointViewModel();
            var question = "a";

            _BLMock.Setup(bl => bl.RegisterPoint(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<Guid>(),
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.IsAny<string>())).Returns(question);

            var result = _Sut.RegisterPoint(viewModel);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(viewModel, (result as OkObjectResult).Value);
            Assert.AreEqual(question, ((RegisterPointViewModel)(result as OkObjectResult).Value).Question);
        }

        [TestMethod]
        public void RegisterPointBusinessError()
        {
            var viewModel = new RegisterPointViewModel();
            var exception = new BusinessException("", BLErrorCodes.Duplicate);

            _BLMock.Setup(bl => bl.RegisterPoint(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<string>(),
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
                It.IsAny<string>(),
                It.IsAny<Guid>(),
                It.IsAny<double>(),
                It.IsAny<double>(),
                It.IsAny<string>())).Throws(exception);

            var result = _Sut.RegisterPoint(viewModel);

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(500, (result as StatusCodeResult).StatusCode);

            _LoggerMock.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().Equals($"Error in {typeof(AppApiController)}: {exceptionMessage}")),
                It.Is<Exception>(e => e.Equals(exception)),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }

        [TestMethod]
        public void RegisterStageEndNoError()
        {
            var viewModel = new RegisterStageEndViewModel();

            var result = _Sut.RegisterStageEnd(viewModel);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(viewModel, (result as OkObjectResult).Value);
        }

        [TestMethod]
        public void RegisterStageEndBusinessError()
        {
            var viewModel = new RegisterStageEndViewModel();
            var exception = new BusinessException("", BLErrorCodes.Duplicate);

            _BLMock.Setup(bl => bl.RegisterStageEnd(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>())).Throws(exception);

            var result = _Sut.RegisterStageEnd(viewModel);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual((int)BLErrorCodes.Duplicate, (int)(result as BadRequestObjectResult).Value);
        }

        [TestMethod]
        public void RegisterStageEndException()
        {
            var viewModel = new RegisterStageEndViewModel();
            var exceptionMessage = "a";
            var exception = new Exception(exceptionMessage);

            _BLMock.Setup(bl => bl.RegisterStageEnd(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>())).Throws(exception);

            var result = _Sut.RegisterStageEnd(viewModel);

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(500, (result as StatusCodeResult).StatusCode);

            _LoggerMock.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().Equals($"Error in {typeof(AppApiController)}: {exceptionMessage}")),
                It.Is<Exception>(e => e.Equals(exception)),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }

        [TestMethod]
        public void RegisterRaceEndNoError()
        {
            var viewModel = new RegisterToRaceViewModel();

            var result = _Sut.RegisterRaceEnd(viewModel);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(viewModel, (result as OkObjectResult).Value);
        }

        [TestMethod]
        public void RegisterRaceEndBusinessError()
        {
            var viewModel = new RegisterToRaceViewModel();
            var exception = new BusinessException("", BLErrorCodes.Duplicate);

            _BLMock.Setup(bl => bl.RegisterRaceEnd(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>())).Throws(exception);

            var result = _Sut.RegisterRaceEnd(viewModel);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual((int)BLErrorCodes.Duplicate, (int)(result as BadRequestObjectResult).Value);
        }

        [TestMethod]
        public void RegisterRaceEndException()
        {
            var viewModel = new RegisterToRaceViewModel();
            var exceptionMessage = "a";
            var exception = new Exception(exceptionMessage);

            _BLMock.Setup(bl => bl.RegisterRaceEnd(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>())).Throws(exception);

            var result = _Sut.RegisterRaceEnd(viewModel);

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(500, (result as StatusCodeResult).StatusCode);

            _LoggerMock.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().Equals($"Error in {typeof(AppApiController)}: {exceptionMessage}")),
                It.Is<Exception>(e => e.Equals(exception)),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }
    }
}
