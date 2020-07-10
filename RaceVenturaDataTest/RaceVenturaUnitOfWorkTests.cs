using RaceVenturaData;
using RaceVenturaData.DatabaseContext;
using RaceVenturaData.Models;
using RaceVenturaData.Models.Races;
using RaceVenturaData.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;

namespace RaceVenturaDataTest
{
    [TestClass]
    public class RaceVenturaUnitOfWorkTests
    {
        private Mock<IRaceVenturaDbContext> _DbContextMock;
        private RaceVenturaUnitOfWork _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            _DbContextMock = new Mock<IRaceVenturaDbContext>();
            _Sut = new RaceVenturaUnitOfWork(_DbContextMock.Object);
        }

        [TestMethod]
        public void UnitOfWorkPropertiesTypesTest()
        {
            Assert.IsInstanceOfType(_Sut.PointRepository, typeof(GenericRepository<Point>));
            Assert.IsInstanceOfType(_Sut.RaceRepository, typeof(GenericRepository<Race>));
            Assert.IsInstanceOfType(_Sut.StageRepository, typeof(GenericRepository<Stage>));
            Assert.IsInstanceOfType(_Sut.TeamRepository, typeof(GenericRepository<Team>));
            Assert.IsInstanceOfType(_Sut.UserLinkRepository, typeof(GenericRepository<UserLink>));
            Assert.IsInstanceOfType(_Sut.VisitedPointRepository, typeof(GenericRepository<VisitedPoint>));
            Assert.IsInstanceOfType(_Sut.FinishedStageRepository, typeof(GenericRepository<FinishedStage>));
            Assert.IsInstanceOfType(_Sut.RegisteredIdRepository, typeof(GenericRepository<RegisteredId>));
        }

        [TestMethod]
        public void DisposeTest()
        {
            _Sut.Dispose();
            _DbContextMock.Verify(mock => mock.Dispose(), Times.Once());
        }

        [TestMethod]
        public void SaveTest()
        {
            _Sut.Save();
            _DbContextMock.Verify(mock => mock.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void SaveAsyncTest()
        {
            _Sut.SaveAsync();
            _DbContextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
