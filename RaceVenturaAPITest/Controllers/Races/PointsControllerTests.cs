using RaceVenturaAPI.Controllers.Races;
using RaceVenturaAPI.ViewModels.Races;
using RaceVenturaData.Models.Races;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RaceVenturaAPITest.Controllers.Races
{
    [TestClass]
    public class PointsControllerTests : ApiControllerCudTestsBase<PointsController, Point, PointViewModel>
    {
        [TestInitialize]
        public void InitializeTest()
        {
            Sut = new PointsController(_BLMock.Object, _MapperMock.Object, _LoggerMock.Object);

            SetControllerContext(Sut);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new PointsController(null, _MapperMock.Object, _LoggerMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new PointsController(_BLMock.Object, null, _LoggerMock.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new PointsController(_BLMock.Object, _MapperMock.Object, null));
        }

        [TestMethod]
        public void AddPointNoErrorsTest()
        {
            AddNoErrorsTest(Sut);
        }

        [TestMethod]
        public void AddPointInvalidModelStateTest()
        {
            AddInvalidModelStateTest(Sut);
        }

        [TestMethod]
        public void AddPointBusinesErrorTest()
        {
            AddBusinessErrorTest(Sut, SetupBlAddBusinessException);
        }

        [TestMethod]
        public void AddPointExceptionTest()
        {
            AddExceptionTest(Sut, SetupBlAddException);
        }

        [TestMethod]
        public void EditPointNoErrorsTest()
        {
            EditNoErrorsTest(Sut);
        }

        [TestMethod]
        public void EditPointInvalidModelStateTest()
        {
            EditInvalidModelStateTest(Sut);
        }

        [TestMethod]
        public void EditPointBusinesErrorTest()
        {
            EditBusinessErrorTest(Sut, SetupBlEditBusinessException);
        }

        [TestMethod]
        public void EditPointExceptionTest()
        {
            EditExceptionTest(Sut, SetupBlEditException);
        }

        [TestMethod]
        public void DeletePointNoErrorsTest()
        {
            DeleteNoErrorsTest(Sut);
        }

        [TestMethod]
        public void DeletePointBusinesErrorTest()
        {
            DeleteBusinessErrorTest(Sut, SetupBlDeleteBusinessException);
        }

        [TestMethod]
        public void DeletePointExceptionTest()
        {
            DeleteExceptionTest(Sut, SetupBlDeleteException);
        }
    }
}
