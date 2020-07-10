using RaceVentura;
using RaceVentura.Models;
using RaceVentura.Races;
using RaceVenturaData;
using RaceVenturaData.Models.Races;
using RaceVenturaData.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace RaceVenturaTest.Races
{
    [TestClass]
    public class TeamBaseBLTests
    {
        private readonly Mock<ILogger> _LoggerMock = new Mock<ILogger>();
        private readonly Mock<IRaceVenturaUnitOfWork> _UnitOfWorkMock = new Mock<IRaceVenturaUnitOfWork>();
        private TestTeamBaseBL _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new TestTeamBaseBL(_UnitOfWorkMock.Object, _LoggerMock.Object);
        }

        [TestMethod]
        public void GetTeamNotFound()
        {
            var teamId = Guid.NewGuid();

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.GetTeam(teamId));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.AreEqual($"Team with ID '{teamId}' is unknown", exception.Message);
        }

        [TestMethod]
        public void GetTeamFound()
        {
            var teamId = Guid.NewGuid();

            var team = new Team();

            var teamRepositoryMock = new Mock<IGenericRepository<Team>>();
            teamRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(x => x.Equals(teamId)))).Returns(team);

            _UnitOfWorkMock.Setup(u => u.TeamRepository).Returns(teamRepositoryMock.Object);

            var result = _Sut.GetTeam(teamId);

            Assert.IsInstanceOfType(result, typeof(Team));
            Assert.AreEqual(team, result);
        }

        private class TestTeamBaseBL : TeamBaseBL
        {
            public TestTeamBaseBL(IRaceVenturaUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
            {

            }

            public new Team GetTeam(Guid teamId)
            {
                return base.GetTeam(teamId);
            }
        }
    }
}
