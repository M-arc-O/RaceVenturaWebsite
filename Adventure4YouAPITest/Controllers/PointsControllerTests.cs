using Adventure4YouAPI.Controllers;
using Adventure4YouAPI.ViewModels.Races;
using Adventure4YouData.Models.Races;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Adventure4YouAPITest.Controllers
{
    [TestClass]
    public class PointsControllerTests : ApiControllerCudTestsBase<PointsController, Point, PointViewModel>
    {
        [TestInitialize]
        public void InitializeTest()
        {
            _Sut = new PointsController(_BLMock.Object, _MapperMock.Object, _LoggerMock.Object);

            SetControllerContext(_Sut);
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
            AddNoErrorsTest(_Sut);
        }

        [TestMethod]
        public void AddPointInvalidModelStateTest()
        {
            AddInvalidModelStateTest(_Sut);
        }

        [TestMethod]
        public void AddPointBusinesErrorTest()
        {
            AddBusinessErrorTest(_Sut, SetupBlAddBusinessException);
        }

        [TestMethod]
        public void AddPointExceptionTest()
        {
            AddExceptionTest(_Sut, SetupBlAddException);
        }

        [TestMethod]
        public void EditPointNoErrorsTest()
        {
            EditNoErrorsTest(_Sut);
        }

        [TestMethod]
        public void EditPointInvalidModelStateTest()
        {
            EditInvalidModelStateTest(_Sut);
        }

        [TestMethod]
        public void EditPointBusinesErrorTest()
        {
            EditBusinessErrorTest(_Sut, SetupBlEditBusinessException);
        }

        [TestMethod]
        public void EditPointExceptionTest()
        {
            EditExceptionTest(_Sut, SetupBlEditException);
        }

        [TestMethod]
        public void DeletePointNoErrorsTest()
        {
            DeleteNoErrorsTest(_Sut);
        }

        [TestMethod]
        public void DeletePointBusinesErrorTest()
        {
            DeleteBusinessErrorTest(_Sut, SetupBlDeleteBusinessException);
        }

        [TestMethod]
        public void DeletePointExceptionTest()
        {
            DeleteExceptionTest(_Sut, SetupBlDeleteException);
        }
    }
}
