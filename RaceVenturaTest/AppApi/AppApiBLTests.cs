using RaceVentura;
using RaceVenturaTestUtils;
using RaceVentura.AppApi;
using RaceVentura.Models;
using RaceVenturaData;
using RaceVenturaData.Models.Races;
using RaceVenturaData.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RaceVenturaTest.AppApi
{
    [TestClass]
    public class AppApiBLTests
    {
        private readonly Mock<ILogger<AppApiBL>> _LoggerMock = new Mock<ILogger<AppApiBL>>();
        private readonly Mock<IRaceVenturaUnitOfWork> _UnitOfWorkMock = new Mock<IRaceVenturaUnitOfWork>();
        private AppApiBL _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new AppApiBL(_UnitOfWorkMock.Object, _LoggerMock.Object);
        }

        [TestMethod]
        public void ConstructorArguments()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new AppApiBL(null, _LoggerMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new AppApiBL(_UnitOfWorkMock.Object, null));
        }

        #region RegisterRace
        [TestMethod]
        public void RegisterToRaceNoErrors()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();

            var regiteredIdRepositoryMock = new Mock<IGenericRepository<RegisteredId>>();
            SetupMocksForRegisterToRace(raceId, teamId, new Mock<IGenericRepository<Team>>(), regiteredIdRepositoryMock, new List<RegisteredId>());

            _Sut.RegisterToRace(raceId, teamId, uniqueId);

            regiteredIdRepositoryMock.Verify(r => r.Insert(It.Is<RegisteredId>(id => id.TeamId == teamId && id.UniqueId.Equals(uniqueId))), Times.Once);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);
            _LoggerMock.VerifyLog(LogLevel.Error, Times.Never);
        }

        [TestMethod]
        public void RegisterToRaceUnknownRace()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();

            var regiteredIdRepositoryMock = new Mock<IGenericRepository<RegisteredId>>();
            SetupMocksForRegisterToRace(raceId, teamId, new Mock<IGenericRepository<Team>>(), regiteredIdRepositoryMock, new List<RegisteredId>());

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterToRace(raceId, teamId, uniqueId));

            AssertUnknownRace(raceId, exception);
        }

        [TestMethod]
        public void RegisterToRaceUnknownTeam()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            raceRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(raceId)))).Returns(new Race { MaximumTeamSize = 2 });
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterToRace(raceId, teamId, uniqueId));

            AssertUnknownTeam(teamId, exception);
        }

        [TestMethod]
        public void RegisterToRaceToManyIds()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();

            var regiteredIdRepositoryMock = new Mock<IGenericRepository<RegisteredId>>();
            SetupMocksForRegisterToRace(raceId, teamId, new Mock<IGenericRepository<Team>>(), regiteredIdRepositoryMock, new List<RegisteredId> { new RegisteredId(), new RegisteredId() });

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterToRace(raceId, teamId, uniqueId));

            Assert.AreEqual(BLErrorCodes.MaxIdsReached, exception.ErrorCode);
            Assert.AreEqual($"Maximum of registered ID's reached.", exception.Message);

            regiteredIdRepositoryMock.Verify(r => r.Insert(It.IsAny<RegisteredId>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
            _LoggerMock.VerifyLog(LogLevel.Error, Times.Once, $"Error in AppApiBL: Someone tried to registers to many ID's to team with id '{raceId}'.");
        }

        [TestMethod]
        public void RegisterToRaceIdAlreadyRegistered()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();

            var regiteredIdRepositoryMock = new Mock<IGenericRepository<RegisteredId>>();
            SetupMocksForRegisterToRace(raceId, teamId, new Mock<IGenericRepository<Team>>(), regiteredIdRepositoryMock, new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } });

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterToRace(raceId, teamId, uniqueId));

            Assert.AreEqual(BLErrorCodes.Duplicate, exception.ErrorCode);
            Assert.AreEqual($"Unique ID '{uniqueId}' allready registered.", exception.Message);

            regiteredIdRepositoryMock.Verify(r => r.Insert(It.IsAny<RegisteredId>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
            _LoggerMock.VerifyLog(LogLevel.Error, Times.Once, $"Error in AppApiBL: Someone tried to registers the unique ID '{uniqueId}' twice.");
        }
        #endregion

        #region RegisterPoint
        [TestMethod]
        public void RegisterPointNoErrors()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();
            var latitude = 0;
            var longitude = 0;
            var answer = "";

            var visitedPointsRepositoryMock = SetupMocksForRegisterPoint(raceId, false, teamId, stageId, 1, pointId, stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, latitude, longitude);

            var result = _Sut.RegisterPoint(raceId, uniqueId, pointId, latitude, longitude, answer);

            AssertRegisterPointNoErrors(result, teamId, pointId, visitedPointsRepositoryMock);
        }

        [TestMethod]
        public void RegisterPointUnknownRace()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();
            var latitude = 0;
            var longitude = 0;
            var answer = "";

            var visitedPointsRepositoryMock = SetupMocksForRegisterPoint(Guid.NewGuid(), false, teamId, stageId, 1, pointId, stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, latitude, longitude);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterPoint(raceId, uniqueId, pointId, latitude, longitude, answer));

            visitedPointsRepositoryMock.Verify(r => r.Insert(It.IsAny<VisitedPoint>()), Times.Never);
            AssertUnknownRace(raceId, exception);
        }

        [TestMethod]
        public void RegisterPointUnknownTeam()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();
            var latitude = 0;
            var longitude = 0;
            var answer = "";

            var visitedPointsRepositoryMock = SetupMocksForRegisterPoint(raceId, false, Guid.NewGuid(), stageId, 1, pointId, stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, latitude, longitude);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterPoint(raceId, uniqueId, pointId, latitude, longitude, answer));

            visitedPointsRepositoryMock.Verify(r => r.Insert(It.IsAny<VisitedPoint>()), Times.Never);
            AssertUnknownTeam(teamId, exception);
        }

        [TestMethod]
        public void RegisterPointUnknownStage()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();
            var latitude = 0;
            var longitude = 0;
            var answer = "";

            var visitedPointsRepositoryMock = SetupMocksForRegisterPoint(raceId, false, teamId, Guid.NewGuid(), 1, pointId, stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, latitude, longitude);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterPoint(raceId, uniqueId, pointId, latitude, longitude, answer));

            visitedPointsRepositoryMock.Verify(r => r.Insert(It.IsAny<VisitedPoint>()), Times.Never);
            AssertUnknownStage(stageId, exception);
        }

        [TestMethod]
        public void RegisterPointUnknownPoint()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();
            var latitude = 0;
            var longitude = 0;
            var answer = "";

            var visitedPointsRepositoryMock = SetupMocksForRegisterPoint(raceId, false, teamId, stageId, 1, Guid.NewGuid(), stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, latitude, longitude);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterPoint(raceId, uniqueId, pointId, latitude, longitude, answer));

            visitedPointsRepositoryMock.Verify(r => r.Insert(It.IsAny<VisitedPoint>()), Times.Never);
            AssertUnknownPoint(pointId, exception);
        }

        [TestMethod]
        public void RegisterPointUnknownUniqueId()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();
            var latitude = 0;
            var longitude = 0;
            var answer = "";

            var registeredIds = new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = Guid.NewGuid() } };

            var visitedPointsRepositoryMock = SetupMocksForRegisterPoint(raceId, false, teamId, stageId, 1, pointId, stageId, null, latitude, longitude);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterPoint(raceId, uniqueId, pointId, latitude, longitude, answer));

            visitedPointsRepositoryMock.Verify(r => r.Insert(It.IsAny<VisitedPoint>()), Times.Never);
            AssertUnknownUniqueId(uniqueId, exception);
        }

        [TestMethod]
        public void RegisterPointWrongStageNumber()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();
            var latitude = 0;
            var longitude = 0;
            var answer = "";

            var visitedPointsRepositoryMock = SetupMocksForRegisterPoint(raceId, false, teamId, stageId, 2, pointId, stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, latitude, longitude);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterPoint(raceId, uniqueId, pointId, latitude, longitude, answer));

            Assert.AreEqual(BLErrorCodes.NotActiveStage, exception.ErrorCode);
            Assert.AreEqual($"Point with ID '{pointId}' not in active stage.", exception.Message);

            visitedPointsRepositoryMock.Verify(r => r.Insert(It.IsAny<VisitedPoint>()), Times.Never);
        }

        [TestMethod]
        public void RegisterPointIncorrectTime()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();
            var latitude = 0;
            var longitude = 0;
            var answer = "";

            var visitedPointsRepositoryMock = SetupMocksForRegisterPoint(raceId, false, teamId, stageId, 1, pointId, stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, latitude, longitude, 0, "", "", DateTime.Now.AddDays(1));

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterPoint(raceId, uniqueId, pointId, latitude, longitude, answer));

            visitedPointsRepositoryMock.Verify(r => r.Insert(It.IsAny<VisitedPoint>()), Times.Never);
            AssertIncorrectTime(exception);
        }

        [TestMethod]
        public void RegisterPointWrongCoordinatesWithCheckOutsideDeviation()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();
            var latitude = 0;
            var longitude = 0;
            var answer = "";

            var visitedPointsRepositoryMock = SetupMocksForRegisterPoint(raceId, true, teamId, stageId, 1, pointId, stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, 1, 1);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterPoint(raceId, uniqueId, pointId, latitude, longitude, answer));

            Assert.AreEqual(BLErrorCodes.CoordinatesIncorrect, exception.ErrorCode);
            Assert.AreEqual($"Coordinates incorrect, latitude '{latitude}', longitude '{longitude}'", exception.Message);

            visitedPointsRepositoryMock.Verify(r => r.Insert(It.IsAny<VisitedPoint>()), Times.Never);
        }

        [TestMethod]
        public void RegisterPointWrongCoordinatesWithCheckInsideDeviation()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();
            var latitude = 0;
            var longitude = 0;
            var answer = "";

            var visitedPointsRepositoryMock = SetupMocksForRegisterPoint(raceId, true, teamId, stageId, 1, pointId, stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, 2, 2, 2);

            var result = _Sut.RegisterPoint(raceId, uniqueId, pointId, latitude, longitude, answer);

            AssertRegisterPointNoErrors(result, teamId, pointId, visitedPointsRepositoryMock);
        }

        [TestMethod]
        public void RegisterPointWrongCoordinatesWithoutCheck()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();
            var latitude = 0;
            var longitude = 0;
            var answer = "";

            var visitedPointsRepositoryMock = SetupMocksForRegisterPoint(raceId, false, teamId, stageId, 1, pointId, stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, 1, 1);

            var result = _Sut.RegisterPoint(raceId, uniqueId, pointId, latitude, longitude, answer);

            AssertRegisterPointNoErrors(result, teamId, pointId, visitedPointsRepositoryMock);
        }

        [TestMethod]
        public void RegisterPointAlreadyRegistered()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();
            var latitude = 0;
            var longitude = 0;
            var answer = "";

            var visitedPointsRepositoryMock = SetupMocksForRegisterPoint(raceId, false, teamId, stageId, 1, pointId, stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, latitude, longitude, 0);

            visitedPointsRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<VisitedPoint, bool>>>(),
                        It.IsAny<Func<IQueryable<VisitedPoint>, IOrderedQueryable<VisitedPoint>>>(),
                        It.IsAny<string>())).Returns(new List<VisitedPoint> { new VisitedPoint() });

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterPoint(raceId, uniqueId, pointId, latitude, longitude, answer));

            Assert.AreEqual(BLErrorCodes.Duplicate, exception.ErrorCode);
            Assert.AreEqual($"Point already registered '{pointId}'.", exception.Message);
            visitedPointsRepositoryMock.Verify(r => r.Insert(It.IsAny<VisitedPoint>()), Times.Never);
        }

        [TestMethod]
        public void RegisterPointWithMessageWithoutAnswer()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();
            var latitude = 0;
            var longitude = 0;
            var message = "message";
            var answer = " ";

            var visitedPointsRepositoryMock = SetupMocksForRegisterPoint(raceId, false, teamId, stageId, 1, pointId, stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, latitude, longitude, 0, message);

            var result = _Sut.RegisterPoint(raceId, uniqueId, pointId, latitude, longitude, answer);

            Assert.AreEqual(message, result);

            visitedPointsRepositoryMock.Verify(r => r.Insert(It.IsAny<VisitedPoint>()), Times.Never);
        }

        [TestMethod]
        public void RegisterPointWithMessageWithIncorrectAnswer()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();
            var latitude = 0;
            var longitude = 0;
            var message = "message";
            var answer = "zes";

            var visitedPointsRepositoryMock = SetupMocksForRegisterPoint(raceId, false, teamId, stageId, 1, pointId, stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, latitude, longitude, 0, message, "een");

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterPoint(raceId, uniqueId, pointId, latitude, longitude, answer));

            Assert.AreEqual(BLErrorCodes.AnswerIncorrect, exception.ErrorCode);
            Assert.AreEqual($"Answer '{answer}' is incorrect.", exception.Message);

            visitedPointsRepositoryMock.Verify(r => r.Insert(It.IsAny<VisitedPoint>()), Times.Never);
        }

        [TestMethod]
        public void RegisterPointWithMessageWithCorrectAnswer()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var pointId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();
            var latitude = 0;
            var longitude = 0;
            var message = "message";
            var answer = "zes";

            var visitedPointsRepositoryMock = SetupMocksForRegisterPoint(raceId, false, teamId, stageId, 1, pointId, stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, latitude, longitude, 0, message, answer);

            var result = _Sut.RegisterPoint(raceId, uniqueId, pointId, latitude, longitude, answer);

            AssertRegisterPointNoErrors(result, teamId, pointId, visitedPointsRepositoryMock);
        }
        #endregion

        #region RegisterStageEnd
        [TestMethod]
        public void RegisterStageEndNoErrors()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();

            SetupMockForRegisterStageEnd(raceId, teamId, stageId, new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, out var teamRepositoryMock, out var finishedStageRepositoryMock);

            _Sut.RegisterStageEnd(raceId, uniqueId, stageId);

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);
            finishedStageRepositoryMock.Verify(r => r.Insert(It.Is<FinishedStage>(s => s.TeamId == teamId && s.StageId == stageId &&
            s.FinishTime.Year == DateTime.Now.Year &&
            s.FinishTime.Month == DateTime.Now.Month &&
            s.FinishTime.Day == DateTime.Now.Day &&
            s.FinishTime.Hour == DateTime.Now.Hour &&
            s.FinishTime.Minute == DateTime.Now.Minute &&
            s.FinishTime.Second == DateTime.Now.Second
            )), Times.Once);
            teamRepositoryMock.Verify(r => r.Update(It.Is<Team>(t => t.ActiveStage == 2)), Times.Once);

            _LoggerMock.VerifyLog(LogLevel.Error, Times.Never);
        }

        [TestMethod]
        public void RegisterStageEndUnknownRace()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();

            SetupMockForRegisterStageEnd(Guid.NewGuid(), teamId, stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, out var teamRepositoryMock, out var finishedStageRepositoryMock);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterStageEnd(raceId, uniqueId, stageId));

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            finishedStageRepositoryMock.Verify(r => r.Insert(It.IsAny<FinishedStage>()), Times.Never);
            AssertUnknownRace(raceId, exception);
        }

        [TestMethod]
        public void RegisterStageEndUnknownTeam()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();

            SetupMockForRegisterStageEnd(raceId, Guid.NewGuid(), stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, out var teamRepositoryMock, out var finishedStageRepositoryMock);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterStageEnd(raceId, uniqueId, stageId));

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            finishedStageRepositoryMock.Verify(r => r.Insert(It.IsAny<FinishedStage>()), Times.Never);
            AssertUnknownTeam(teamId, exception);
        }

        [TestMethod]
        public void RegisterStageEndUnknownStage()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();

            SetupMockForRegisterStageEnd(raceId, teamId, Guid.NewGuid(),
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, out var teamRepositoryMock, out var finishedStageRepositoryMock);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterStageEnd(raceId, uniqueId, stageId));

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            finishedStageRepositoryMock.Verify(r => r.Insert(It.IsAny<FinishedStage>()), Times.Never);
            AssertUnknownStage(stageId, exception);
        }

        [TestMethod]
        public void RegisterStageEndUnknownUniqueId()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();

            SetupMockForRegisterStageEnd(raceId, teamId, stageId, null, out var teamRepositoryMock, out var finishedStageRepositoryMock);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterStageEnd(raceId, uniqueId, stageId));

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            finishedStageRepositoryMock.Verify(r => r.Insert(It.IsAny<FinishedStage>()), Times.Never);
            AssertUnknownUniqueId(uniqueId, exception);
        }

        [TestMethod]
        public void RegisterStageEndWrongStageNumber()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();

            SetupMockForRegisterStageEnd(Guid.NewGuid(), teamId, stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, out var teamRepositoryMock, out var finishedStageRepositoryMock, 2);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterStageEnd(raceId, uniqueId, stageId));

            Assert.AreEqual(BLErrorCodes.NotActiveStage, exception.ErrorCode);
            Assert.AreEqual($"Stage with ID '{stageId}' is not the active stage.", exception.Message);

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            finishedStageRepositoryMock.Verify(r => r.Insert(It.IsAny<FinishedStage>()), Times.Never);
        }

        [TestMethod]
        public void RegisterStageEndIncorrectTime()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();

            SetupMockForRegisterStageEnd(raceId, teamId, stageId,
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } },
                out var teamRepositoryMock, out var finishedStageRepositoryMock, 1, DateTime.Now.AddDays(1));

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterStageEnd(raceId, uniqueId, stageId));

            AssertIncorrectTime(exception);

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            finishedStageRepositoryMock.Verify(r => r.Insert(It.IsAny<FinishedStage>()), Times.Never);
        }
        #endregion

        #region RegisterRaceEnd
        [TestMethod]
        public void RegisterRaceEndNoErrors()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            SetupMocksForRegisterToRace(raceId, teamId, teamRepositoryMock, new Mock<IGenericRepository<RegisteredId>>(),
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } });

            _Sut.RegisterRaceEnd(raceId, uniqueId);

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);

            teamRepositoryMock.Verify(r => r.Update(It.Is<Team>(t =>
            t.FinishTime.Year == DateTime.Now.Year &&
            t.FinishTime.Month == DateTime.Now.Month &&
            t.FinishTime.Day == DateTime.Now.Day &&
            t.FinishTime.Hour == DateTime.Now.Hour &&
            t.FinishTime.Minute == DateTime.Now.Minute &&
            t.FinishTime.Second == DateTime.Now.Second)), Times.Once);

            _LoggerMock.VerifyLog(LogLevel.Error, Times.Never);
        }

        [TestMethod]
        public void RegisterRaceEndUnknownRace()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            SetupMocksForRegisterToRace(Guid.NewGuid(), teamId, teamRepositoryMock, new Mock<IGenericRepository<RegisteredId>>(),
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } });

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterRaceEnd(raceId, uniqueId));

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            AssertUnknownRace(raceId, exception);
        }

        [TestMethod]
        public void RegisterRaceEndUnknownTeam()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            SetupMocksForRegisterToRace(raceId, Guid.NewGuid(), teamRepositoryMock, new Mock<IGenericRepository<RegisteredId>>(),
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } });

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterRaceEnd(raceId, uniqueId));

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            AssertUnknownTeam(teamId, exception);
        }

        [TestMethod]
        public void RegisterRaceEndUnknownUniqueId()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var uniqueId = Guid.NewGuid();

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            SetupMocksForRegisterToRace(raceId, teamId, teamRepositoryMock, new Mock<IGenericRepository<RegisteredId>>(), null);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterRaceEnd(raceId, uniqueId));

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            AssertUnknownUniqueId(uniqueId, exception);
        }
        #endregion

        private void SetupMocksForRegisterToRace(Guid raceId, Guid teamId, Mock<IGenericRepository<Team>> teamRepositoryMock, Mock<IGenericRepository<RegisteredId>> regiteredIdRepositoryMock, List<RegisteredId> registeredIds, DateTime? startTime = null, bool checkCoordinates = false, double deviation = 0)
        {
            if (!startTime.HasValue)
            {
                startTime = DateTime.Now;
            }

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            raceRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(raceId)))).Returns(new Race { AllowedCoordinatesDeviation = deviation, CoordinatesCheckEnabled = checkCoordinates, MaximumTeamSize = 2, StartTime = startTime.Value });
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            teamRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(teamId)))).Returns(new Team { TeamId = teamId, ActiveStage = 1 });
            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);

            regiteredIdRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<RegisteredId, bool>>>(),
                        It.IsAny<Func<IQueryable<RegisteredId>, IOrderedQueryable<RegisteredId>>>(),
                        It.IsAny<string>())).Returns(registeredIds);
            _UnitOfWorkMock.Setup(u => u.RegisteredIdRepository).Returns(regiteredIdRepositoryMock.Object);
        }

        private void SetupMockForRegisterStageEnd(Guid raceId, Guid teamId, Guid stageId, List<RegisteredId> registeredIds, out Mock<IGenericRepository<Team>> teamRepositoryMock, out Mock<IGenericRepository<FinishedStage>> finishedStageRepositoryMock, int stageNumber = 1, DateTime? startTime = null)
        {
            teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            SetupMocksForRegisterToRace(raceId, teamId, teamRepositoryMock, new Mock<IGenericRepository<RegisteredId>>(), registeredIds, startTime);

            var stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            stageRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(stageId)))).Returns(new Stage { Number = stageNumber });
            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);

            finishedStageRepositoryMock = new Mock<IGenericRepository<FinishedStage>>();
            _UnitOfWorkMock.Setup(u => u.FinishedStageRepository).Returns(finishedStageRepositoryMock.Object);
        }

        private Mock<IGenericRepository<VisitedPoint>> SetupMocksForRegisterPoint(Guid raceId, bool checkCoordinates, Guid teamId, Guid stageId, int stageNumber, Guid pointId, Guid pointStageId, List<RegisteredId> registeredIds, double latitude, double longitude, double deviation = 0, string message = "", string answer = "", DateTime? startTime = null)
        {
            SetupMocksForRegisterToRace(raceId, teamId, new Mock<IGenericRepository<Team>>(), new Mock<IGenericRepository<RegisteredId>>(), registeredIds, startTime, checkCoordinates, deviation);

            var stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            stageRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(stageId)))).Returns(new Stage { Number = stageNumber });
            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);

            var pointReposityrMock = new Mock<IGenericRepository<Point>>();
            pointReposityrMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(pointId)))).Returns(new Point
            {
                PointId = pointId,
                StageId = pointStageId,
                Latitude = latitude,
                Longitude = longitude,
                Message = message,
                Answer = answer
            });
            _UnitOfWorkMock.Setup(u => u.PointRepository).Returns(pointReposityrMock.Object);

            var visitedPointsRepositoryMock = new Mock<IGenericRepository<VisitedPoint>>();
            _UnitOfWorkMock.Setup(u => u.VisitedPointRepository).Returns(visitedPointsRepositoryMock.Object);
            return visitedPointsRepositoryMock;
        }

        private void AssertUnknownRace(Guid raceId, BusinessException exception)
        {
            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.AreEqual($"Race with ID '{raceId}' does not exsist.", exception.Message);

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
            _LoggerMock.VerifyLog(LogLevel.Error, Times.Once, $"Error in AppApiBL: Someone tried to access race with ID '{raceId}' but it does not exsist.");
        }

        private void AssertUnknownTeam(Guid teamId, BusinessException exception)
        {
            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.AreEqual($"Team with ID '{teamId}' is unknown", exception.Message);

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
            _LoggerMock.VerifyLog(LogLevel.Error, Times.Once, $"Error in AppApiBL: Someone tried to access team with ID '{teamId}' but it does not exsist.");
        }

        private void AssertUnknownStage(Guid stageId, BusinessException exception)
        {
            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.AreEqual($"Stage with ID '{stageId}' not found.", exception.Message);

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
            _LoggerMock.VerifyLog(LogLevel.Error, Times.Once, $"Error in AppApiBL: Someone tried to access stage with id '{stageId}' but it does not exsist.");
        }

        private void AssertUnknownPoint(Guid pointId, BusinessException exception)
        {
            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.AreEqual($"Point with ID '{pointId}' not found.", exception.Message);

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
            _LoggerMock.VerifyLog(LogLevel.Error, Times.Once, $"Error in AppApiBL: Someone tried to access point with id '{pointId}' but it does not exsist.");
        }

        private void AssertUnknownUniqueId(Guid uniqueId, BusinessException exception)
        {
            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);
            Assert.AreEqual($"RegisteredID with uniqueID '{uniqueId}' is unknown", exception.Message);

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
            _LoggerMock.VerifyLog(LogLevel.Error, Times.Once, $"Error in AppApiBL: Someone tried to register a point but the unique id '{uniqueId}' does not exsist.");
        }

        private static void AssertIncorrectTime(BusinessException exception)
        {
            Assert.AreEqual(BLErrorCodes.RaceNotStarted, exception.ErrorCode);
            Assert.AreEqual($"Race not started yet", exception.Message);
        }

        private void AssertRegisterPointNoErrors(string result, Guid teamId, Guid pointId, Mock<IGenericRepository<VisitedPoint>> visitedPointsRepositoryMock)
        {
            Assert.AreEqual("", result);

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);

            visitedPointsRepositoryMock.Verify(r => r.Insert(It.Is<VisitedPoint>(p => p.TeamId == teamId && p.PointId == pointId &&
             p.Time.Year == DateTime.Now.Year &&
             p.Time.Month == DateTime.Now.Month &&
             p.Time.Day == DateTime.Now.Day &&
             p.Time.Hour == DateTime.Now.Hour &&
             p.Time.Minute == DateTime.Now.Minute &&
             p.Time.Second == DateTime.Now.Second
            )), Times.Once);

            _LoggerMock.VerifyLog(LogLevel.Error, Times.Never);
        }
    }
}
