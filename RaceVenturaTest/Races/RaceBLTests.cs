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
    public class RaceBLTests
    {
        private readonly Mock<ILogger<RaceBL>> _LoggerMock = new Mock<ILogger<RaceBL>>();
        private readonly Mock<IRaceVenturaUnitOfWork> _UnitOfWorkMock = new Mock<IRaceVenturaUnitOfWork>();
        private RaceBL _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new RaceBL(_UnitOfWorkMock.Object, _LoggerMock.Object);
        }

        [TestMethod]
        public void Get()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            userLinkRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>())).Returns(new[] { new UserLink { UserId = userId, RaceId = raceId } });
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            _Sut.Get(userId);

            userLinkRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>()), Times.Once);

            raceRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<Race, bool>>>(),
                It.IsAny<Func<IQueryable<Race>, IOrderedQueryable<Race>>>(),
                It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetById()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var race = new Race
            {
                RaceId = raceId,
                Teams = new List<Team>
                {
                    new Team { Number = 2 },
                    new Team { Number = 1 }
                },
                Stages = new List<Stage>
                {
                    new Stage { Number = 2 },
                    new Stage
                    {
                        Number = 1,
                        Points = new List<Point>
                        {
                            new Point { Name = "b" },
                            new Point { Name = "a" }
                        }
                    }
                }
            };

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            raceRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<Race, bool>>>(),
                It.IsAny<Func<IQueryable<Race>, IOrderedQueryable<Race>>>(),
                It.Is<string>(s => s.Equals("Teams,Teams.VisitedPoints,Teams.FinishedStages,Stages,Stages.Points"))))
                .Returns(new[] { race });
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            userLinkRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>())).Returns(new[] { new UserLink { UserId = userId, RaceId = raceId } });
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            var result = _Sut.GetById(userId, raceId);

            Assert.IsInstanceOfType(result, typeof(Race));
            Assert.AreEqual(1, race.Teams[0].Number);
            Assert.AreEqual(1, race.Stages[0].Number);
            Assert.AreEqual("a", race.Stages[0].Points[0].Name);

            userLinkRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>()), Times.Once);

            raceRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<Race, bool>>>(),
                It.IsAny<Func<IQueryable<Race>, IOrderedQueryable<Race>>>(),
                It.Is<string>(s => s.Equals("Teams,Teams.VisitedPoints,Teams.FinishedStages,Stages,Stages.Points"))), Times.Once);
        }

        [TestMethod]
        public void GetByIdNoUserLink()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.GetById(userId, raceId));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);
            Assert.AreEqual($"User is not authorized for race.", exception.Message);

            userLinkRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>()), Times.Once);

            raceRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<Race, bool>>>(),
                It.IsAny<Func<IQueryable<Race>, IOrderedQueryable<Race>>>(),
                It.IsAny<string>()), Times.Never);

            _LoggerMock.VerifyLog(LogLevel.Warning, Times.Once, $"Error in RaceBL: User with ID '{userId}' tried to access race with ID '{raceId}' but is unauthorized.");
        }

        [TestMethod]
        public void GetByIdRaceNotFound()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            userLinkRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>())).Returns(new[] { new UserLink { UserId = userId, RaceId = raceId } });
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.GetById(userId, raceId));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.AreEqual($"No race with ID {raceId} found.", exception.Message);

            userLinkRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>()), Times.Once);

            raceRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<Race, bool>>>(),
                It.IsAny<Func<IQueryable<Race>, IOrderedQueryable<Race>>>(),
                It.Is<string>(s => s.Equals("Teams,Teams.VisitedPoints,Teams.FinishedStages,Stages,Stages.Points"))), Times.Once);
        }

        [TestMethod]
        public void Add()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var race = new Race
            {
                RaceId = raceId
            };

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            _Sut.Add(userId, race);

            raceRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<Race, bool>>>(),
                It.IsAny<Func<IQueryable<Race>, IOrderedQueryable<Race>>>(),
                It.IsAny<string>()), Times.Once);
            raceRepositoryMock.Verify(r => r.Insert(It.Is<Race>(x => x.Equals(race))), Times.Once);

            userLinkRepositoryMock.Verify(r => r.Insert(It.Is<UserLink>(l => l.UserId.Equals(userId) && l.RaceId.Equals(raceId))), Times.Once);

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Exactly(2));
        }

        [TestMethod]
        public void AddNameExists()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var mockRace = new Race
            {
                RaceId = raceId,
                Name = "test"
            };

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            raceRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<Race, bool>>>(),
                It.IsAny<Func<IQueryable<Race>, IOrderedQueryable<Race>>>(),
                It.IsAny<string>())).Returns(new List<Race> { mockRace });
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            var race = new Race
            {
                RaceId = raceId,
                Name = "Test"
            };
            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Add(userId, race));

            Assert.AreEqual(BLErrorCodes.Duplicate, exception.ErrorCode);
            Assert.AreEqual($"A race with name '{race.Name}' already exists.", exception.Message);

            raceRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<Race, bool>>>(),
                It.IsAny<Func<IQueryable<Race>, IOrderedQueryable<Race>>>(),
                It.IsAny<string>()), Times.Once);
            raceRepositoryMock.Verify(r => r.Insert(It.Is<Race>(x => x.Equals(mockRace))), Times.Never);

            userLinkRepositoryMock.Verify(r => r.Insert(It.IsAny<UserLink>()), Times.Never);

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void Edit()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var raceMock = new Race
            {
                RaceId = raceId,
                CoordinatesCheckEnabled = false,
                AllowedCoordinatesDeviation = 1,
                MaxDuration = null,
                MaximumTeamSize = 1,
                MinimumPointsToCompleteStage = 2,
                PenaltyPerMinuteLate = 3,
                SpecialTasksAreStage = false,
                StartTime = DateTime.Now,
                Name = "Test",
            };


            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            userLinkRepositoryMock.Setup(r => r.Get(
                It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>())).Returns(new List<UserLink> { new UserLink() });
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            raceRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(x => x.Equals(raceId)))).Returns(raceMock);

            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            var race = new Race
            {
                RaceId = raceId,
                CoordinatesCheckEnabled = true,
                AllowedCoordinatesDeviation = 2,
                MaxDuration = new TimeSpan(),
                MaximumTeamSize = 4,
                MinimumPointsToCompleteStage = 5,
                PenaltyPerMinuteLate = 6,
                SpecialTasksAreStage = true,
                StartTime = DateTime.Now,
                Name = "Blaat"
            };

            _Sut.Edit(userId, race);

            raceRepositoryMock.Verify(r => r.Update(It.Is<Race>(x =>
                x.CoordinatesCheckEnabled == race.CoordinatesCheckEnabled &&
                x.MaxDuration.Equals(race.MaxDuration) &&
                x.MaximumTeamSize == race.MaximumTeamSize &&
                x.MinimumPointsToCompleteStage == race.MinimumPointsToCompleteStage &&
                x.Name.Equals(race.Name) &&
                x.PenaltyPerMinuteLate == race.PenaltyPerMinuteLate &&
                x.SpecialTasksAreStage == race.SpecialTasksAreStage &&
                x.StartTime.Equals(race.StartTime)
            )), Times.Once);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        public void EditRaceDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            SetupUserLinkAndRaceRepositoryMock(new List<UserLink> { new UserLink() }, Guid.NewGuid(), out var raceRepositoryMock);

            var race = new Race
            {
                RaceId = raceId
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, race));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);

            raceRepositoryMock.Verify(r => r.Update(It.IsAny<Race>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void EditNotAuthorizedForRace()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            SetupUserLinkAndRaceRepositoryMock(new List<UserLink> { }, raceId, out var raceRepositoryMock);

            var race = new Race
            {
                RaceId = raceId
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, race));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);

            raceRepositoryMock.Verify(r => r.Update(It.IsAny<Race>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void EditWrongAccessLevelForRace()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            SetupUserLinkAndRaceRepositoryMock(new List<UserLink> { new UserLink { UserId = userId, RaceId = raceId, RaceAccess = RaceAccessLevel.WriteTeams } }, raceId, out var raceRepositoryMock);

            var race = new Race
            {
                RaceId = raceId
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, race));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);

            raceRepositoryMock.Verify(r => r.Update(It.IsAny<Race>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void EditRaceNameExists()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            SetupUserLinkAndRaceRepositoryMock(new List<UserLink> { new UserLink() }, raceId, out var raceRepositoryMock);
            raceRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<Race, bool>>>(),
                    It.IsAny<Func<IQueryable<Race>, IOrderedQueryable<Race>>>(),
                    It.IsAny<string>())).Returns(new List<Race>
                    {
                        new Race{ RaceId = raceId, Name = "test" }
                    });

            var race = new Race
            {
                RaceId = raceId,
                Name = "Test"
            };

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Edit(userId, race));

            Assert.AreEqual(BLErrorCodes.Duplicate, exception.ErrorCode);
            Assert.AreEqual($"A race with name '{race.Name}' already exists.", exception.Message);

            raceRepositoryMock.Verify(r => r.Update(It.IsAny<Race>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void Delete()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var raceMock = new Race
            {
                RaceId = raceId
            };

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            raceRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(x => x.Equals(raceId)))).Returns(raceMock);
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            userLinkRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>())).Returns(new[] { new UserLink { UserId = userId, RaceId = raceId, RaceAccess = RaceAccessLevel.Owner } });
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            _Sut.Delete(userId, raceId);

            raceRepositoryMock.Verify(r => r.Delete(It.Is<Guid>(r => r.Equals(raceId))), Times.Once);
            userLinkRepositoryMock.Verify(r => r.Delete(It.Is<UserLink>(l => l.UserId.Equals(userId))), Times.Once);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        public void DeleteNoUserLink()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            raceRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(raceId))))
                .Returns(new Race { RaceId = raceId, Name = "test1" });
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Delete(userId, raceId));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);
            Assert.AreEqual($"User is not authorized for race.", exception.Message);

            userLinkRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>()), Times.Once);
            userLinkRepositoryMock.Verify(r => r.Delete(It.IsAny<object>()), Times.Never);

            raceRepositoryMock.Verify(r => r.Delete(It.IsAny<object>()), Times.Never);

            _LoggerMock.VerifyLog(LogLevel.Warning, Times.Once, $"Error in RaceBL: User with ID '{userId}' tried to access race with ID '{raceId}' but is unauthorized.");

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void DeleteNoOwner()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var raceMock = new Race
            {
                RaceId = raceId
            };

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            raceRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(x => x.Equals(raceId)))).Returns(raceMock);
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            userLinkRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>())).Returns(new[] { new UserLink { UserId = userId, RaceId = raceId, RaceAccess = RaceAccessLevel.ReadWrite } });
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Delete(userId, raceId));

            Assert.AreEqual(BLErrorCodes.UserUnauthorized, exception.ErrorCode);
            Assert.AreEqual($"User is not authorized for race.", exception.Message);

            userLinkRepositoryMock.Verify(r => r.Get(It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>()), Times.Once);
            userLinkRepositoryMock.Verify(r => r.Delete(It.IsAny<object>()), Times.Never);

            raceRepositoryMock.Verify(r => r.Delete(It.IsAny<object>()), Times.Never);

            _LoggerMock.VerifyLog(LogLevel.Warning, Times.Once, $"Error in RaceBL: User with ID '{userId}' tried to access race with ID '{raceId}' but is unauthorized.");

            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        [TestMethod]
        public void DeleteNoRace()
        {
            var userId = Guid.NewGuid();
            var raceId = Guid.NewGuid();

            var raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);

            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.Delete(userId, raceId));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.AreEqual($"Race with ID '{raceId}' does not exsist.", exception.Message);

            userLinkRepositoryMock.Verify(r => r.Delete(It.IsAny<object>()), Times.Never);
            raceRepositoryMock.Verify(r => r.Delete(It.IsAny<object>()), Times.Never);
            _UnitOfWorkMock.Verify(u => u.Save(), Times.Never);
        }

        private void SetupUserLinkAndRaceRepositoryMock(List<UserLink> userLinks, Guid raceId, out Mock<IGenericRepository<Race>> raceRepositoryMock)
        {
            var userLinkRepositoryMock = new Mock<IGenericRepository<UserLink>>();
            userLinkRepositoryMock.Setup(r => r.Get(
                It.IsAny<Expression<Func<UserLink, bool>>>(),
                It.IsAny<Func<IQueryable<UserLink>, IOrderedQueryable<UserLink>>>(),
                It.IsAny<string>())).Returns(userLinks);
            _UnitOfWorkMock.Setup(u => u.UserLinkRepository).Returns(userLinkRepositoryMock.Object);

            raceRepositoryMock = new Mock<IGenericRepository<Race>>();
            raceRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(g => g.Equals(raceId))))
                .Returns(new Race { RaceId = raceId, Name = "test1" });
            _UnitOfWorkMock.Setup(u => u.RaceRepository).Returns(raceRepositoryMock.Object);
        }
    }
}
