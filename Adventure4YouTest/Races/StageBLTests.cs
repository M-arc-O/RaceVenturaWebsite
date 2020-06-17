using Adventure4You;
using Adventure4You.Models;
using Adventure4You.Races;
using Adventure4YouData;
using Adventure4YouData.Models;
using Adventure4YouData.Models.Races;
using Adventure4YouData.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Adventure4YouTest.Races
{
    [TestClass]
    public class StageBLTests
    {
        private readonly Mock<ILogger<StageBL>> _LoggerMock = new Mock<ILogger<StageBL>>();
        private readonly Mock<IAdventure4YouUnitOfWork> _UnitOfWorkMock = new Mock<IAdventure4YouUnitOfWork>();
        private StageBL _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new StageBL(_UnitOfWorkMock.Object, _LoggerMock.Object);
        }

        [TestMethod]
        public void Add()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var stage = new Stage
            {
                RaceId = raceId
            };

            var stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            _Sut.Add(userId, stage);

            stageRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<Stage, bool>>>(),
                It.IsAny<Func<IQueryable<Stage>, IOrderedQueryable<Stage>>>(),
                It.IsAny<string>()), Times.Once);
            stageRepositoryMock.Verify(r => r.Insert(It.Is<Stage>(x => x.Equals(stage))), Times.Once);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        public void AddRaceDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var stage = new Stage
            {
                RaceId = raceId
            };

            var stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, Guid.NewGuid());

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Add(userId, stage));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);

            stageRepositoryMock.Verify(r => r.Insert(It.IsAny<Stage>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void AddNotAuthorizedForRace()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var stage = new Stage
            {
                RaceId = raceId
            };

            var stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { }, raceId);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Add(userId, stage));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);

            stageRepositoryMock.Verify(r => r.Insert(It.IsAny<Stage>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void AddNumberExists()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var mockStage = new Stage
            {
                RaceId = raceId,
                Name = "",
                Number = 1
            };

            var stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            stageRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<Stage, bool>>>(),
                It.IsAny<Func<IQueryable<Stage>, IOrderedQueryable<Stage>>>(),
                It.IsAny<string>())).Returns(new List<Stage> { mockStage });

            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var stage = new Stage
            {
                RaceId = raceId,
                Name = "Test",
                Number = 1
            };
            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Add(userId, stage));

            Assert.AreEqual(BLErrorCodes.Duplicate, exception.ErrorCode);
            Assert.AreEqual($"A stage with number '{stage.Number}' already exists.", exception.Message);

            stageRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<Stage, bool>>>(),
                It.IsAny<Func<IQueryable<Stage>, IOrderedQueryable<Stage>>>(),
                It.IsAny<string>()), Times.Once);
            stageRepositoryMock.Verify(r => r.Insert(It.Is<Stage>(x => x.Equals(mockStage))), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void Edit()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();

            SetupStageMock(raceId, stageId, out var stageRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var stage = new Stage
            {
                StageId = stageId,
                RaceId = raceId,
                Name = "Blaat",
                Number = 2
            };

            _Sut.Edit(userId, stage);

            stageRepositoryMock.Verify(r => r.Update(It.Is<Stage>(x =>
                x.Name.Equals(stage.Name) &&
                x.Number == stage.Number
            )), Times.Once);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        public void EditStageDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();

            SetupStageMock(raceId, stageId, out var stageRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var stage = new Stage
            {
                StageId = Guid.NewGuid()
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, stage));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);

            stageRepositoryMock.Verify(r => r.Update(It.IsAny<Stage>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void EditRaceDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();

            SetupStageMock(raceId, stageId, out var stageRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, Guid.NewGuid());

            var stage = new Stage
            {
                StageId = stageId,
                RaceId = raceId
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, stage));
            
            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.IsTrue(exception.Message.Contains(raceId.ToString()));

            stageRepositoryMock.Verify(r => r.Update(It.IsAny<Stage>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void EditNotAuthorizedForRace()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();

            SetupStageMock(raceId, stageId, out var stageRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { }, raceId);

            var stage = new Stage
            {
                StageId = stageId,
                RaceId = raceId,
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, stage));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);

            stageRepositoryMock.Verify(r => r.Update(It.IsAny<Stage>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void EditStageNumberExists()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();

            var stageMock = new Stage
            {
                StageId = stageId,
                RaceId = raceId,
                Name = "Test",
                Number = 1
            };

            var stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            stageRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(x => x.Equals(stageId)))).Returns(stageMock);
            stageRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<Stage, bool>>>(),
                It.IsAny<Func<IQueryable<Stage>, IOrderedQueryable<Stage>>>(),
                It.IsAny<string>())).Returns(new List<Stage>
                {
                    new Stage{ RaceId = raceId, Number = 3 }
                });

            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var stage = new Stage
            {
                StageId = stageId,
                Name = "Test",
                Number = 3,
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, stage));

            Assert.AreEqual(BLErrorCodes.Duplicate, exception.ErrorCode);
            Assert.AreEqual($"A stage with number '{stage.Number}' already exists.", exception.Message);

            stageRepositoryMock.Verify(r => r.Update(It.IsAny<Stage>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void Delete()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();

            SetupStageMock(raceId, stageId, out var stageRepositoryMock);

            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            _Sut.Delete(userId, stageId);

            stageRepositoryMock.Verify(r => r.Delete(It.Is<Guid>(g => g.Equals(stageId))), Times.Once);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        public void DeleteStageDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();

            SetupStageMock(raceId, stageId, out var stageRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Delete(userId, Guid.NewGuid()));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);

            stageRepositoryMock.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void DeleteRaceDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();

            SetupStageMock(raceId, stageId, out var stageRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, Guid.NewGuid());

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Delete(userId, stageId));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.IsTrue(exception.Message.Contains(raceId.ToString()));

            stageRepositoryMock.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void DeleteNotAuthorizedForRace()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var stageId = Guid.NewGuid();

            SetupStageMock(raceId, stageId, out var stageRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { }, raceId);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Delete(userId, stageId));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);

            stageRepositoryMock.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        private void SetupStageMock(Guid raceId, Guid stageId, out Mock<IGenericRepository<Stage>> stageRepositoryMock)
        {
            var stageMock = new Stage
            {
                StageId = stageId,
                RaceId = raceId,
                Name = "Test",
                Number = 1
            };

            stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            stageRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(x => x.Equals(stageId)))).Returns(stageMock);

            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);
        }
    }
}
