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

            SetupMocksForRegisterToRace(raceId, new List<RegisteredId>());

            var regiteredIdRepositoryMock = new Mock<IGenericRepository<RegisteredId>>();
            _UnitOfWorkMock.Setup(u => u.RegisteredIdRepository).Returns(regiteredIdRepositoryMock.Object);

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

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterToRace(raceId, teamId, uniqueId));

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

        [TestMethod]
        public void RegisterToRaceToManyIds()
        {
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var uniqueId = "unique";

            SetupMocksForRegisterToRace(raceId, new List<RegisteredId> { new RegisteredId(), new RegisteredId() });

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterToRace(raceId, teamId, uniqueId));

            Assert.AreEqual(BLErrorCodes.MaxIdsReached, exception.ErrorCode);
            Assert.AreEqual($"Maximum of registered ID's reached.", exception.Message);

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

            SetupMocksForRegisterToRace(raceId, new List<RegisteredId> { new RegisteredId { TeamId = teamId, UniqueId = uniqueId } });

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.RegisterToRace(raceId, teamId, uniqueId));

            Assert.AreEqual(BLErrorCodes.Duplicate, exception.ErrorCode);
            Assert.AreEqual($"Unique ID '{uniqueId}' allready registered.", exception.Message);

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

        private void SetupMocksForRegisterToRace(Guid raceId, List<RegisteredId> ids)
        {
            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            raceRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(raceId)))).Returns(new Race { MaximumTeamSize = 2 });
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            var teamMock = new Team
            {
                RegisteredIds = ids
            };

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            teamRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<Team, bool>>>(),
                        It.IsAny<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
                        It.IsAny<string>())).Returns(new List<Team> { teamMock });
            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);
        }
    }
}
