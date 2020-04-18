using Adventure4You;
using Adventure4You.Models;
using Adventure4You.Races;
using Adventure4YouAPI.Controllers;
using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouData.Models.Races;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace Adventure4YouAPITest
{
    [TestClass]
    public class TeamsControllerTests
    {
        private readonly Mock<IGenericBL<Team>> _TeamBLMock = new Mock<IGenericBL<Team>>();
        private readonly Mock<IGenericBL<VisitedPoint>> _VisitedPointBLMock = new Mock<IGenericBL<VisitedPoint>>();
        private readonly Mock<IMapper> _MapperMock = new Mock<IMapper>();
        private readonly Mock<ILogger> _LoggerMock = new Mock<ILogger>();
        private TeamsController _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new TeamsController(_TeamBLMock.Object, _VisitedPointBLMock.Object, _MapperMock.Object, _LoggerMock.Object);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new GenericIdentity("Username", "Token"), new[]
            {
                new Claim(Adventure4YouAPI.Helpers.Constants.Strings.JwtClaimIdentifiers.Id, Guid.NewGuid().ToString()),
                new Claim(Adventure4YouAPI.Helpers.Constants.Strings.JwtClaimIdentifiers.Rol, Adventure4YouAPI.Helpers.Constants.Strings.JwtClaims.ApiAccess)
            })));

            _Sut.ControllerContext.HttpContext = contextMock.Object;

            Assert.IsNotNull(_Sut.HttpContext.User);
        }

        [TestMethod]
        public void AddTeamNoErrorsTest()
        {
            var viewModel = new TeamViewModel();

            var result = _Sut.AddTeam(viewModel);

            Assert.IsInstanceOfType(result, typeof(ActionResult<TeamViewModel>));
        }

        [TestMethod]
        public void AddTeamBusinessErrorTest()
        {
            var viewModel = new TeamViewModel();

            _TeamBLMock.Setup(bl => bl.Add(It.IsAny<Guid>(), It.IsAny<Team>())).Throws(new BusinessException("", BLErrorCodes.Duplicate));

            var result = _Sut.AddTeam(viewModel);

            Assert.IsInstanceOfType(result, typeof(ActionResult<TeamViewModel>));
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual((int)BLErrorCodes.Duplicate, (int)(result.Result as BadRequestObjectResult).Value);
        }

        [TestMethod]
        public void AddTeamExceptionTest()
        {
            var viewModel = new TeamViewModel();
            var exceptionMessage = "a";
            var exception = new Exception(exceptionMessage);

            _TeamBLMock.Setup(bl => bl.Add(It.IsAny<Guid>(), It.IsAny<Team>())).Throws(exception);

            var result = _Sut.AddTeam(viewModel);

            Assert.IsInstanceOfType(result, typeof(ActionResult<TeamViewModel>));
            Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));
            Assert.AreEqual(500, (result.Result as StatusCodeResult).StatusCode);

            _LoggerMock.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().Equals($"Error in {typeof(TeamsController)}: {exceptionMessage}")),
                It.Is<Exception>(e => e.Equals(exception)),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }
    }
}
