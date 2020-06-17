using Adventure4You;
using Adventure4You.Models;
using Adventure4You.Races;
using Adventure4YouData;
using Adventure4YouData.Models;
using Adventure4YouData.Models.Races;
using Adventure4YouData.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Adventure4YouTest.Races
{
    [TestClass]
    public class RaceBaseBLTests
    {
        private readonly Mock<ILogger> _LoggerMock = new Mock<ILogger>();
        private readonly Mock<IAdventure4YouUnitOfWork> _UnitOfWorkMock = new Mock<IAdventure4YouUnitOfWork>();
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

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.CheckUserIsAuthorizedForRace(userId, raceId));

            Assert.AreEqual($"User not authorized for race.", exception.Message);
            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);

            _LoggerMock.Verify(
            m => m.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().Equals($"Error in TestRaceBaseBL: User with ID '{userId}' tried to access race with ID '{raceId}' but is unauthorized.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }

        [TestMethod]
        public void CheckUserIsAuthorizedForRaceNoException()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            SetupRepositoryMock(userId, raceId, out UserLink userLink, out Mock<IGenericRepository<UserLink>> userLinkRepositoryMock);

            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            _Sut.CheckUserIsAuthorizedForRace(userId, raceId);

            _LoggerMock.Verify(
            m => m.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<FormattedLogValues>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Never);
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

            _LoggerMock.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().Equals($"Error in TestRaceBaseBL: User with ID '{userId}' tried to access race with ID '{raceId}' but it does not exsist.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
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

            _LoggerMock.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<FormattedLogValues>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Never);
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
            public TestRaceBaseBL(IAdventure4YouUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
            {

            }

            public new UserLink GetRaceUserLink(Guid userId, Guid raceId)
            {
                return base.GetRaceUserLink(userId, raceId);
            }

            public new void CheckUserIsAuthorizedForRace(Guid userId, Guid raceId)
            {
                base.CheckUserIsAuthorizedForRace(userId, raceId);
            }

            public new void CheckIfRaceExsists(Guid userId, Guid raceId)
            {
                base.CheckIfRaceExsists(userId, raceId);
            }
        }
    }
}
