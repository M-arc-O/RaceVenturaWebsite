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
    public class StageBaseBLTests
    {
        private readonly Mock<ILogger> _LoggerMock = new Mock<ILogger>();
        private readonly Mock<IRaceVenturaUnitOfWork> _UnitOfWorkMock = new Mock<IRaceVenturaUnitOfWork>();
        private TestStageBaseBL _Sut;

        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new TestStageBaseBL(_UnitOfWorkMock.Object, _LoggerMock.Object);
        }

        [TestMethod]
        public void GetStageNotFound()
        {
            var stageId = Guid.NewGuid();

            var stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);

            var exception = Assert.ThrowsException<BusinessException>(() => _Sut.GetStage(stageId));

            Assert.AreEqual(BLErrorCodes.NotFound, exception.ErrorCode);
            Assert.AreEqual($"Stage with ID '{stageId}' is unknown", exception.Message);
        }

        [TestMethod]
        public void GetStageFound()
        {
            var stageId = Guid.NewGuid();

            var stage = new Stage();

            var stageRepositoryMock = new Mock<IGenericRepository<Stage>>();
            stageRepositoryMock.Setup(r => r.GetByID(It.Is<Guid>(x => x.Equals(stageId)))).Returns(stage);

            _UnitOfWorkMock.Setup(u => u.StageRepository).Returns(stageRepositoryMock.Object);

            var result = _Sut.GetStage(stageId);

            Assert.IsInstanceOfType(result, typeof(Stage));
            Assert.AreEqual(stage, result);
        }

        private class TestStageBaseBL : StageBaseBL
        {
            public TestStageBaseBL(IRaceVenturaUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
            {

            }

            public new Stage GetStage(Guid stageId)
            {
                return base.GetStage(stageId);
            }
        }
    }
}
