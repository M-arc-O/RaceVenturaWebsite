using RaceVentura;
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
    public class VisitedPointBLTests
    {
        private readonly Mock<ILogger<VisitedPointBL>> _LoggerMock = new Mock<ILogger<VisitedPointBL>>();
        private readonly Mock<IRaceVenturaUnitOfWork> _UnitOfWorkMock = new Mock<IRaceVenturaUnitOfWork>();
        private VisitedPointBL _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new VisitedPointBL(_UnitOfWorkMock.Object, _LoggerMock.Object);
        }

        [TestMethod]
        public void Add()
        {
            var userId = Guid.NewGuid();
            var teamId = Guid.NewGuid();

            var visitedPoint = new VisitedPoint
            {
                TeamId = teamId
            };

            SetupUnitOfWorkToPassAuthorizedTest(teamId);

            var visitedRepositoryMock = new Mock<IGenericRepository<VisitedPoint>>();

            _UnitOfWorkMock.Setup(u => u.VisitedPointRepository).Returns(visitedRepositoryMock.Object);

            _Sut.Add(userId, visitedPoint);

            visitedRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<VisitedPoint, bool>>>(),
                It.IsAny<Func<IQueryable<VisitedPoint>, IOrderedQueryable<VisitedPoint>>>(),
                It.IsAny<string>()), Times.Once);
            visitedRepositoryMock.Verify(r => r.Insert(It.Is<VisitedPoint>(x => x.Equals(visitedPoint))), Times.Once);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        public void AddNoTeam()
        {
            var userId = Guid.NewGuid();
            var teamId = Guid.NewGuid();

            var visitedPoint = new VisitedPoint
            {
                TeamId = teamId
            };

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);

            var visitedRepositoryMock = new Mock<IGenericRepository<VisitedPoint>>();
            _UnitOfWorkMock.Setup(u => u.VisitedPointRepository).Returns(visitedRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Add(userId, visitedPoint));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);

            visitedRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<VisitedPoint, bool>>>(),
                It.IsAny<Func<IQueryable<VisitedPoint>, IOrderedQueryable<VisitedPoint>>>(),
                It.IsAny<string>()), Times.Never);
            visitedRepositoryMock.Verify(r => r.Insert(It.Is<VisitedPoint>(x => x.Equals(visitedPoint))), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void AddNotAuthorized()
        {
            var userId = Guid.NewGuid();
            var teamId = Guid.NewGuid();

            SetupUnitOfWorkToPassAuthorizedTest(teamId);

            var visitedRepositoryMock = new Mock<IGenericRepository<VisitedPoint>>();
            _UnitOfWorkMock.Setup(u => u.VisitedPointRepository).Returns(visitedRepositoryMock.Object);

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            var visitedPoint = new VisitedPoint
            {
                TeamId = teamId
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Add(userId, visitedPoint));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);

            visitedRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<VisitedPoint, bool>>>(),
                It.IsAny<Func<IQueryable<VisitedPoint>, IOrderedQueryable<VisitedPoint>>>(),
                It.IsAny<string>()), Times.Never);
            visitedRepositoryMock.Verify(r => r.Insert(It.Is<VisitedPoint>(x => x.Equals(visitedPoint))), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void AddWrongAccessLevel()
        {
            var userId = Guid.NewGuid();
            var teamId = Guid.NewGuid();

            SetupUnitOfWorkToPassAuthorizedTest(teamId);

            var visitedRepositoryMock = new Mock<IGenericRepository<VisitedPoint>>();
            _UnitOfWorkMock.Setup(u => u.VisitedPointRepository).Returns(visitedRepositoryMock.Object);

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            userLinkRepositoryMock.Setup(r => r.Get(
                It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>())).Returns(new List<UserLink> { new UserLink { UserId = userId, RaceAccess = RaceAccessLevel.Read } });
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            var visitedPoint = new VisitedPoint
            {
                TeamId = teamId
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Add(userId, visitedPoint));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);

            visitedRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<VisitedPoint, bool>>>(),
                It.IsAny<Func<IQueryable<VisitedPoint>, IOrderedQueryable<VisitedPoint>>>(),
                It.IsAny<string>()), Times.Never);
            visitedRepositoryMock.Verify(r => r.Insert(It.Is<VisitedPoint>(x => x.Equals(visitedPoint))), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void AddDuplicate()
        {
            var userId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var pointId = Guid.NewGuid();

            var visitedPoint = new VisitedPoint
            {
                TeamId = teamId,
                PointId = pointId
            };

            SetupUnitOfWorkToPassAuthorizedTest(teamId);

            var visitedRepositoryMock = new Mock<IGenericRepository<VisitedPoint>>();
            visitedRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<VisitedPoint, bool>>>(),
                It.IsAny<Func<IQueryable<VisitedPoint>, IOrderedQueryable<VisitedPoint>>>(),
                It.IsAny<string>())).Returns(new List<VisitedPoint> { visitedPoint });

            _UnitOfWorkMock.Setup(u => u.VisitedPointRepository).Returns(visitedRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Add(userId, visitedPoint));

            Assert.AreEqual(BLErrorCodes.Duplicate, exception.ErrorCode);
            Assert.AreEqual($"Visited point with ID '{pointId}' is already known", exception.Message);

            visitedRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<VisitedPoint, bool>>>(),
                It.IsAny<Func<IQueryable<VisitedPoint>, IOrderedQueryable<VisitedPoint>>>(),
                It.IsAny<string>()), Times.Once);
            visitedRepositoryMock.Verify(r => r.Insert(It.Is<VisitedPoint>(x => x.Equals(visitedPoint))), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void EditNotImplemented()
        {
            Assert.ThrowsException<NotImplementedException>(() => _Sut.Edit(Guid.NewGuid(), new VisitedPoint()));
        }

        [TestMethod]
        public void Delete()
        {
            var userId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var visitedPointId = Guid.NewGuid();

            var visitedPoint = new VisitedPoint
            {
                TeamId = teamId
            };

            SetupUnitOfWorkToPassAuthorizedTest(teamId);

            var visitedRepositoryMock = new Mock<IGenericRepository<VisitedPoint>>();
            visitedRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(visitedPointId)))).Returns(visitedPoint);
            _UnitOfWorkMock.Setup(u => u.VisitedPointRepository).Returns(visitedRepositoryMock.Object);

            _Sut.Delete(userId, visitedPointId);

            visitedRepositoryMock.Verify(r => r.GetByID(It.Is<Guid>(g => g.Equals(visitedPointId))), Times.Once);
            visitedRepositoryMock.Verify(r => r.Delete(It.Is<VisitedPoint>(g => g.Equals(visitedPoint))), Times.Once);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        public void DeleteNoTeam()
        {
            var userId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var visitedPointId = Guid.NewGuid();

            var visitedPoint = new VisitedPoint
            {
                TeamId = teamId
            };

            var visitedRepositoryMock = new Mock<IGenericRepository<VisitedPoint>>();
            visitedRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(visitedPointId)))).Returns(visitedPoint);
            _UnitOfWorkMock.Setup(u => u.VisitedPointRepository).Returns(visitedRepositoryMock.Object);

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Delete(userId, visitedPointId));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);

            teamRepositoryMock.Verify(r => r.GetByID(It.Is<Guid>(g => g.Equals(teamId))), Times.Once);

            visitedRepositoryMock.Verify(r => r.GetByID(It.Is<Guid>(g => g.Equals(visitedPointId))), Times.Once);
            visitedRepositoryMock.Verify(r => r.Delete(It.IsAny<VisitedPoint>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void DeleteNotAuthorized()
        {
            var userId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var visitedPointId = Guid.NewGuid();

            SetupUnitOfWorkToPassAuthorizedTest(teamId);

            var visitedPoint = new VisitedPoint
            {
                VisitedPointId = visitedPointId,
                TeamId = teamId
            };

            var visitedRepositoryMock = new Mock<IGenericRepository<VisitedPoint>>();
            visitedRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(visitedPointId)))).Returns(visitedPoint);
            _UnitOfWorkMock.Setup(u => u.VisitedPointRepository).Returns(visitedRepositoryMock.Object);

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Delete(userId, visitedPointId));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);

            visitedRepositoryMock.Verify(r => r.GetByID(It.Is<Guid>(g => g.Equals(visitedPointId))), Times.Once);
            visitedRepositoryMock.Verify(r => r.Delete(It.IsAny<VisitedPoint>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void DeleteWrongAccessLevel()
        {
            var userId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var visitedPointId = Guid.NewGuid();

            SetupUnitOfWorkToPassAuthorizedTest(teamId);

            var visitedPoint = new VisitedPoint
            {
                VisitedPointId = visitedPointId,
                TeamId = teamId
            };

            var visitedRepositoryMock = new Mock<IGenericRepository<VisitedPoint>>();
            visitedRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(visitedPointId)))).Returns(visitedPoint);
            _UnitOfWorkMock.Setup(u => u.VisitedPointRepository).Returns(visitedRepositoryMock.Object);

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            userLinkRepositoryMock.Setup(r => r.Get(
                It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>())).Returns(new List<UserLink> { new UserLink { UserId = userId, RaceAccess = RaceAccessLevel.Read } });
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Delete(userId, visitedPointId));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);

            visitedRepositoryMock.Verify(r => r.GetByID(It.Is<Guid>(g => g.Equals(visitedPointId))), Times.Once);
            visitedRepositoryMock.Verify(r => r.Delete(It.IsAny<VisitedPoint>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void DeleteVisitedPointNotFound()
        {
            var userId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var visitedPointId = Guid.NewGuid();

            SetupUnitOfWorkToPassAuthorizedTest(teamId);

            var visitedRepositoryMock = new Mock<IGenericRepository<VisitedPoint>>();

            _UnitOfWorkMock.Setup(u => u.VisitedPointRepository).Returns(visitedRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Delete(userId, visitedPointId));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.AreEqual($"Visited point with ID '{visitedPointId}' is unknown", exception.Message);

            visitedRepositoryMock.Verify(r => r.GetByID(It.Is<Guid>(g => g.Equals(visitedPointId))), Times.Once);
            visitedRepositoryMock.Verify(r => r.Delete(It.Is<Guid>(g => g.Equals(visitedPointId))), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        private void SetupUnitOfWorkToPassAuthorizedTest(Guid teamId)
        {
            var raceId = Guid.NewGuid();

            var team = new Team
            {
                RaceId = raceId
            };

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            teamRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(teamId)))).Returns(team);

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            userLinkRepositoryMock.Setup(r => r.Get(
                It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>())).Returns(new List<UserLink> { new UserLink() });

            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);
        }
    }
}
