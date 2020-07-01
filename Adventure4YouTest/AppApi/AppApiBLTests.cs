using Adventure4You;
using Adventure4You.AppApi;
using Adventure4You.Models;
using Adventure4YouData;
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

namespace Adventure4YouTest.AppApi
{
    [TestClass]
    public class AppApiBLTests
    {
        private readonly Mock<ILogger<AppApiBL>> _LoggerMock = new Mock<ILogger<AppApiBL>>();
        private readonly Mock<IAdventure4YouUnitOfWork> _UnitOfWorkMock = new Mock<IAdventure4YouUnitOfWork>();
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

        [TestMethod]
        public void RegisterToRaceNoErrors()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var uniqueId = "unique";

            var regiteredIdRepositoryMock = new Mock<IGenericRepository<RegisteredId>>();
            SetupMocksForRegisterToRace(raceId, teamId, new Mock<IGenericRepository<Team>>(), regiteredIdRepositoryMock, new List<RegisteredId>());

            _Sut.RegisterToRace(raceId, teamId, uniqueId);

            regiteredIdRepositoryMock.Verify(r => r.Insert(It.Is<RegisteredId>(id => id.TeamId == teamId && id.UniqueId.Equals(uniqueId))), Times.Once);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once); 
            _LoggerMock.Verify(
             m => m.Log(
                 LogLevel.Error,
                 It.IsAny<EventId>(),
                 It.IsAny<FormattedLogValues>(),
                 It.IsAny<Exception>(),
                 It.IsAny<Func<object, Exception, string>>()),
             Times.Never);
        }

        [TestMethod]
        public void RegisterToRaceUnknownRace()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var uniqueId = "unique";

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
            var uniqueId = "unique";

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
            var uniqueId = "unique";

            var regiteredIdRepositoryMock = new Mock<IGenericRepository<RegisteredId>>();
            SetupMocksForRegisterToRace(raceId, teamId, new Mock<IGenericRepository<Team>>(), regiteredIdRepositoryMock, new List<RegisteredId> { new RegisteredId(), new RegisteredId() });

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterToRace(raceId, teamId, uniqueId));

            Assert.AreEqual(BLErrorCodes.MaxIdsReached, exception.ErrorCode);
            Assert.AreEqual($"Maximum of registered ID's reached.", exception.Message);

            regiteredIdRepositoryMock.Verify(r => r.Insert(It.IsAny<RegisteredId>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
            _LoggerMock.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().Equals($"Error in AppApiBL: Someone tried to registers to many ID's to team with id '{raceId}'.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }

        [TestMethod]
        public void RegisterToRaceIdAlreadyRegistered()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var uniqueId = "unique";

            var regiteredIdRepositoryMock = new Mock<IGenericRepository<RegisteredId>>();
            SetupMocksForRegisterToRace(raceId, teamId, new Mock<IGenericRepository<Team>>(), regiteredIdRepositoryMock, new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } });

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterToRace(raceId, teamId, uniqueId));

            Assert.AreEqual(BLErrorCodes.Duplicate, exception.ErrorCode);
            Assert.AreEqual($"Unique ID '{uniqueId}' allready registered.", exception.Message);

            regiteredIdRepositoryMock.Verify(r => r.Insert(It.IsAny<RegisteredId>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
            _LoggerMock.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().Equals($"Error in AppApiBL: Someone tried to registers the unique ID '{uniqueId}' twice.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }



        [TestMethod]
        public void RegisterStageEndNoErrors()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var uniqueId = "unique";

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            SetupMocksForRegisterToRace(raceId, teamId, teamRepositoryMock, new Mock<IGenericRepository<RegisteredId>>(),
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } });

            var stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            stageRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(stageId)))).Returns(new Stage { Number = 1 });
            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);

            var finishedStageRepositoryMock = new Mock<IGenericRepository<FinishedStage>>();
            _UnitOfWorkMock.Setup(u => u.FinishedStageRepository).Returns(finishedStageRepositoryMock.Object);

            _Sut.RegisterStageEnd(raceId, teamId, uniqueId, stageId);

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);
            finishedStageRepositoryMock.Verify(r=> r.Insert(It.Is<FinishedStage>(s => s.TeamId == teamId && s.StageId == stageId && 
            s.FinishTime.Year == DateTime.Now.Year &&
            s.FinishTime.Month == DateTime.Now.Month &&
            s.FinishTime.Day == DateTime.Now.Day &&
            s.FinishTime.Hour == DateTime.Now.Hour &&
            s.FinishTime.Minute == DateTime.Now.Minute &&
            s.FinishTime.Second == DateTime.Now.Second
            )), Times.Once);
            teamRepositoryMock.Verify(r => r.Update(It.Is<Team>(t => t.ActiveStage == 2)), Times.Once);


            _LoggerMock.Verify(
             m => m.Log(
                 LogLevel.Error,
                 It.IsAny<EventId>(),
                 It.IsAny<FormattedLogValues>(),
                 It.IsAny<Exception>(),
                 It.IsAny<Func<object, Exception, string>>()),
             Times.Never);
        }

        [TestMethod]
        public void RegisterStageEndUnknownRace()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var uniqueId = "unique";

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            SetupMocksForRegisterToRace(Guid.NewGuid(), teamId, teamRepositoryMock, new Mock<IGenericRepository<RegisteredId>>(),
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } });

            var stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            stageRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(stageId)))).Returns(new Stage { Number = 1 });
            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);

            var finishedStageRepositoryMock = new Mock<IGenericRepository<FinishedStage>>();
            _UnitOfWorkMock.Setup(u => u.FinishedStageRepository).Returns(finishedStageRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterStageEnd(raceId, teamId, uniqueId, stageId));

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
            var uniqueId = "unique";

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            SetupMocksForRegisterToRace(raceId, Guid.NewGuid(), teamRepositoryMock, new Mock<IGenericRepository<RegisteredId>>(),
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } });

            var stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            stageRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(stageId)))).Returns(new Stage { Number = 1 });
            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);

            var finishedStageRepositoryMock = new Mock<IGenericRepository<FinishedStage>>();
            _UnitOfWorkMock.Setup(u => u.FinishedStageRepository).Returns(finishedStageRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterStageEnd(raceId, teamId, uniqueId, stageId));

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
            var uniqueId = "unique";

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            SetupMocksForRegisterToRace(raceId, teamId, teamRepositoryMock, new Mock<IGenericRepository<RegisteredId>>(),
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } });

            var stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);

            var finishedStageRepositoryMock = new Mock<IGenericRepository<FinishedStage>>();
            _UnitOfWorkMock.Setup(u => u.FinishedStageRepository).Returns(finishedStageRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterStageEnd(raceId, teamId, uniqueId, stageId));

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
            var uniqueId = "unique";

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            SetupMocksForRegisterToRace(raceId, teamId, teamRepositoryMock, new Mock<IGenericRepository<RegisteredId>>(),
                null);

            var stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            stageRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(stageId)))).Returns(new Stage { Number = 1 });
            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);

            var finishedStageRepositoryMock = new Mock<IGenericRepository<FinishedStage>>();
            _UnitOfWorkMock.Setup(u => u.FinishedStageRepository).Returns(finishedStageRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterStageEnd(raceId, teamId, uniqueId, stageId));

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            finishedStageRepositoryMock.Verify(r => r.Insert(It.IsAny<FinishedStage>()), Times.Never);
            AssertUnknownUniqueId(teamId, uniqueId, exception);
        }

        [TestMethod]
        public void RegisterStageEndWrongStageNumber()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var stageId = Guid.NewGuid();
            var uniqueId = "unique";

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            SetupMocksForRegisterToRace(raceId, teamId, teamRepositoryMock, new Mock<IGenericRepository<RegisteredId>>(),
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } });

            var stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            stageRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(stageId)))).Returns(new Stage { Number = 2 });
            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);

            var finishedStageRepositoryMock = new Mock<IGenericRepository<FinishedStage>>();
            _UnitOfWorkMock.Setup(u => u.FinishedStageRepository).Returns(finishedStageRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterStageEnd(raceId, teamId, uniqueId, stageId));

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
            var uniqueId = "unique";

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            SetupMocksForRegisterToRace(raceId, teamId, teamRepositoryMock, new Mock<IGenericRepository<RegisteredId>>(),
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } }, DateTime.Now.AddDays(1));

            var stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            stageRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(stageId)))).Returns(new Stage { Number = 1 });
            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);

            var finishedStageRepositoryMock = new Mock<IGenericRepository<FinishedStage>>();
            _UnitOfWorkMock.Setup(u => u.FinishedStageRepository).Returns(finishedStageRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterStageEnd(raceId, teamId, uniqueId, stageId));

            Assert.AreEqual(BLErrorCodes.RaceNotStarted, exception.ErrorCode);
            Assert.AreEqual($"Race not started yet", exception.Message);

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            finishedStageRepositoryMock.Verify(r => r.Insert(It.IsAny<FinishedStage>()), Times.Never);
        }

        [TestMethod]
        public void RegisterRaceEndNoErrors()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var uniqueId = "unique";

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            SetupMocksForRegisterToRace(raceId, teamId, teamRepositoryMock, new Mock<IGenericRepository<RegisteredId>>(), 
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } });

            _Sut.RegisterRaceEnd(raceId, teamId, uniqueId);

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);

            teamRepositoryMock.Verify(r => r.Update(It.Is<Team>(t => 
            t.FinishTime.Year == DateTime.Now.Year &&
            t.FinishTime.Month == DateTime.Now.Month &&
            t.FinishTime.Day == DateTime.Now.Day &&
            t.FinishTime.Hour == DateTime.Now.Hour &&
            t.FinishTime.Minute == DateTime.Now.Minute &&
            t.FinishTime.Second == DateTime.Now.Second)), Times.Once);

            _LoggerMock.Verify(
             m => m.Log(
                 LogLevel.Error,
                 It.IsAny<EventId>(),
                 It.IsAny<FormattedLogValues>(),
                 It.IsAny<Exception>(),
                 It.IsAny<Func<object, Exception, string>>()),
             Times.Never);
        }

        [TestMethod]
        public void RegisterRaceEndUnknownRace()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var uniqueId = "unique";

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            SetupMocksForRegisterToRace(Guid.NewGuid(), teamId, teamRepositoryMock, new Mock<IGenericRepository<RegisteredId>>(),
                new List<RegisteredId>());

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterRaceEnd(raceId, teamId, uniqueId));

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            AssertUnknownRace(raceId, exception);
        }

        [TestMethod]
        public void RegisterRaceEndUnknownTeam()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var uniqueId = "unique";

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            SetupMocksForRegisterToRace(raceId, Guid.NewGuid(), teamRepositoryMock, new Mock<IGenericRepository<RegisteredId>>(),
                new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } });

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterRaceEnd(raceId, teamId, uniqueId));

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            AssertUnknownTeam(teamId, exception);
        }
        
        [TestMethod]
        public void RegisterRaceEndUnknownUniqueId()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var uniqueId = "unique";

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            SetupMocksForRegisterToRace(raceId, teamId, teamRepositoryMock, new Mock<IGenericRepository<RegisteredId>>(), null);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterRaceEnd(raceId, teamId, uniqueId));

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            AssertUnknownUniqueId(teamId, uniqueId, exception);
        }

        private void SetupMocksForRegisterToRace(Guid raceId, Guid teamId, Mock<IGenericRepository<Team>> teamRepositoryMock, Mock<IGenericRepository<RegisteredId>> regiteredIdRepositoryMock, List<RegisteredId> registeredIds, DateTime? startTime = null)
        {
            if (!startTime.HasValue)
            {
                startTime = DateTime.Now;
            }

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            raceRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(raceId)))).Returns(new Race { MaximumTeamSize = 2, StartTime = startTime.Value });
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            teamRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g=>g.Equals(teamId)))).Returns(new Team { TeamId = teamId, ActiveStage = 1 });
            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);

            regiteredIdRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<RegisteredId, bool>>>(),
                        It.IsAny<Func<IQueryable<RegisteredId>, IOrderedQueryable<RegisteredId>>>(),
                        It.IsAny<string>())).Returns(registeredIds);
            _UnitOfWorkMock.Setup(u => u.RegisteredIdRepository).Returns(regiteredIdRepositoryMock.Object);
        }

        private void AssertUnknownRace(Guid raceId, BusinessException exception)
        {
            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.AreEqual($"Race with ID '{raceId}' does not exsist.", exception.Message);

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
            _LoggerMock.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().Equals($"Error in AppApiBL: Someone tried to access race with ID '{raceId}' but it does not exsist.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }

        private void AssertUnknownTeam(Guid teamId, BusinessException exception)
        {
            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.AreEqual($"Team with ID '{teamId}' is unknown", exception.Message);

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
            _LoggerMock.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().Equals($"Error in AppApiBL: Someone tried to access team with ID '{teamId}' but it does not exsist.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }

        private void AssertUnknownStage(Guid stageId, BusinessException exception)
        {
            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.AreEqual($"Stage with ID '{stageId}' not found.", exception.Message);

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
            _LoggerMock.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().Equals($"Error in AppApiBL: Someone tried to access stage with id '{stageId}' but it does not exsist.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }

        private void AssertUnknownUniqueId(Guid teamId, string uniqueId, BusinessException exception)
        {
            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);
            Assert.AreEqual($"Unauthorized action, team id '{teamId}' and unique id '{uniqueId}' do not match.", exception.Message);

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
            _LoggerMock.Verify(
            m => m.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().Equals($"Error in AppApiBL: Someone tried to register a point but the team id '{teamId}' and unique id '{uniqueId}' do not match.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Once);
        }
    }
}
