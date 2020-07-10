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
    public class PointBLTests
    {
        private readonly Mock<ILogger<PointBL>> _LoggerMock = new Mock<ILogger<PointBL>> ();
        private readonly Mock<IRaceVenturaUnitOfWork> _UnitOfWorkMock = new Mock<IRaceVenturaUnitOfWork>();
        private PointBL _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new PointBL(_UnitOfWorkMock.Object, _LoggerMock.Object);
        }

        [TestMethod]
        public void Add()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();

            SetupStageRepo(raceId, stageId);

            var point = new Point
            {
                StageId = stageId
            };

            var pointRepositoryMock = new Mock<IGenericRepository<Point>>();
            _UnitOfWorkMock.Setup(u => u.PointRepository).Returns(pointRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            _Sut.Add(userId, point);

            pointRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<Point, bool>>>(),
                It.IsAny<Func<IQueryable<Point>, IOrderedQueryable<Point>>>(),
                It.IsAny<string>()), Times.Once);
            pointRepositoryMock.Verify(r => r.Insert(It.Is<Point>(x => x.Equals(point))), Times.Once);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        public void AddStageDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();

            SetupStageRepo(raceId, stageId);

            var pointRepositoryMock = new Mock<IGenericRepository<Point>>();
            _UnitOfWorkMock.Setup(u => u.PointRepository).Returns(pointRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var point = new Point
            {
                StageId = Guid.NewGuid()
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Add(userId, point));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);

            pointRepositoryMock.Verify(r => r.Insert(It.IsAny<Point>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void AddRaceDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();

            SetupStageRepo(raceId, stageId);

            var pointRepositoryMock = new Mock<IGenericRepository<Point>>();
            _UnitOfWorkMock.Setup(u => u.PointRepository).Returns(pointRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, Guid.NewGuid());

            var point = new Point
            {
                StageId = stageId
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Add(userId, point));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);

            pointRepositoryMock.Verify(r => r.Insert(It.IsAny<Point>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void AddNotAuthorizedForRace()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();

            SetupStageRepo(raceId, stageId);

            var pointRepositoryMock = new Mock<IGenericRepository<Point>>();
            _UnitOfWorkMock.Setup(u => u.PointRepository).Returns(pointRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { }, raceId);

            var point = new Point
            {
                StageId = stageId
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Add(userId, point));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);

            pointRepositoryMock.Verify(r => r.Insert(It.IsAny<Point>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void AddNumberExists()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();

            SetupStageRepo(raceId, stageId);

            var mockPoint = new Point
            {
                StageId = stageId,
                Name = "Test"
            };

            var pointRepositoryMock = new Mock<IGenericRepository<Point>>();
            pointRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<Point, bool>>>(),
                It.IsAny<Func<IQueryable<Point>, IOrderedQueryable<Point>>>(),
                It.IsAny<string>())).Returns(new List<Point> { mockPoint });

            _UnitOfWorkMock.Setup(u => u.PointRepository).Returns(pointRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var point = new Point
            {
                StageId = stageId,
                Name = "Test"
            };
            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Add(userId, point));

            Assert.AreEqual(BLErrorCodes.Duplicate, exception.ErrorCode);
            Assert.AreEqual($"A point with name '{point.Name}' already exists.", exception.Message);

            pointRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<Point, bool>>>(),
                It.IsAny<Func<IQueryable<Point>, IOrderedQueryable<Point>>>(),
                It.IsAny<string>()), Times.Once);
            pointRepositoryMock.Verify(r => r.Insert(It.Is<Point>(x => x.Equals(mockPoint))), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void Edit()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();

            SetupStageRepo(raceId, stageId);

            var pointMock = new Point
            {
                PointId = pointId,
                StageId = stageId,
                Answer = "a",
                Latitude = 1.1,
                Longitude = 1.2,
                Message = "m",
                Name = "Test",
                Type = PointType.CheckPoint,
                Value = 10
            };

            var pointRepositoryMock = new Mock<IGenericRepository<Point>>();
            pointRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(x => x.Equals(pointId)))).Returns(pointMock);

            _UnitOfWorkMock.Setup(u => u.PointRepository).Returns(pointRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var point = new Point
            {
                PointId = pointId,
                StageId = stageId,
                Answer = "b",
                Latitude = 1.31,
                Longitude = 1.42,
                Message = "n",
                Name = "Blaat",
                Type = PointType.QuestionCheckPoint,
                Value = 20
            };

            _Sut.Edit(userId, point);

            pointRepositoryMock.Verify(r => r.Update(It.Is<Point>(x =>
                x.Answer.Equals(point.Answer) &&
                x.Latitude == point.Latitude &&
                x.Longitude == point.Longitude &&
                x.Message.Equals(point.Message) &&
                x.Name.Equals(point.Name) &&
                x.Type == point.Type &&
                x.Value == point.Value
            )), Times.Once);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        public void EditPointDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();

            SetupStageRepo(raceId, stageId);
            SetupPointRepositoryMock(stageId, pointId, out var pointRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var point = new Point
            {
                PointId = Guid.NewGuid()
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, point));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);

            pointRepositoryMock.Verify(r => r.Update(It.IsAny<Point>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void EditStageDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();

            SetupStageRepo(raceId, Guid.NewGuid());
            SetupPointRepositoryMock(stageId, pointId, out var pointRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var point = new Point
            {
                PointId = pointId
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, point));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.IsTrue(exception.Message.Contains(stageId.ToString()));

            pointRepositoryMock.Verify(r => r.Update(It.IsAny<Point>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void EditRaceDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();

            SetupStageRepo(raceId, stageId);
            SetupPointRepositoryMock(stageId, pointId, out var pointRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, Guid.NewGuid());

            var point = new Point
            {
                PointId = pointId
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, point));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.IsTrue(exception.Message.Contains(raceId.ToString()));

            pointRepositoryMock.Verify(r => r.Update(It.IsAny<Point>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void EditNotAuthorizedForRace()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();

            SetupStageRepo(raceId, stageId);
            SetupPointRepositoryMock(stageId, pointId, out var pointRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { }, raceId);

            var point = new Point
            {
                PointId = pointId
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, point));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);

            pointRepositoryMock.Verify(r => r.Update(It.IsAny<Point>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void EditPointNameExists()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();

            SetupStageRepo(raceId, stageId);

            var pointMock = new Point
            {
                PointId = pointId,
                StageId = stageId,
                Name = "Test"
            };

            var pointRepositoryMock = new Mock<IGenericRepository<Point>>();
            pointRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(x => x.Equals(pointId)))).Returns(pointMock);
            pointRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<Point, bool>>>(),
                It.IsAny<Func<IQueryable<Point>, IOrderedQueryable<Point>>>(),
                It.IsAny<string>())).Returns(new List<Point>
                {
                    new Point{ StageId = stageId, Name = "test2" }
                });

            _UnitOfWorkMock.Setup(u => u.PointRepository).Returns(pointRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var point = new Point
            {
                PointId = pointId,
                Name = "Test2"
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, point));

            Assert.AreEqual(BLErrorCodes.Duplicate, exception.ErrorCode);
            Assert.AreEqual($"A point with name '{point.Name}' already exists.", exception.Message);

            pointRepositoryMock.Verify(r => r.Update(It.IsAny<Point>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void Delete()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();

            SetupStageRepo(raceId, stageId);

            var pointMock = new Point
            {
                PointId = pointId,
                StageId = stageId,
            };

            var pointRepositoryMock = new Mock<IGenericRepository<Point>>();
            pointRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(x => x.Equals(pointId)))).Returns(pointMock);

            _UnitOfWorkMock.Setup(u => u.PointRepository).Returns(pointRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            _Sut.Delete(userId, pointId);

            pointRepositoryMock.Verify(r => r.Delete(It.Is<Guid>(g => g.Equals(pointId))), Times.Once);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        public void DeletePointDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();

            SetupStageRepo(raceId, stageId);
            SetupPointRepositoryMock(stageId, pointId, out var pointRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Delete(userId, Guid.NewGuid()));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);

            pointRepositoryMock.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void DeleteStageDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();

            SetupStageRepo(raceId, Guid.NewGuid());
            SetupPointRepositoryMock(stageId, pointId, out var pointRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Delete(userId, pointId));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.IsTrue(exception.Message.Contains(stageId.ToString()));

            pointRepositoryMock.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void DeleteRaceDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();

            SetupStageRepo(raceId, stageId);
            SetupPointRepositoryMock(stageId, pointId, out var pointRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, Guid.NewGuid());

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Delete(userId, pointId));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.IsTrue(exception.Message.Contains(raceId.ToString()));

            pointRepositoryMock.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void DeleteNotAuthorizedForRace()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();

            SetupStageRepo(raceId, stageId);
            SetupPointRepositoryMock(stageId, pointId, out var pointRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { }, raceId);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Delete(userId, pointId));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);

            pointRepositoryMock.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        private void SetupStageRepo(Guid raceId, Guid stageId)
        {
            var stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            stageRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(stageId)))).Returns(new Stage { RaceId = raceId });

            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);
        }

        private void SetupPointRepositoryMock(Guid stageId, Guid pointId, out Mock<IGenericRepository<Point>> pointRepositoryMock)
        {
            var pointMock = new Point
            {
                PointId = pointId,
                StageId = stageId,
                Answer = "a",
                Latitude = 1.1,
                Longitude = 1.2,
                Message = "m",
                Name = "Test",
                Type = PointType.CheckPoint,
                Value = 10
            };

            pointRepositoryMock = new Mock<IGenericRepository<Point>>();
            pointRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(x => x.Equals(pointId)))).Returns(pointMock);

            _UnitOfWorkMock.Setup(u => u.PointRepository).Returns(pointRepositoryMock.Object);
        }
    }
}
