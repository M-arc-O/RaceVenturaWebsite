using RaceVentura;
using RaceVenturaTestUtils;
using RaceVentura.Models;
using RaceVentura.Races;
using RaceVenturaData;
using RaceVenturaData.Models;
using RaceVenturaData.Models.Races;
using RaceVenturaData.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RaceVenturaTest.Races
{
    [TestClass]
    public class RaceBaseBLTests
    {
        private readonly Mock<ILogger> _LoggerMock = new Mock<ILogger>();
        private readonly Mock<IRaceVenturaUnitOfWork> _UnitOfWorkMock = new Mock<IRaceVenturaUnitOfWork>();
        private TestRaceBaseBL _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new TestRaceBaseBL(_UnitOfWorkMock.Object, _LoggerMock.Object);
        }

        [TestMethod]
        public void GetRaceUserLinkWithResult()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            SetupRepositoryMock(userId, raceId, out UserLink userLink, out Mock<IGenericRepository<UserLink>> userLinkRepositoryMock);

            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            var result = _Sut.GetRaceUserLink(userId, raceId);

            Assert.IsInstanceOfType(result, typeof(UserLink));
            Assert.AreEqual(userLink, result);
        }

        [TestMethod]
        public void GetRaceUserLinkWithoutResult()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            var result = _Sut.GetRaceUserLink(userId, raceId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void CheckUserIsAuthorizedForRaceThrowsException()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.CheckUserIsAuthorizedForRace(userId, raceId, RaceAccessLevel.Owner));

            Assert.AreEqual($"User is not authorized for race.", exception.Message);
            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);

            _LoggerMock.VerifyLog(LogLevel.Warning, Times.Once, $"Error in TestRaceBaseBL: User with ID '{userId}' tried to access race with ID '{raceId}' but is unauthorized.");
        }

        [TestMethod]
        public void CheckUserIsAuthorizedForRaceNoException()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            SetupRepositoryMock(userId, raceId, out UserLink userLink, out Mock<IGenericRepository<UserLink>> userLinkRepositoryMock);

            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            _Sut.CheckUserIsAuthorizedForRace(userId, raceId, RaceAccessLevel.Owner);

            _LoggerMock.VerifyLog(LogLevel.Warning, Times.Never);
        }

        [TestMethod]
        public void CheckIfRaceExsistsThrowsException()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.CheckIfRaceExsists(userId, raceId));

            Assert.AreEqual($"Race with ID '{raceId}' does not exsist.", exception.Message);
            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);

            _LoggerMock.VerifyLog(LogLevel.Error, Times.Once, $"Error in TestRaceBaseBL: User with ID '{userId}' tried to access race with ID '{raceId}' but it does not exsist.");
        }

        [TestMethod]
        public void CheckIfRaceExsistsNoException()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();

            raceRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(raceId)))).Returns(new Race());

            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            _Sut.CheckIfRaceExsists(userId, raceId);

            _LoggerMock.VerifyLog(LogLevel.Error, Times.Never);
        }

        private static void SetupRepositoryMock(Guid userId, Guid raceId, out UserLink userLink, out Mock<IGenericRepository<UserLink>> userLinkRepositoryMock)
        {
            userLink = new UserLink
            {
                UserId = userId,
                RaceId = raceId
            };
            userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            userLinkRepositoryMock.Setup(r => r.Get(
                It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>())).Returns(new List<UserLink> { userLink });
        }

        private class TestRaceBaseBL : RaceBaseBL
        {
            public TestRaceBaseBL(IRaceVenturaUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
            {

            }

            public new UserLink GetRaceUserLink(Guid userId, Guid raceId)
            {
                return base.GetRaceUserLink(userId, raceId);
            }

            public new void CheckUserIsAuthorizedForRace(Guid userId, Guid raceId, RaceAccessLevel minimumAccessLevel)
            {
                base.CheckUserIsAuthorizedForRace(userId, raceId, minimumAccessLevel);
            }

            public new void CheckIfRaceExsists(Guid userId, Guid raceId)
            {
                base.CheckIfRaceExsists(userId, raceId);
            }
        }
    }
}
