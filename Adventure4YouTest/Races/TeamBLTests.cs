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
    public class TeamBLTests
    {
        private readonly Mock<ILogger<TeamBL>> _LoggerMock = new Mock<ILogger<TeamBL>>();
        private readonly Mock<IAdventure4YouUnitOfWork> _UnitOfWorkMock = new Mock<IAdventure4YouUnitOfWork>();
        private TeamBL _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new TeamBL(_UnitOfWorkMock.Object, _LoggerMock.Object);
        }

        [TestMethod]
        public void Add()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var team = new Team
            {
                RaceId = raceId
            };

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            _Sut.Add(userId, team);

            teamRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<Team, bool>>>(),
                It.IsAny<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
                It.IsAny<string>()), Times.Exactly(2));
            teamRepositoryMock.Verify(r => r.Insert(It.Is<Team>(x => x.Equals(team))), Times.Once);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        public void AddRaceDoesNotExists()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var team = new Team
            {
                RaceId = raceId
            };

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, Guid.NewGuid());

            var Exception = Assert.ThrowsException<BusinessException>(() => _Sut.Add(userId, team));

            Assert.AreEqual(BLErrorCodes.NotFound, Exception.ErrorCode);

            teamRepositoryMock.Verify(r => r.Insert(It.IsAny<Team>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void AddNotAuthorizedForRace()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var team = new Team
            {
                RaceId = raceId
            };

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink>(), raceId);

            var Exception = Assert.ThrowsException<BusinessException>(() => _Sut.Add(Guid.NewGuid(), team));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, Exception.ErrorCode);

            teamRepositoryMock.Verify(r => r.Insert(It.IsAny<Team>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void AddNameExists()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var teamMock = new Team
            {
                RaceId = raceId,
                Name = "test"
            };

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            teamRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<Team, bool>>>(),
                It.IsAny<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
                It.IsAny<string>())).Returns(new List<Team> { teamMock });

            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var team = new Team
            {
                RaceId = raceId,
                Name = "test"
            };
            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Add(userId, team));

            Assert.AreEqual(BLErrorCodes.Duplicate, exception.ErrorCode);
            Assert.AreEqual($"A team with name '{team.Name}' already exists.", exception.Message);

            teamRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<Team, bool>>>(),
                It.IsAny<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
                It.IsAny<string>()), Times.Once);
            teamRepositoryMock.Verify(r => r.Insert(It.Is<Team>(x => x.Equals(team))), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void AddNumberExists()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var mockTeam = new Team
            {
                RaceId = raceId,
                Name = "",
                Number = 1
            };

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            teamRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<Team, bool>>>(),
                It.IsAny<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
                It.IsAny<string>())).Returns(new List<Team> { mockTeam });

            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var team = new Team
            {
                RaceId = raceId,
                Name = "Test",
                Number = 1
            };
            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Add(userId, team));

            Assert.AreEqual(BLErrorCodes.Duplicate, exception.ErrorCode);
            Assert.AreEqual($"A team with number '{team.Number}' already exists.", exception.Message);

            teamRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<Team, bool>>>(),
                It.IsAny<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
                It.IsAny<string>()), Times.Exactly(2));
            teamRepositoryMock.Verify(r => r.Insert(It.Is<Team>(x => x.Equals(mockTeam))), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void Edit()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();

            var teamMock = new Team
            {
                TeamId = teamId,
                RaceId = raceId,
                Name = "Test",
                Number = 1,
                FinishTime = DateTime.Now
            };

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            teamRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(x => x.Equals(teamId)))).Returns(teamMock);

            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var team = new Team
            {
                TeamId = teamId,
                RaceId = raceId,
                Name = "Blaat",
                Number = 2,
                FinishTime = DateTime.Now
            };

            _Sut.Edit(userId, team);

            teamRepositoryMock.Verify(r => r.Update(It.Is<Team>(x =>
                x.Name.Equals(team.Name) &&
                x.Number == team.Number &&
                x.FinishTime.Equals(team.FinishTime)
            )), Times.Once);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        public void EditTeamDoesNotExists()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            
            SetupTeamRepositoryMock(raceId, teamId, out var teamRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var team = new Team
            {
                TeamId = Guid.NewGuid()
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, team));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void EditRaceDoesNotExists()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();

            SetupTeamRepositoryMock(raceId, teamId, out var teamRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, Guid.NewGuid());

            var team = new Team
            {
                TeamId = teamId,
                RaceId = raceId
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, team));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.IsTrue(exception.Message.Contains(raceId.ToString()));

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void EditNotAuthorizedForRace()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();

            SetupTeamRepositoryMock(raceId, teamId, out var teamRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { }, raceId);

            var team = new Team
            {
                TeamId = teamId,
                RaceId = raceId
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, team));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void EditTeamNameExists()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();

            var teamMock = new Team
            {
                TeamId = teamId,
                RaceId = raceId,
                Name = "Test",
                Number = 1,
                FinishTime = DateTime.Now
            };

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            teamRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(x => x.Equals(teamId)))).Returns(teamMock);
            teamRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<Team, bool>>>(),
                It.IsAny<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
                It.IsAny<string>())).Returns(new List<Team>
                {
                    new Team{ RaceId = raceId, Name = "Test2" }
                });

            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var team = new Team
            {
                TeamId = teamId,
                Name = "test2",
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, team));

            Assert.AreEqual(BLErrorCodes.Duplicate, exception.ErrorCode);
            Assert.AreEqual($"A team with name '{team.Name}' already exists.", exception.Message);

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void EditTeamNumberExists()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();

            var teamMock = new Team
            {
                TeamId = teamId,
                RaceId = raceId,
                Name = "Test",
                Number = 1,
                FinishTime = DateTime.Now
            };

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            teamRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(x => x.Equals(teamId)))).Returns(teamMock);
            teamRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<Team, bool>>>(),
                It.IsAny<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
                It.IsAny<string>())).Returns(new List<Team>
                {
                    new Team{ RaceId = raceId, Number = 3 }
                });

            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var team = new Team
            {
                TeamId = teamId,
                Name = "Test",
                Number = 3,
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, team));

            Assert.AreEqual(BLErrorCodes.Duplicate, exception.ErrorCode);
            Assert.AreEqual($"A team with number '{team.Number}' already exists.", exception.Message);

            teamRepositoryMock.Verify(r => r.Update(It.IsAny<Team>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void Delete()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();

            var teamMock = new Team
            {
                TeamId = teamId,
                RaceId = raceId,
            };

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            teamRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(x => x.Equals(teamId)))).Returns(teamMock);

            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            _Sut.Delete(userId, teamId);

            teamRepositoryMock.Verify(r => r.Delete(It.Is<Guid>(g => g.Equals(teamId))), Times.Once);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        public void DeleteTeamDoesNotExists()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();

            SetupTeamRepositoryMock(raceId, teamId, out var teamRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, raceId);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Delete(userId, Guid.NewGuid()));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);

            teamRepositoryMock.Verify(r => r.Delete(It.Is<Guid>(g => g.Equals(teamId))), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void DeleteRaceDoesNotExists()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();

            SetupTeamRepositoryMock(raceId, teamId, out var teamRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { new UserLink() }, Guid.NewGuid());

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Delete(userId, teamId));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.IsTrue(exception.Message.Contains(raceId.ToString()));

            teamRepositoryMock.Verify(r => r.Delete(It.Is<Guid>(g => g.Equals(teamId))), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void DeleteNotAuthorizedForRace()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();
            var teamId = Guid.NewGuid();

            SetupTeamRepositoryMock(raceId, teamId, out var teamRepositoryMock);

            TestUtils.SetupUnitOfWorkToPassAuthorizedAndRace(_UnitOfWorkMock, new List<UserLink> { }, raceId);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Delete(userId, teamId));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);

            teamRepositoryMock.Verify(r => r.Delete(It.Is<Guid>(g => g.Equals(teamId))), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        private Mock<IGenericRepository<Team>> SetupTeamRepositoryMock(Guid raceId, Guid teamId, out Mock<IGenericRepository<Team>> teamRepositoryMock)
        {
            var teamMock = new Team
            {
                TeamId = teamId,
                RaceId = raceId,
                Name = "Test",
                Number = 1,
                FinishTime = DateTime.Now
            };

            teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            teamRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(x => x.Equals(teamId)))).Returns(teamMock);

            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);
            return teamRepositoryMock;
        }
    }
}
